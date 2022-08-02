using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class voltageMeter : MonoBehaviour
{
    Text text;
    void Start()
    {
        text = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            // Update voltage meter text to the voltage of the currently drawn equipotential surface. 
            text.text = potential.voltage.ToString("F2") + " V";
        }
    }
}
