using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Actions : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean toggle;

    private GameObject collidingObject; 
    private GameObject objectInHand; 

    public GameObject gameObject0;
    GameObject model;

    bool editMode = false;

    Vector3 placementPosition;

    private void Start()
    {
        model = Instantiate(gameObject0);
        model.GetComponent<MeshRenderer>().material.color = Color.grey; 
        model.GetComponent<Collider>().enabled = false;
    }
    void Update()
    {
        placementPosition = transform.position + 0.15f * transform.forward;
        model.transform.position = placementPosition;
        model.SetActive(!editMode);

        if (grabAction.GetState(handType))
        {
            if (collidingObject)
            {
                collidingObject.transform.position = transform.position + 0.15f * transform.forward;
            }
        }

        if (toggle.GetLastStateDown(handType))
        {
            editMode = !editMode;
        }

        if (grabAction.GetLastStateUp(handType))
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
        }

        if (grabAction.GetLastStateDown(handType) && !editMode)
        {
            Instantiate(gameObject0, placementPosition, Quaternion.identity);
        }
    }
    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        collidingObject = col.gameObject;
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
        //var joint = AddFixedJoint();
        //joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {
        if (GetComponent<FixedJoint>())
        {
            //GetComponent<FixedJoint>().connectedBody = null;
            //Destroy(GetComponent<FixedJoint>());
            //objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
            //objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
        }
        objectInHand = null;
    }
}
