using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public bool fadeIn;
    public float initialFadeInDelay;
    public float initialFadeOutDelay;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        
        if (fadeIn) {
            canvasGroup.alpha = 1f;
        } else {
            canvasGroup.alpha = 0f;
        }
    }

    void Start()
    {
        if (fadeIn) {
            StartCoroutine(FadeIn());
        } else {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(initialFadeInDelay);
        float elapsedTime = 0f;
        
        while (elapsedTime < 3f)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / 3f);
            yield return null;
        }
        
        canvasGroup.alpha = 0f;
    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(initialFadeOutDelay);
        float elapsedTime = 0f;
        
        while (elapsedTime < 3f)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / 3f);
            yield return null;
        }
        
        canvasGroup.alpha = 1f;
    }
}