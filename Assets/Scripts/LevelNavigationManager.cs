using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNavigationManager : MonoBehaviour
{
    public List<GameObject> EntryPoints; //GameObjects representing the actual spawn points in the level; the player will be placed at one of these when loaded in the game.

    // Start is called before the first frame update
    void Start()
    {
        //Don't worry too much about this, it just avoids moving the player when they are loaded to the first scene
        if (PersistantGameManager.LevelEntryPoint == -1) return;
        
        SetPlayerPositionAndRotation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetPlayerPositionAndRotation()
    {
        GameObject player = GameObject.Find("Player");
        int entryPointId = PersistantGameManager.LevelEntryPoint;
        Vector3 entryPointPosition = EntryPoints[entryPointId - 1].transform.position;
        player.transform.position = new Vector3(entryPointPosition.x, player.transform.position.y, entryPointPosition.z);
    }
}
