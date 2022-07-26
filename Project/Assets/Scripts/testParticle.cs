using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testParticle : MonoBehaviour
{
    Rigidbody rigidBody;
    public static GameObject[] charges;
    public static List<GameObject> nonStaticCharges;
    public static GameObject[] extendedObjects;
    public static List<GameObject> nonChildCharges;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        // Exert force onto the test particle according to the field at its position. 
        rigidBody.AddForce(calculateField(transform.position));
    }

    // Updates the arrays that holds references to the different charge types. 
    public static void updateCharges()
    {
        charges = GameObject.FindGameObjectsWithTag("Charge");
        extendedObjects = GameObject.FindGameObjectsWithTag("Extended Object");
        
        // These are the charges which are not attached to an extended object. 
        nonChildCharges = new List<GameObject>();
        for (int i = 0; i < charges.Length; i++)
        {
            if (charges[i])
            {
                if(!charges[i].transform.parent)
                {
                    nonChildCharges.Add(charges[i]);
                }
                else if(charges[i].transform.parent.tag != ("Extended Object"))
                {
                    nonChildCharges.Add(charges[i]);
                }
                
            }
        }


    }
    
    // Updates the array that holds references all the non static charges present in the scene, those are charges that are attached to the controllers. 
    public static void updateNonStaticCharges()
    {
        GameObject[] charges0 = GameObject.FindGameObjectsWithTag("Charge");
        nonStaticCharges = new List<GameObject>();

        for (int i = 0; i < charges0.Length; i++)
        {
            // Checks if the charge is part of the background charge distribution.
            if (!charges0[i].GetComponent<Charges>().backGround)
            {
                nonStaticCharges.Add(charges0[i]);
            }
        }
    }
    
    // Calculates the field produces by all the charges in the scene.
    public static Vector3 calculateField(Vector3 position)
    {
        Vector3 field = Vector3.zero;

        for (int i = 0; i < charges.Length; i++)
        {
            if (charges[i])
            {
                field += charges[i].GetComponent<Charges>().charge * ((0.2f / Mathf.Pow(((position - charges[i].transform.position).magnitude), 2))) * (position - charges[i].transform.position).normalized;
            }
           
        }

        return field;

    }

    // Calculates the field produces by the charges that are part of the background charge distrubution i.e. static. 
    public static Vector3 calculateStaticField(Vector3 position)
    {
        Vector3 field = Vector3.zero;

        GameObject[] charges = GameObject.FindGameObjectsWithTag("Charge");

        for (int i = 0; i < charges.Length; i++)
        {
           
            if (charges[i].GetComponent<Charges>().backGround)
            {
                field += charges[i].GetComponent<Charges>().charge * ((0.2f / Mathf.Pow(((position - charges[i].transform.position).magnitude), 2))) * (position - charges[i].transform.position).normalized;
            }
        }

        return field;

    }
    
    // Calculates the field produces by the non static charges i.e. those that are attached to the controllers. 
    public static Vector3 calculateNonStaticField(Vector3 position)
    {
        Vector3 field = Vector3.zero;

        /*        if(leftControl.model)
                {
                    if(leftControl.model.tag == "Charge")
                    {
                        field += leftControl.model.GetComponent<Charges>().charge * ((0.2f / Mathf.Pow(((position - leftControl.model.transform.position).magnitude), 2))) * (position - leftControl.model.transform.position).normalized;
                    } else if (leftControl.model.tag == "Extended Object")
                    {
                        for (int i = 0; i < leftControl.model.transform.childCount; i++)
                        {
                            field += leftControl.model.transform.GetChild(i).gameObject.GetComponent<Charges>().charge * ((0.2f / Mathf.Pow(((position - leftControl.model.transform.GetChild(i).position).magnitude), 2))) * (position - leftControl.model.transform.GetChild(i).position).normalized;

                        }
                    }
                }
                if (rightControl.objectInHand)
                {
                    if (rightControl.objectInHand.tag == "Charge")
                    {
                        field += rightControl.objectInHand.GetComponent<Charges>().charge * ((0.2f / Mathf.Pow(((position - rightControl.objectInHand.transform.position).magnitude), 2))) * (position - rightControl.objectInHand.transform.position).normalized;
                    } else if (rightControl.objectInHand.tag == "Extended Object")
                    {
                        for (int i = 0; i < rightControl.objectInHand.transform.childCount; i++)
                        {
                            field += rightControl.objectInHand.transform.GetChild(i).gameObject.GetComponent<Charges>().charge * ((0.2f / Mathf.Pow(((position - rightControl.objectInHand.transform.GetChild(i).position).magnitude), 2))) * (position - rightControl.objectInHand.transform.GetChild(i).position).normalized;

                        }
                    }
                }*/
        for(int i = 0; i < nonStaticCharges.Count; i++)
        {
            if (nonStaticCharges[i])
            {
                Vector3 chargePosition = nonStaticCharges[i].transform.position;
                field += nonStaticCharges[i].GetComponent<Charges>().charge * ((0.2f / Mathf.Pow(((position - chargePosition).magnitude), 2))) * (position - chargePosition).normalized;
            }
            else
            {
                nonStaticCharges.RemoveAt(i);
            }
        }
        return field;

    }
}
