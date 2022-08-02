using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;
using Nezix;

public class potential : MonoBehaviour
{
    public static float[] backgroundPotential;
    static Vector3[,,] gridPoints;
    float[] potentials;
    MarchingCubesBurst mcb;
    public static float voltage = 1;
    public int gridWidth;
    public static bool potentialToggle = false;
    Vector3 oriXInv;

    int3 gridSize;

    public float dx;
    private void Start()
    {
        gridPoints = electricField.createGrid(gridWidth, dx * gridWidth, transform.position);
        backgroundPotential = new float[gridWidth * gridWidth * gridWidth];
        potentials = new float[gridWidth * gridWidth * gridWidth];
        oriXInv = new Vector3(-dx * gridWidth / 2.0f, -dx * gridWidth / 2.0f, -dx * gridWidth / 2.0f);
        gridSize.x = gridWidth;
        gridSize.y = gridWidth;
        gridSize.z = gridWidth;
        for (int x = 0; x < backgroundPotential.Length; x++)
        {
            backgroundPotential[x] = 0;
            potentials[x] = 0;
        }
        

    }


    private void Update()
    {
        if (!potentialToggle)
        {
            gameObject.GetComponent<MeshFilter>().mesh = new Mesh();
        }

        if (potentialToggle)
            {

            if(testParticle.nonStaticCharges.Count == 0)
            {
                for(int i = 0; i < backgroundPotential.Length; i++)
                {
                    potentials[i] = backgroundPotential[i];
                }
            }
            else
            {
                int x = 0;
                for (int i = 0; i < gridWidth; i++)
                {
                    for (int j = 0; j < gridWidth; j++)
                    {
                        for (int k = 0; k < gridWidth; k++)
                        {
                            potentials[x] = backgroundPotential[x] + calculateNonStaticPotential(gridPoints[i, j, k]);
                            x++;
                        }
                    }
                }
            }



                //Instantiate the MCB class
                mcb = new MarchingCubesBurst(potentials, gridSize, oriXInv, dx);

                //Compute an iso surface, this can be called several time without modifying mcb
                mcb.computeIsoSurface(voltage);

                Vector3[] newVerts = mcb.getVertices();
                Vector3[] newNorms = mcb.getNormals();

                if (newVerts.Length == 0)
                {
                    mcb.Clean();
                }

                int[] newTri = mcb.getTriangles();

                Mesh newMesh = new Mesh();
                newMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                newMesh.vertices = newVerts;
                newMesh.triangles = newTri;
                newMesh.normals = newNorms;

                gameObject.GetComponent<MeshFilter>().mesh = newMesh;

                //When done => free data
                mcb.Clean();


        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            toggleMarchingCube();
        }

    }


 /*   public static void equipotentialPoints(float voltage, float constraint)
    {
*//*        GameObject[] previousGridObjects = GameObject.FindGameObjectsWithTag("particleSystem");
        for (int i = 0; i < previousGridObjects.GetLength(0); i++)
        {
            Destroy(previousGridObjects[i]);
        }
        for (int i = 0; i < backgroundPotential.GetLength(0); i++)
        {
            for (int j = 0; j < backgroundPotential.GetLength(0); j++)
            {
                for (int k = 0; k < backgroundPotential.GetLength(0); k++)
                {
                    if (backgroundPotential[i, j, k] < voltage + constraint && backgroundPotential[i, j, k] > voltage - constraint)
                    {
                        Instantiate(gridObject, gridPoints[i, j, k], Quaternion.identity);
                    }
                }
            }
        }*/

/*        List<GameObject> equiParticleSystems = new List<GameObject>();
        GameObject[] allParticleSystems = GameObject.FindGameObjectsWithTag("particleSystem");
        for (int i = 0; i < backgroundPotential.GetLength(0); i++)
        {
            for (int j = 0; j < backgroundPotential.GetLength(0); j++)
            {
                for (int k = 0; k < backgroundPotential.GetLength(0); k++)
                {
                    if (backgroundPotential[i, j, k] < voltage + constraint && backgroundPotential[i, j, k] > voltage - constraint)
                    {
                        bool exists = false;
                        for (int z = 0; z < allParticleSystems.Length; z++)
                        {
                            if (allParticleSystems[z].transform.position == gridPoints[i, j, k])
                            {
                                equiParticleSystems.Add(allParticleSystems[z]);
                                exists = true;
                                break;
                            }
                        }
                        if (!exists)
                        {
                            GameObject temp = Instantiate(gridObject, gridPoints[i, j, k], Quaternion.identity);
                            equiParticleSystems.Add(temp);
                        }
                    }
                }
            }
        }
        allParticleSystems = GameObject.FindGameObjectsWithTag("particleSystem");

        for (int i = 0; i < allParticleSystems.Length; i++)
        {
            bool exists = false;
            for (int j = 0; j < equiParticleSystems.Count; j++)
            {
                if (allParticleSystems[i] == equiParticleSystems[j])
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                Destroy(allParticleSystems[i]);
            }
        }*/
    
    public static void toggleMarchingCube()
    {
        potentialToggle = !potentialToggle;
    }

    public static float calculatePotential(Vector3 position)
    {
        float potential = 0;

        GameObject[] charges = GameObject.FindGameObjectsWithTag("Charge");

        for (int i = 0; i < charges.Length; i++)
        {
            potential += charges[i].GetComponent<Charges>().charge * (0.2f / (position - charges[i].transform.position).magnitude);
        }
        return potential;
    }

    public static void updateBackgroundPotentials()
    {
        int x = 0;
        for (int i = 0; i < gridPoints.GetLength(0); i++)
        {
            for (int j = 0; j < gridPoints.GetLength(0); j++)
            {
                for (int k = 0; k < gridPoints.GetLength(0); k++)
                {
                    backgroundPotential[x] = calculateStaticPotential(gridPoints[i, j, k]);
                    x++;
                }
            }
        }
    }

    public static float calculateStaticPotential(Vector3 position)
    {
        float potential = 0;

        GameObject[] charges = GameObject.FindGameObjectsWithTag("Charge");

        for (int i = 0; i < charges.Length; i++)
        {
            if (charges[i].GetComponent<Charges>().backGround)
            {
                potential += charges[i].GetComponent<Charges>().charge * (0.2f / (position - charges[i].transform.position).magnitude);
            }
        }

        return potential;

    }
    public static float calculatePotential(Vector3 position, GameObject gameObject)
    {
        float potential = 0;

        if (gameObject.GetComponent<Charges>().backGround)
        {
            potential += gameObject.GetComponent<Charges>().charge * (0.2f / (position - gameObject.transform.position).magnitude);
        }

        return potential;

    }
    public static float calculateNonStaticPotential(Vector3 position)
    {
        float potential = 0;

        /*        if (leftControl.model)
                {
                    if (leftControl.model.tag == "Charge")
                    {
                        potential += leftControl.model.GetComponent<Charges>().charge * (0.2f / (position - leftControl.model.transform.position).magnitude);
                    }
                    else if (leftControl.model.tag == "Extended Object")
                    {
                        for (int i = 0; i < leftControl.model.transform.childCount; i++)
                        {
                            potential += leftControl.model.transform.GetChild(i).gameObject.GetComponent<Charges>().charge * ((0.2f / ((position - leftControl.model.transform.GetChild(i).position).magnitude)));

                        }
                    }
                }
                if (rightControl.objectInHand)
                {
                    if (rightControl.objectInHand.tag == "Charge")
                    {
                        potential += rightControl.objectInHand.GetComponent<Charges>().charge * (0.2f / (position - rightControl.objectInHand.transform.position).magnitude);
                    }
                    else if (rightControl.objectInHand.tag == "Extended Object")
                    {
                        for (int i = 0; i < rightControl.objectInHand.transform.childCount; i++)
                        {
                            potential += rightControl.objectInHand.transform.GetChild(i).gameObject.GetComponent<Charges>().charge * ((0.2f / ((position - rightControl.objectInHand.transform.GetChild(i).position).magnitude)));

                        }
                    }
                }*/
        for (int i = 0; i < testParticle.nonStaticCharges.Count; i++)
        {
            if (testParticle.nonStaticCharges[i])
            {
                Vector3 chargePosition = testParticle.nonStaticCharges[i].transform.position;
                potential += testParticle.nonStaticCharges[i].GetComponent<Charges>().charge * ((0.2f / ((position - chargePosition).magnitude))) ;
            }
            else
            {
                testParticle.nonStaticCharges.RemoveAt(i);
            }
        }

        return potential;
    }
}
