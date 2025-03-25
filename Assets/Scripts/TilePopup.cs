using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TilePopup : MonoBehaviour
{
    public GameObject controlsPopup;
    private CanvasGroup canvasGroup;
    public static bool triggered = false;
    private bool isTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        controlsPopup.SetActive(true);
        canvasGroup = controlsPopup.GetComponent<CanvasGroup>();
        // Set initial large scale and center position
        RectTransform rectTransform = controlsPopup.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one * 20f; // Start 2x normal size
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;

        // Start animation to bottom right
        // Start with fully transparent
        canvasGroup.alpha = 0f;

        // Fade in first, then animate to corner
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        float fadeDuration = 2f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            
            // Smooth fade interpolation
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            
            yield return null;
        }

        // Ensure we end at full opacity
        canvasGroup.alpha = 1f;
        StartCoroutine(AnimateToCorner());
    }

    private IEnumerator AnimateToCorner()
    {
        yield return new WaitForSeconds(1f);
        RectTransform rectTransform = controlsPopup.GetComponent<RectTransform>();
        Vector2 startPos = rectTransform.anchoredPosition;
        Vector3 startScale = rectTransform.localScale;
        
        // Target position in bottom right (adjust these values as needed)
        Vector2 targetPos = new Vector2(800f, -400f);
        Vector3 targetScale = Vector3.one * 6f; // End at 75% of original size
        
        float elapsedTime = 0f;
        float duration = 1.5f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            // Smooth interpolation
            t = Mathf.SmoothStep(0, 1, t);
            
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
            
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered && !isTriggered)
        {
            StartCoroutine(hideControls());
            isTriggered = true;
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
