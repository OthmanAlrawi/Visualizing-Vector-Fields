using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tracer : MonoBehaviour
{

    Rigidbody rigidBody;
    ParticleSystem trailRenderer;
    ParticleSystem.MainModule trailRendererMain;
    
    //Controls the speed of the tracer particles or how fast the potential field lines are drawn
    public float speed = 0.7f;
    
    private void Start()
    {
        //Get reference to the trail component attached to the tracer particle
        trailRenderer = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        trailRendererMain = trailRenderer.main;
        
        rigidBody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        //Linearly interpolate the color of the tracer's trail according to electric field magnitude
        Vector3 force = testParticle.calculateField(transform.position);
        float t = force.magnitude / (electricField.maxField - electricField.minField) - (electricField.minField / (electricField.maxField - electricField.minField));
        trailRendererMain.startColor = Color.Lerp(Color.blue, Color.red, t); 
        

        rigidBody.velocity = speed * force.normalized;
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        //If tracer particle collides, make it static 
        rigidBody.isKinematic = true;
    }
}
