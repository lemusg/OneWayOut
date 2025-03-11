using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureLight : MonoBehaviour
{
    [SerializeField] private float colorChangeSpeed = 0.5f;  // Color cycle speed
    
    private Light lightComponent;
    
    void Start()
    {
        lightComponent = GetComponent<Light>();
    }

    void Update()
    {
        // Cycle through colors using HSV
        float hue = Mathf.PingPong(Time.time * colorChangeSpeed, 1f);
        lightComponent.color = Color.HSVToRGB(hue, 1f, 1f);
    }
}