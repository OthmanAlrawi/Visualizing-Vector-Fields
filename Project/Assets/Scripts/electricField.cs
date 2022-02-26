using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class electricField : MonoBehaviour
{
    public GameObject arrow;
    public int gridResolution = 5;
    public int gridWidth = 4;
    public static float maxDistance;
    public int fieldLineResolution = 5;
    static GameObject[] arrows;
    public GameObject test;
    public GameObject charge;
    public float spawnInterval = 0.5f;
    public static float spawnInterval1 = 0.5f;
    public float lifeTime = 0.5f;
    public static float lifeTime1 = 0.5f;
    public GameObject tracer;
    Vector3[] sphereTracerSpawnPositions;
    public int tracerCount;
    public static Vector3[] backgroundField;
    public static float maxField = 0;
    public static float minField;
/*    public Color startColor = Color.black;
    public Color endColor = Color.black;*/
    public static bool fieldLinesToggle = false;
    public bool fieldLinesToggle0 = false;
    public bool electricFieldToggle0 = false;
    static bool electricFieldToggle;
    public static bool potentialToggle = true;
    public bool potentialToggle0 = true;
    void Start()
    {
        maxDistance = Mathf.Sqrt(3) * gridWidth;
        lifeTime1 = lifeTime;
        spawnInterval1 = spawnInterval;
        fieldLinesToggle = fieldLinesToggle0;
        InvokeRepeating("invokedSpawnTracers", 0, spawnInterval);
        Vector3[,,] grid = createGrid(gridResolution, gridWidth, transform.position);

        arrows = new GameObject[gridResolution * gridResolution * gridResolution];

        for (int i = 0, index = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(0); j++)
            {
                for (int k = 0; k < grid.GetLength(0); k++)
                {
                    arrows[index] = Instantiate(arrow, grid[i, j, k], Quaternion.identity);
                    arrows[index].transform.localScale = Vector3.zero;
                    index++;
                }
            }
        }
        sphereTracerSpawnPositions = findMeshTracerSpawnPositions(charge);
        backgroundField = new Vector3[arrows.Length];
        for (int i = 0; i < backgroundField.Length;  i++)
        {
            backgroundField[i] = Vector3.zero;
        }

    }

    public static void updateBackgroundField()
    {
        if (electricFieldToggle)
        {
            Vector3[] fields = new Vector3[arrows.Length];

            for (int i = 0; i < arrows.Length; i++)
            {
                fields[i] = testParticle.calculateStaticField(arrows[i].transform.position);
            }

            backgroundField = fields;
        }
        if (potentialToggle)
        {
            potential.updateBackgroundPotentials();
        }

    }


    void invokedSpawnTracers()
    {
        spawnTracers(tracer, sphereTracerSpawnPositions, lifeTime, tracerCount);
    }
    public static void spawnTracers(GameObject tracer, Vector3[] tracerSpawnPositions, float tracerLifetime, int tracerCount)
    {
        if (fieldLinesToggle)
        {
            GameObject[] charges = GameObject.FindGameObjectsWithTag("Charge");
            
            for (int i = 0; i < charges.Length; i++)
            {
                if (charges[i].GetComponent<Charges>().charge > 0 && !charges[i].Equals(leftControl.model) && !charges[i].Equals(rightControl.objectInHand) && !charges[i].transform.parent)
                {
                    int interval = (tracerSpawnPositions.Length) / tracerCount;

                    for (int z = 0; z < tracerCount; z++)
                    {
                        int j = z * interval;
                        GameObject tracerInstance = Instantiate(tracer, charges[i].transform.position + tracerSpawnPositions[j], Quaternion.identity, charges[i].transform);
                        Destroy(tracerInstance, tracerLifetime);
                    }
                }
            }
        }
    }
    public static void spawnTracersForExtendedObjects(GameObject tracer, Vector3[] tracerSpawnPositions, float tracerLifetime, int tracerCount, GameObject gameObject)
    {
        if (fieldLinesToggle)
        {
            if (gameObject.GetComponent<extendedObject>().charge > 0 && !gameObject.Equals(leftControl.model) && !gameObject.Equals(rightControl.objectInHand))
            {
                int interval = (tracerSpawnPositions.Length) / tracerCount;
                for (int z = 0; z < tracerCount; z++)
                {
                    int j = z * interval;
                    GameObject tracerInstance = Instantiate(tracer, tracerSpawnPositions[j], Quaternion.identity);
                    Destroy(tracerInstance, tracerLifetime);
                }
            }
        }
    }
    public static Vector3[] findMeshTracerSpawnPositions(GameObject gameObject)
    {
        if (!gameObject.GetComponent<MeshFilter>().sharedMesh)
        {
            return null;
        }

        Vector3[] vertices = gameObject.GetComponent<MeshFilter>().sharedMesh.vertices;
        Vector3[] normals = gameObject.GetComponent<MeshFilter>().sharedMesh.normals;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = gameObject.transform.TransformPoint(vertices[i]);
            vertices[i] += gameObject.transform.TransformDirection(normals[i]).normalized * 0.04f;
        }
        return vertices;
    }
    void Update()
    {
        potentialToggle = potentialToggle0;
        electricFieldToggle = electricFieldToggle0;
        maxField = 0;
        minField = (backgroundField[0] + testParticle.calculateNonStaticField(arrows[0].transform.position)).magnitude;
        if (electricFieldToggle)
        {

            for (int i = 0; i < arrows.Length; i++)
            {
                Vector3 field;
                if (testParticle.nonStaticCharges.Count == 0)
                {
                   field = backgroundField[i];
                }
                else
                {
                    field = backgroundField[i] + testParticle.calculateNonStaticField(arrows[i].transform.position);
                }

                if (field.magnitude > maxField)
                {
                    maxField = field.magnitude;
                }
                if (field.magnitude < minField)
                {
                    minField = field.magnitude;
                }

                arrows[i].GetComponent<arrows>().field = field;

            }

        }
    }

    public static Vector3[,,] createGrid(int resolution, float width, Vector3 position)
    {
        Vector3[,,] grid = new Vector3[resolution, resolution, resolution];

        float spacing = (float) width / resolution;

        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                for (int k = 0; k < resolution; k++)
                {
                    grid[i, j, k] = position + new Vector3(spacing * i - width/2, spacing * j - width / 2, spacing * k - width / 2);
                }
            }
        }

        return grid;
    }

    Vector3[,] createSphereicalGrid(int resolution, float radius, Vector3 position)
    {
        Vector3[,] grid = new Vector3[resolution + 1, resolution + 1];

        float spacing = 2 * Mathf.PI / resolution;
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Vector3 offset = new Vector3(radius * Mathf.Sin(spacing * j / 2.0f) * Mathf.Cos(spacing * i), radius * Mathf.Sin(spacing * j / 2.0f) * Mathf.Sin(spacing * i), radius * Mathf.Cos(spacing * j / 2.0f));
                grid[i, j] = position + offset;
            }
        }

        return grid;
    }

}
