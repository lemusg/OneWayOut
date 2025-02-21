using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    public GameObject fadeIn;
    public GameObject fadeOut;
    public bool fadeDone;
    // This delay should equal the value of the fadeInDelay from the FadeIn object + 3f
    public float delay;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeController());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeDone) {
            SceneManager.LoadScene("Main Menu");
        }
    }

    private IEnumerator FadeController() {
        fadeIn.SetActive(true);
        fadeOut.SetActive(false);
        yield return new WaitForSeconds(delay);
        fadeIn.SetActive(false);
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(delay);
        fadeDone = true;
    }
}
