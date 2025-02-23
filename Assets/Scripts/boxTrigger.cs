using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class boxTrigger : MonoBehaviour
{
    public static int boxesInTrigger = 0;
    public Color triggerColor;
    public GameObject winText;

    void Start()
    {
        triggerColor = GetComponent<Renderer>().material.color;
    }

    private bool ColorMatch(Color boxColor, Color triggerColor)
    {
        return Mathf.Abs(boxColor.r - triggerColor.r) < 0.01f &&
               Mathf.Abs(boxColor.g - triggerColor.g) < 0.01f &&
               Mathf.Abs(boxColor.b - triggerColor.b) < 0.01f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            Color boxColor = other.GetComponent<Renderer>().material.color;
            if (ColorMatch(boxColor, triggerColor))
            {
                boxesInTrigger++;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            Color boxColor = other.GetComponent<Renderer>().material.color;
            if (ColorMatch(boxColor, triggerColor))
            {
                boxesInTrigger--;
            }
        }
    }

    private void Update()
    {
        if (boxesInTrigger == 3)
        {
            winText.SetActive(true);
        }
    }
}