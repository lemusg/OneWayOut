using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{

    public string LevelName;
    public int LevelEntryPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO: When the player enter's the door's trigger, set the destination level/entry point ID in the manager, then load the level
        PersistantGameManager.SetTargetLevel(LevelName, LevelEntryPoint);
        SceneManager.LoadScene(LevelName);
    }
}
