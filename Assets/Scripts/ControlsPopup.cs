using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsPopup : MonoBehaviour
{
    public GameObject controlsPopup;
    private CanvasGroup canvasGroup;
    private bool keyPressed = false;
    // Start is called before the first frame update
    void Start()
    {
        controlsPopup.SetActive(true);
        canvasGroup = controlsPopup.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            if (!keyPressed)
            {
                StartCoroutine(hideControls());
                keyPressed = true;
            }
        }
    }

    IEnumerator hideControls()
    {
        yield return new WaitForSeconds(3f);
        float elapsedTime = 0f;
        
        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / 2f);
            yield return null;
        }
        
        canvasGroup.alpha = 0f;
        controlsPopup.SetActive(false);
    }
}
