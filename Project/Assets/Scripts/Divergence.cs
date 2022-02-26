using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Divergence : MonoBehaviour
{

    public float differential = 0.000001f;
    Material material;
    // Start is called before the first frame update
    void Start()
    {
        material = gameObject.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

        float incrementAlpha = calculateDivergence(gameObject.transform.position) / 1000000f;

        if (material.color.a + incrementAlpha < 0.9f && material.color.a + incrementAlpha > 0.1f)
        {
            Color color = new Color(1, 1, 1, material.color.a + incrementAlpha);
            material.color = color;
        }

    }

    float calculateDivergence(Vector3 position)
    {
        float divergence;

        Vector3 differentialVector = testParticle.calculateField(position + new Vector3(differential, differential, differential)) - testParticle.calculateField(position);

        divergence = differentialVector.x / differential + differentialVector.y / differential + differentialVector.z / differential;

        return divergence;
    }
}
