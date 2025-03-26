using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Popup : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public bool triggered = false;
    private bool isTriggered = false;
    void Start()
    {
        gameObject.SetActive(true);
        canvasGroup = GetComponent<CanvasGroup>();
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one * 20f;
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;

        canvasGroup.alpha = 0f;

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
            
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            
            yield return null;
        }

        canvasGroup.alpha = 1f;
        StartCoroutine(AnimateToCorner());
    }

    private IEnumerator AnimateToCorner()
    {
        yield return new WaitForSeconds(1f);
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 startPos = rectTransform.anchoredPosition;
        Vector3 startScale = rectTransform.localScale;
        
        Vector2 targetPos = new Vector2(805f, -420f);
        Vector3 targetScale = Vector3.one * 6f;
        
        float elapsedTime = 0f;
        float duration = 1.5f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            t = Mathf.SmoothStep(0, 1, t);
            
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
            
            yield return null;
        }
    }

    void Update()
    {
        if (triggered && !isTriggered)
        {
            StartCoroutine(hideControls());
            isTriggered = true;
        }

        if (SceneManager.GetActiveScene().name == "RoomA" && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)))
        {
            GetComponent<Popup>().triggered = true;
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
        gameObject.SetActive(false);
    }
}
