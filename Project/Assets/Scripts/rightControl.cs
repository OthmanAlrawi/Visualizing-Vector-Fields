using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class rightControl : MonoBehaviour
{
    public SteamVR_Input_Sources handType;

    public SteamVR_Action_Boolean touch = null;
    public SteamVR_Action_Boolean press = null;
    public SteamVR_Action_Vector2 touchPosition = null;
    public SteamVR_Action_Boolean togglePotential;
    public GameObject voltageMeter;

    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean toggle;

    private GameObject collidingObject;
    public static GameObject objectInHand;
    bool nextFrame = false;

    Vector3 placementPosition;

    void Update()
    {
        if (togglePotential.GetLastStateDown(handType))
        {
            potential.toggleMarchingCube();
        }

        if (press.GetState(handType))
        {
            if(touchPosition.GetAxis(handType).y > 0)
            {
                potential.voltage += 0.01f;
            }
            else
            {
                potential.voltage -= 0.01f;
            }
        }

        if (potential.potentialToggle)
        {
            if (touch.GetStateDown(handType))
            {
                voltageMeter.SetActive(true);
            }
            if (touch.GetStateUp(handType))
            {
                voltageMeter.SetActive(false);
            }
        }


        /*        if (nextFrame)
                {
                    electricField.updateBackgroundField();
                    nextFrame = !nextFrame;
                }*/
        if (toggle.GetLastStateDown(handType))
        {
            Application.LoadLevel(Application.loadedLevel);
/*            Reset();
            nextFrame = !nextFrame;*/
        }

        placementPosition = transform.position + 0.15f * transform.forward;
        if (grabAction.GetState(handType) && objectInHand != null)
        {
            if (objectInHand.tag == "Charge" || objectInHand.tag == "Extended Object")
            {
                objectInHand.transform.position = placementPosition;
                objectInHand.transform.rotation = gameObject.transform.rotation;

            }
        }

        if (grabAction.GetLastStateDown(handType) && collidingObject)
        {
            if(collidingObject != leftControl.model)
            {
                if (collidingObject.tag == "Test Particle")
                {
                    GrabObject();
                }
                else if (collidingObject.tag == "Charge")
                {
                    collidingObject.GetComponent<Charges>().backGround = false;
                    electricField.updateBackgroundField();
                    GrabObject();
                }
                else if (collidingObject.tag == "Extended Object")
                {
                    for (int i = 0; i < collidingObject.transform.childCount; i++)
                    {
                        collidingObject.transform.GetChild(i).gameObject.GetComponent<Charges>().backGround = false;
                    }
                    electricField.updateBackgroundField();
                    GrabObject();

                }
                testParticle.updateNonStaticCharges();
            }
            
       

        }

        if (grabAction.GetLastStateUp(handType) && objectInHand)
        {

            if (objectInHand.tag == "Charge")
            {
                objectInHand.GetComponent<Charges>().backGround = true;
                electricField.updateBackgroundField();
            } else if(objectInHand.tag == "Extended Object")
            {
                for (int i = 0; i < objectInHand.transform.childCount; i++)
                {
                    objectInHand.transform.GetChild(i).gameObject.GetComponent<Charges>().backGround = true;
                }
                electricField.updateBackgroundField();
            }
            testParticle.updateNonStaticCharges();
            ReleaseObject(); 
        }

    }


    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        collidingObject = col.gameObject;
        if(col.tag == "Test Particle" || col.tag == "Charge" || col.tag == "Extended Object"  && !objectInHand)
        {
            collidingObject.GetComponent<Outline>().enabled = true;
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Test Particle" || other.tag == "Charge" || other.tag == "Extended Object")
        {
            other.GetComponent<Outline>().enabled = false;
        }
        if (!collidingObject)
        {
            return;
        }
        collidingObject = null;
    }

    private void GrabObject()
    {
        objectInHand = collidingObject;
        collidingObject = null;
        if (objectInHand.tag == "Test Particle")
        {
            var joint = AddFixedJoint();
            joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
        }
        if(objectInHand.transform.childCount > 0 && objectInHand.tag == "Charge")
        {
            for (int z = 0; z < objectInHand.transform.childCount; z++)
            {
                GameObject child = objectInHand.transform.GetChild(z).gameObject;
                Destroy(child, 0);
            }
        }
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = Mathf.Infinity;
        fx.breakTorque = Mathf.Infinity;
        return fx;
    }

    private void Reset()
    {
        GameObject[] charge = GameObject.FindGameObjectsWithTag("Test Particle");
        GameObject[] charge1 = GameObject.FindGameObjectsWithTag("Charge");
        GameObject[] charge2 = GameObject.FindGameObjectsWithTag("Tracer");
        for (int i = 0; i < charge.Length; i++) {
            if (!charge[i].Equals(leftControl.model))
            {
                leftControl.explosionEffect(charge[i]);
                Destroy(charge[i]);
            }
        }
        for (int i = 0; i < charge1.Length; i++)
        {
            if (!charge1[i].Equals(leftControl.model))
            {
                leftControl.explosionEffect(charge1[i]);
                Destroy(charge1[i]);
            }
        }
        for (int i = 0; i < charge2.Length; i++)
        {
            if (!charge2[i].Equals(leftControl.model))
            {
                Destroy(charge2[i]);
            }
        }

    }
    private void ReleaseObject()
    {
        
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
            objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
        }
        objectInHand = null;
    }
}
