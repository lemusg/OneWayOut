using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class boxTrigger : MonoBehaviour
{
    public static int boxesInTrigger = 0;
    public Material mat;
    public GameObject door;
    private Door d;
    void Start()
    {
        d = door.GetComponent<Door>();
    }

    void Update()
    {
        if (boxesInTrigger == 3)
        {
            d.isOpen = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            BoxPopup.triggered = true;
            Material boxMat = other.GetComponent<Renderer>().sharedMaterial;
            if (boxMat == mat)
            {
                boxesInTrigger++;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            Material boxMat = other.GetComponent<Renderer>().sharedMaterial;
            if (boxMat == mat)
            {
                boxesInTrigger--;
            }
        }
    }
}