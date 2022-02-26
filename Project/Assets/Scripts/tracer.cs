using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tracer : MonoBehaviour
{

    Rigidbody rigidBody;
    ParticleSystem trailRenderer;
    ParticleSystem.MainModule trailRendererMain;
    public float speed = 0.7f;
    private void Start()
    {
        trailRenderer = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        trailRendererMain = trailRenderer.main;
        rigidBody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Vector3 force = testParticle.calculateField(transform.position);
        float t = force.magnitude / (electricField.maxField - electricField.minField) - (electricField.minField / (electricField.maxField - electricField.minField));
        trailRendererMain.startColor = Color.Lerp(Color.blue, Color.red, t); 
        rigidBody.velocity = speed * force.normalized;
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        rigidBody.isKinematic = true;
    }
}
