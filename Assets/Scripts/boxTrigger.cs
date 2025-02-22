using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is a box
        if (other.CompareTag("Box"))
        {
            // Trigger the event when a box enters the trigger area
            Debug.Log("Box entered trigger area");
            // You can add your custom event logic here
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the box leaves the trigger area
        if (other.CompareTag("Box"))
        {
            Debug.Log("Box left trigger area");
            // You can add your custom event logic here when the box leaves
        }
    }
}
