using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GameObject.Find("Fade").GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(2f);
        float elapsedTime = 0f;
        
        while (elapsedTime < 3f)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / 3f);
            yield return null;
        }
        
        canvasGroup.alpha = 0f;
        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < 3f)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / 3f);
            yield return null;
        }
        
        canvasGroup.alpha = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
