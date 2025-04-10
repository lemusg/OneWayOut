using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class SimonSays : MonoBehaviour
{
    public Material buttOn;
    public Material buttOff;
    private bool done = false;
    public static int flippedSwitch = -1;
    public List<int> switches;
    private bool wrong = false;

    void Awake()
    {
        Simon();
        StartCoroutine(ShowSimon());
    }
    void Simon()
    {
        if (!done) {
            switches = new List<int> {1, 2, 3, 4, 5};
            for (int i = switches.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (switches[i], switches[j]) = (switches[j], switches[i]);
            }
            done = true;
        }
    }

    private void OnEnable()
    {
        Switch.OnSwitchFlipped += HandleSwitchFlip;
    }

    private void OnDisable()
    {
        Switch.OnSwitchFlipped -= HandleSwitchFlip;
    }

    IEnumerator ShowSimon() {
        while (true) {
            yield return new WaitForSeconds(3f);
            for (int i = 1; i < 6; i++) {
                for (int j = 0; j < i; j++) {
                    int expectedSwitch = switches[j];

                    GameObject switchObj = transform.GetChild(expectedSwitch - 1).GetChild(0).gameObject;
                    Renderer switchRend = switchObj.GetComponent<Renderer>();

                    switchRend.material = buttOn;
                    yield return new WaitForSeconds(1f);
                    switchRend.material = buttOff;
                    yield return new WaitForSeconds(1f);
                }
                for (int j = 0; j < i; j++) {
                    int expectedSwitch = switches[j];
                    yield return new WaitUntil(() => flippedSwitch != -1);
                    if (flippedSwitch == expectedSwitch - 1) {
                        Switch.correctSwitches = j + 1;
                        flippedSwitch = -1;
                    } else {
                        Switch.correctSwitches = 0;
                        flippedSwitch = -1;
                        wrong = true;
                        break;
                    }
                    yield return new WaitForSeconds(1f);
                }
                if (wrong) {
                    wrong = false;
                    break;
                }
            }
        }
    }

    private void HandleSwitchFlip(int flippedIndex)
    {
        flippedSwitch = flippedIndex;
    }
}