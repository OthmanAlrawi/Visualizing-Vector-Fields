using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrows : MonoBehaviour
{
    public Vector3 field;
    public float minArrowLength = 0.8f;
    public float maxArrowLength = 0.8f;
    public Color startColor;
    public Color endColor;
    public float threshold;
    public float sharpness;
    public float scale = 4.5f;
    private float maxDistance;
    public bool toggleDistanceColorScaling = true;


    private void Start()
    {
        
    }
    void Update()
    {
        if (!float.IsNaN(field.magnitude) && !float.IsNaN(field.magnitude / electricField.maxField))
        {
            float t = field.magnitude / (electricField.maxField - electricField.minField) - (electricField.minField / (electricField.maxField - electricField.minField));
            /*            if(t < threshold)
                        {
                            GetComponent<Renderer>().enabled = false;
                        }
                        else
                        {
                            GetComponent<Renderer>().enabled = true;
                        }*/
            /*            if(t > threshold)
                        {
                            t = 1;
                        }
                        */
            if (toggleDistanceColorScaling)
            {
                List<GameObject> charges = testParticle.nonChildCharges;
                GameObject[] extendedObjects = testParticle.extendedObjects;

                float z = 1;
                for (int i = 0; i < charges.Count; i++)
                {
                    if (charges[i])
                    {
                        float u = scale * (charges[i].transform.position - transform.position).magnitude / (electricField.maxDistance);
                        if (u < z)
                        {
                            z = u;
                        }
                    }
                }
                for (int i = 0; i < extendedObjects.Length; i++)
                {
                    if (extendedObjects[i])
                    {
                        float u = scale * (extendedObjects[i].transform.position - transform.position).magnitude / (electricField.maxDistance);
                        if (u < z)
                        {
                            z = u;
                        }
                    }
                }
                if (z > threshold)
                {
                    gameObject.GetComponent<Renderer>().enabled = false;
                }
                else
                {
                    gameObject.GetComponent<Renderer>().enabled = true;
                    GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(startColor, endColor, Mathf.Pow(z, sharpness)));
                }
            }

            
            gameObject.transform.localScale = new Vector3(Mathf.Lerp(minArrowLength, maxArrowLength, t), 1, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(0, 0, 0);
        }
        gameObject.transform.rotation = Quaternion.FromToRotation(-Vector3.right, field.normalized);
        }
    }

    

