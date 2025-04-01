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
            popup.GetComponent<Popup>().triggered = true;
            if (other.gameObject == box)
            {
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
                boxesInTrigger--;
            }
        }
    }
}