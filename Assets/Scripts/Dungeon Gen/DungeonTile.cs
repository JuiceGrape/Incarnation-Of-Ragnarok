using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonTile : MonoBehaviour
{
    public Vector2Int size;

    // Start is called before the first frame update
    void Start()
    {
        if (size.x == 0 || size.y == 0)
        {
            throw new System.Exception("Tile with name " + transform.name + " has a uninitialized dungeon tile script");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
