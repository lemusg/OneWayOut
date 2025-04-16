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
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        d = door.GetComponent<Door>();
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