using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsPopup : MonoBehaviour
{
    public GameObject controls;
    public float additionalDelay = 0;
    private bool buttonPressed = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndShow());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            if (!buttonPressed)
            {
                buttonPressed = true;
                StartCoroutine(WaitAndHide());
            }
        }
    }

    IEnumerator WaitAndShow()
    {
        CanvasGroup canvasGroup = controls.GetComponent<CanvasGroup>();
        float elapsedTime = 0f;
        float duration = 2f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }
    IEnumerator WaitAndHide()
    {
        CanvasGroup canvasGroup = controls.GetComponent<CanvasGroup>();
        float elapsedTime = 0f;
        float duration = 2f;
        
        yield return new WaitForSeconds(additionalDelay);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
