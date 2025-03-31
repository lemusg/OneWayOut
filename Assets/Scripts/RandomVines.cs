using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomVines : MonoBehaviour
{
    public GameObject vine;
    private bool done = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (!done) {
            done = true;
            int rand = Random.Range(1, 3);
            for (int i = 0; i < rand; i++) {
                //Picks whether to put on north wall or west wall
                float wall = Random.Range(0, 2);
                float randomScale = (float)Random.Range(25, 40);
                float posY = 105f-randomScale/2f;
                //Gets "coordinates" of a random vine
                if (wall == 0) {
                    float randomX = (float)Random.Range(-80, 70);
                    Vector3 randPos = new Vector3(randomX, posY, 89.9f);
                    GameObject vineCopy = Instantiate(vine, randPos, Quaternion.identity);
                    vineCopy.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                } else {
                    float randomZ = (float)Random.Range(-80, 70);
                    Vector3 randPos = new Vector3(89.9f, posY, randomZ);
                    GameObject vineCopy = Instantiate(vine, randPos, Quaternion.Euler(0, 90, 0));
                    vineCopy.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                }
            }
        }
    }
}
