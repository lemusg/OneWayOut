using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class boxTrigger : MonoBehaviour
{
    public static int boxesInTrigger = 0;
    public GameObject box;
    public GameObject door;
    public GameObject popup;
    private Door d;
    public AudioClip win;
    private AudioSource audioSource;
    private static List<(GameObject box, Vector3 initialPosition, Quaternion initialRotation)> allBoxes = new List<(GameObject, Vector3, Quaternion)>();
    
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        d = door.GetComponent<Door>();
        
        // Register this box's initial position if it hasn't been registered yet
        if (box != null && !allBoxes.Any(b => b.box == box))
        {
            allBoxes.Add((box, box.transform.position, box.transform.rotation));
        }
    }

    public static void ResetRoom()
    {
        // Reset all boxes to their initial positions
        foreach (var boxData in allBoxes)
        {
            if (boxData.box != null)
            {
                boxData.box.transform.position = boxData.initialPosition;
                boxData.box.transform.rotation = boxData.initialRotation;
                
                // Reset the Rigidbody velocity
                Rigidbody rb = boxData.box.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
        }
        
        // Reset the trigger count
        boxesInTrigger = 0;
        
        // Find and reset all trigger lights
        boxTrigger[] triggers = GameObject.FindObjectsOfType<boxTrigger>();
        foreach (boxTrigger trigger in triggers)
        {
            if (trigger.transform.childCount > 0)
            {
                GameObject light = trigger.transform.GetChild(0).gameObject;
                light.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (boxesInTrigger == 3)
        {
            d.isOpen = true;
            if (win != null)
                audioSource.PlayOneShot(win);
            boxesInTrigger = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            if (popup != null)
                if (!popup.GetComponent<Popup>().triggered)
                    popup.GetComponent<Popup>().triggered = true;
            if (other.gameObject == box)
            {
                GameObject light = transform.GetChild(0).gameObject;
                light.SetActive(true);
                boxesInTrigger++;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            if (other.gameObject == box)
            {
                GameObject light = transform.GetChild(0).gameObject;
                light.SetActive(false);
                boxesInTrigger--;
            }
        }
    }
}