using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorShake : MonoBehaviour
{
    private SpriteRenderer cSprite;
    private SpriteRenderer oSprite;
    private bool inTrigger = false;
    // Start is called before the first frame update
    void Start()
    {
        Transform parent = transform.parent;
        cSprite = parent.Find("ClosedSprite")?.GetComponent<SpriteRenderer>();
        oSprite = parent.Find("OpenSprite")?.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inTrigger && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Shake());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
        }
    }

    IEnumerator Shake()
    {
        Vector3 pos = cSprite.transform.position;
        Vector3 openPos = oSprite.transform.position;
        float duration = 0.3f;
        float shake = 0.5f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Generate random small offset
            float xOffset = Random.Range(-shake, shake);
            float yOffset = Random.Range(-shake, shake);

            // Apply shake effect
            cSprite.transform.position = pos + new Vector3(xOffset, yOffset, 0);
            oSprite.transform.position = openPos + new Vector3(xOffset, yOffset, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset position after shake
        cSprite.transform.position = pos;
        oSprite.transform.position = openPos;
    }
}
