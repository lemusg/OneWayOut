using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDirt : MonoBehaviour
{
    public GameObject dirt;
    private bool done = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (!done) {
            done = true;
            int rand = Random.Range(10, 25);
            for (int i = 0; i < rand; i++) {
                //Gets "coordinates" of a random floor tile. One of the tiles in the middle is at 2.85, 2.85
                //So it just places another in reference to that
                float randomX = Random.Range(-6,5)*15.65f + 2.85f;
                float randomZ = Random.Range(-6,5)*15.65f + 2.85f;
                Vector3 randPos = new Vector3(randomX, 5.01f, randomZ);
                Instantiate(dirt, randPos, Quaternion.Euler(-90, 0, 0));
            }
        }
    }
}
