using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tracer : MonoBehaviour
{

    Rigidbody rigidBody;
    ParticleSystem trailRenderer;
    ParticleSystem.MainModule trailRendererMain;
    // Controls the speed of the tracer particles or how fast the potential field lines are drawn.
    public float speed = 0.7f;
    
    private void Start()
    {
        // Get reference to the trail component attached to the tracer particle.
        trailRenderer = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        trailRendererMain = trailRenderer.main;
        
        rigidBody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        // Calculate force at the current position of tracer particle.
        Vector3 force = testParticle.calculateField(transform.position);
        
        // t is a linear interpolation parameter which scales down the force magnitude to a value from 0 to 1 according to maxField/minField.
        float t = force.magnitude / (electricField.maxField - electricField.minField) - (electricField.minField / (electricField.maxField - electricField.minField));
        
        // Linearly interpolate the color of the tracer's trail according to t.
        trailRendererMain.startColor = Color.Lerp(Color.blue, Color.red, t); 
        
        // Sets the velocity direction of the tracer to the field's direction with magnitude to speed. 
        rigidBody.velocity = speed * force.normalized;
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        // If tracer particle collides, 
        rigidBody.isKinematic = true;
    }
}
