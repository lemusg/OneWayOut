using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ladder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ExitLevel());
        }
    }

    IEnumerator ExitLevel() {
        SceneFader fader = FindObjectOfType<SceneFader>();
        yield return StartCoroutine(fader.FadeOut());
        SceneManager.LoadScene("Scenes/Floor2/Level2");
    }
}
