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
    public GameObject doorSprite;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
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

    private void Update()
    {
        if (boxesInTrigger == 3)
        {
            door.SetActive(true);
            doorSprite.SetActive(true);
        }
    }
}