using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class leftControl : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean toggle;

    private GameObject objectInHand;

    public GameObject negativeCharge;
    public GameObject positiveCharge;
    public GameObject testCharge;

    public GameObject top;
    public GameObject right;
    public GameObject bottom;
    public GameObject left;

    int currentIndex = 0;
    int currentIndex1 = 0;
    int currentColor = 0;

    public Color[] chargeColors = new Color[2];



    public GameObject effect;
    public static GameObject explosion;

    int index = 0;

    public static GameObject model;

    GameObject[] placables;
    GameObject[] placables1;

    Vector3 placementPosition;


    private void Start()
    {
        explosion = effect;   
        placables = new GameObject[] { top, right, bottom, left };
        placables1 = new GameObject[] { negativeCharge, positiveCharge, testCharge };

        objectInHand = placables[index];
        model = Instantiate(objectInHand, transform.position + 0.2f * transform.forward, Quaternion.identity, gameObject.transform);
        testParticle.updateNonStaticCharges();
        testParticle.updateCharges();
        if (model.tag == "Test Particle")
        {
            model.GetComponent<Collider>().enabled = false;
        }
    }
    void Update()
    {
        placementPosition = transform.position + 0.2f * transform.forward;

        model.transform.position = placementPosition;

        if (grabAction.GetLastStateDown(handType))
        {
            spawnModel();
        }

        if (toggle.GetLastStateDown(handType))
        {
            currentColor++;
            if(currentColor == chargeColors.Length)
            {
                currentColor = 0;
            }

            if(currentIndex == 0)
            {
                currentIndex1++;
                if (currentIndex1 == placables1.Length)
                {
                    currentIndex1 = 0;
                }
                setModel(currentIndex1, placables1);
            }
            
            if (model.tag == "Extended Object")
            {
                model.GetComponent<Renderer>().material.color = chargeColors[currentColor];
                model.GetComponent<extendedObject>().charge *= -1;
                for (int i = 0; i < model.transform.childCount; i++)
                {
                    model.transform.GetChild(i).gameObject.GetComponent<Charges>().charge *= -1;
                }
            }

        }
       
    }

    void spawnModel()
    {
        GameObject temp = Instantiate(model, placementPosition, model.transform.rotation);


        if (model.tag == "Extended Object")
        {
            temp.GetComponent<Renderer>().material.color = model.GetComponent<Renderer>().material.color;
            temp.GetComponent<extendedObject>().charge = model.GetComponent<extendedObject>().charge;
            for (int i = 0; i < temp.transform.childCount; i++)
            {
                temp.transform.GetChild(i).gameObject.GetComponent<Charges>().charge = model.transform.GetChild(0).gameObject.GetComponent<Charges>().charge;
            }


        }
        /*            explosionEffect(temp);*/
        if (temp.tag == "Charge")
        {
            temp.GetComponent<Charges>().backGround = true;
            electricField.updateBackgroundField();
        }
        else if (temp.tag == "Extended Object")
        {
            for (int i = 0; i < temp.transform.childCount; i++)
            {
                temp.transform.GetChild(i).gameObject.GetComponent<Charges>().backGround = true;
            }

            electricField.updateBackgroundField();
        } else if (model.tag == "Test Particle")
        {
            temp.GetComponent<Collider>().enabled = true;
        }

        testParticle.updateCharges();
        testParticle.updateNonStaticCharges();

    }
    void setModel(int index_, GameObject[] placables_)
    {

        objectInHand = placables_[index_];
        Destroy(model);
        model = Instantiate(objectInHand, placementPosition, gameObject.transform.rotation, gameObject.transform);
        model.transform.localRotation = objectInHand.transform.rotation;
        if (model.tag == "Test Particle")
        {
            model.GetComponent<Collider>().enabled = false;
        }

        testParticle.updateCharges();
        testParticle.updateNonStaticCharges();
    }

    public void topEvent()
    {
        currentColor = 0;
        setModel(0, placables);
        currentIndex = 0;
    }
    public void rightEvent()
    {
        currentColor = 0;
        setModel(1, placables);
        currentIndex = 1;
    }
    public void bottomEvent()
    {
        currentColor = 0;
        setModel(2, placables);
        currentIndex = 2;
    }
    public void leftEvent()
    {
        currentColor = 0;
        setModel(3, placables);
        currentIndex = 3;
    }

    public static void explosionEffect(GameObject spawnedObject)
    {
        ParticleSystem.MainModule ps = explosion.GetComponent<ParticleSystem>().main;

        ps.startColor = spawnedObject.GetComponent<MeshRenderer>().sharedMaterial.color;

        GameObject instance = Instantiate(explosion, spawnedObject.transform.position, Quaternion.identity);

        Destroy(instance, ps.startLifetimeMultiplier);

    }
}
