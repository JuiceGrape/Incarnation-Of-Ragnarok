//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//[System.Serializable]
//public struct RoomWithWeight
//{
//    public DungeonRoom room;
//    public float weight; 
//}

//public class InstantiableRoom
//{
//    public InstantiableRoom()
//    {
//        attachedRooms = new List<InstantiableRoom>();
//    }
//    public DungeonRoom room;
//    public int weight;
//    public Vector2Int pos;
//    public bool isRotated = false;

//    public override string ToString()
//    {
//        return room.GetWeight().ToString();
//    }

//    public GameObject tile;

//    //Dijkstra stuff
//    public List<InstantiableRoom> attachedRooms;
//    public InstantiableRoom parent;
//    public bool walkable = true;
//    public int weightFromStart = 0;
//}

//public class Dungeon : MonoBehaviour
//{
//    private static Dungeon instance;

//    #region dungeon
//    [Header("Dungeon Stuff")]
//    [SerializeField]  private int m_seed = 133742069;
//    [SerializeField]  private int width = 10;
//    [SerializeField]  private int height = 10;
//    [SerializeField]  private int tilewidth = 10;
//    [SerializeField]  private int tileHeight = 10;
//    #endregion

//    [Header("Dungeon Stuff 2")]
//    [SerializeField] private int minWeight = 0;
//    [SerializeField] private int maxWeight = 10;

//    [Header("More Dungeon Stuff")]
//    [SerializeField] private DungeonRoom filler;
//    [SerializeField] private DungeonRoom start;
//    [SerializeField] private Vector2Int startPos;
//    [SerializeField] private DungeonRoom end;
//    [SerializeField] private Vector2Int endPos;

//    private InstantiableRoom spawnedStart;
//    private InstantiableRoom spawnedEnd;

//    [SerializeField]  private RoomWithWeight[] layout;

//    private InstantiableRoom[,] rooms;

//    private List<InstantiableRoom> placedRooms;

//    void Start()
//    {
//        if (instance == null)
//        {
//            instance = this;
//        }
//        else
//        {
//            throw new System.Exception("Multiple dungeon instances in one scene");
//        }

//        availableTiles = new List<Vector2Int>();
//        legalTiles = new List<Vector2Int>();
//        placedRooms = new List<InstantiableRoom>();

//        UnityEngine.Random.InitState(m_seed);
//        Generate();
//    }

//    public static Dungeon GetInstance()
//    {
//        return instance;
//    }


//    void Update()
//    {
        
//    }

//    List<Vector2Int> availableTiles;
//    List<Vector2Int> legalTiles; //All legal tiles for this set

//    void Generate()
//    {
//        double startTime = Time.realtimeSinceStartup;

//        rooms = new InstantiableRoom[width, height];

//        int amountOfTiles = width * height;

//        SortRooms(); //Maybe make devs pre sort this? //Maybe make spawnableRooms another scriptable object?

//        for(int x = 0; x < width; x++)
//        {
//            for(int y = 0; y < height; y++)
//            {
//                availableTiles.Add(new Vector2Int(x,y));
//            }
//        }

//        if (CanPlaceTile(startPos, start.width, start.height, false))
//        {
//            spawnedStart = placeRoom(start, startPos, false);
//            spawnedStart.weight = 0;
//        }

//        if (CanPlaceTile(endPos, end.width, end.height, false))
//        {

//            spawnedEnd = placeRoom(end, endPos, false);
//            spawnedEnd.weight = 0;
//        }

//        foreach (RoomWithWeight target in layout) //Spawn all rooms according to ratios
//        {
//            Debug.Log("Placing: " + target.room.GetWeight());
//            legalTiles.Clear();

//            foreach(Vector2Int tile in availableTiles) //Fill legal tiles with all available ones
//            {
//                legalTiles.Add(tile); 
//            }

//            int targetTileAmount = (int)(((target.weight / 100.0f) * amountOfTiles) / (float)target.room.GetWeight()); //Set the amount of tiles we expect to generate of this type
//            int actualTiles = 0; //Amount of tiles we have generated of this type so far
//            int amount = 0; 
//            while(legalTiles.Count > 0 && actualTiles < targetTileAmount) //Keep looping until we have created the amount of tiles we expect to create
//            {
//                amount++;
//                Vector2Int targetTile = legalTiles[UnityEngine.Random.Range(0, legalTiles.Count)]; //Grab a random legal tile.
//                //int targetTile = legalTiles[legalTiles.Count - 1];

//                bool rotated = target.room.CanRotate() && UnityEngine.Random.value > 0.5f;
//                bool canPlace = CanPlaceTile(targetTile, target.room.width, target.room.height, rotated); //Can we place this tile here?
                
//                if (!canPlace && target.room.CanRotate()) //If we can't, can we rotate it?
//                {
//                    rotated = !rotated;
//                    canPlace = CanPlaceTile(targetTile, target.room.height, target.room.width, rotated); //Try it, but rotated this time
//                }
                
//                if (canPlace) //Can place the tile here. Proceed to placing it.
//                {
//                    //Debug.Log("placing tile");
//                    placeRoom(target.room, targetTile, rotated);
//                    actualTiles++;
//                }
//                else
//                {
//                    legalTiles.Remove(targetTile); //Not a legal position. Remove it.
//                }
//            }
//            Debug.Log("Looped " + amount + " times");
//            Debug.Log("Expected: " + targetTileAmount + ", Actual:" + actualTiles);
//        }
        

//        //Fill in the rest
//        for(int x = 0; x < width; x++)
//        {
//            for(int y = 0; y < height; y++)
//            {
//                if (rooms[x,y] == null)
//                {
//                    placeRoom(filler, new Vector2Int(x,y), false);
//                }
//            }
//        }

//        //Connect room
//        for (int x = 0; x < width; x++)
//        {
//            for (int y = 0; y < height; y++)
//            {
//                InstantiableRoom current = rooms[x, y];
//                Vector2Int pos = new Vector2Int(x, y);
//                //Upper check
//                if (y + 1 < height)
//                {
//                    InstantiableRoom target = rooms[x, y + 1];
//                    if (target != current && !current.attachedRooms.Contains(target))
//                    {
//                        current.attachedRooms.Add(target);
//                    }
//                }

//                //Lower check
//                if (y - 1 >= 0)
//                {
//                    InstantiableRoom target = rooms[x, y - 1];
//                    if (target != current && !current.attachedRooms.Contains(target))
//                    {
//                        current.attachedRooms.Add(target);
//                    }
//                }

//                //Left check
//                if (x - 1 >= 0)
//                {
//                    InstantiableRoom target = rooms[x - 1, y];
//                    if (target != current && !current.attachedRooms.Contains(target))
//                    {
//                        current.attachedRooms.Add(target);
//                    }
//                }

//                //Right check
//                if (x + 1 < width)
//                {
//                    InstantiableRoom target = rooms[x + 1, y];
//                    if (target != current && !current.attachedRooms.Contains(target))
//                    {
//                        current.attachedRooms.Add(target);
//                    }
//                }

//            }
//        }

//        foreach (InstantiableRoom room in placedRooms)
//        {
//            room.weight = UnityEngine.Random.Range(minWeight, maxWeight + 1);
//        }

//        List<InstantiableRoom> dungeon = findShortestPath(spawnedStart, spawnedEnd);

//        //Spawn rooms
//        //foreach(InstantiableRoom room in placedRooms)   //To make it place all rooms
//        foreach (InstantiableRoom room in dungeon)        //To make it place the generated dungeon
//        {  
//            int adjustedW = (room.isRotated ? room.room.height : room.room.width) - 1;
//            int adjustedH = (room.isRotated ? room.room.width : room.room.height) - 1;

//            Vector3 pos;
//            pos.y = 1; //TODO: Height???
//            pos.x = (room.pos.x * tilewidth) + ((tilewidth / 2) * adjustedW); //Offsets the tile so it spawns in the correct position
//            pos.z = (room.pos.y * tileHeight) + ((tileHeight / 2) * adjustedH);
//            GameObject tile = Instantiate<GameObject>(room.room.tiles[0].gameObject);
//            room.tile = tile;
//            tile.transform.position = pos;

//            if (room.isRotated)
//            {
//                tile.transform.Rotate(0.0f, 90.0f, 0.0f);
//            }

//            if (room == spawnedStart)
//            {
//                tile.name = "start";
//            }
//            if (room == spawnedEnd)
//            {
//                tile.name = "end";
//            }
//        }

//        double endTime = (Time.realtimeSinceStartup - startTime);
//        print("Generation time: " + endTime);
//    }

//    bool CanPlaceTile(Vector2Int tilePos, int width, int height, bool rotated)
//    {
//        int adjustedW = rotated ? height : width;
//        int adjustedH = rotated ? width : height;

//        if(CrossesEdge(tilePos, adjustedW, adjustedH))
//        {
//            return false;
//        }
        
//        bool canPlace = true;

//        for(int x = tilePos.x; x < tilePos.x + adjustedW; x++)
//        {
//            if (!canPlace) break; //Stop looping if we can't place a tile here
//            for(int y = tilePos.y; y < tilePos.y + adjustedH; y++)
//            {
//                if (!canPlace) break; //Stop looping if we can't place a tile here
//                if (rooms[x,y] != null)
//                { 
//                    canPlace = false;
//                }
//            }
//        }

//        return canPlace;
//    }

//    InstantiableRoom placeRoom(DungeonRoom target, Vector2Int targetTile, bool rotated)
//    {
//        InstantiableRoom room = new InstantiableRoom();

//        room.room = target;
//        room.pos = targetTile;
//        room.isRotated = rotated;

//        placedRooms.Add(room); 

//        int adjustedW = rotated ?  target.height : target.width;
//        int adjustedH = rotated ?  target.width : target.height;

//        for(int x = targetTile.x; x < targetTile.x + adjustedW; x++)
//        {
//            for(int y = targetTile.y; y < targetTile.y + adjustedH; y++)
//            {
//                Vector2Int tileToRemove = new Vector2Int(x,y);

//                legalTiles.Remove(tileToRemove);
//                availableTiles.Remove(tileToRemove);

//                rooms[x,y] = room;
//            }
//        }
//        return room;
//    }

//    void SortRooms()
//    {
//        Array.Sort<RoomWithWeight>(layout,
//            delegate (RoomWithWeight a, RoomWithWeight b)
//            {
//                int weightA = a.room.GetWeight();
//                int weightB = b.room.GetWeight();

//                if (weightA == weightB)
//                {
//                    return b.weight.CompareTo(a.weight);
//                }
//                return weightB.CompareTo(weightA);
//            });
//    }

//    bool CrossesEdge(Vector2Int pos, int tileWidth, int tileHeight)
//    {
//        int W = tileWidth - 1;
//        int H = tileHeight - 1;

//        if (W > 0)
//        {
//            if (pos.x + W >= width)
//            {
//                return true; //Crosses on left side
//            }
//        }

//        if (H > 0)
//        {
//            if (pos.y + H >= height)
//            {
//                return true; //Crosses on top
//            }
//        }

//        return false;
//    }

//    public List<InstantiableRoom> findShortestPath(InstantiableRoom start, InstantiableRoom end)
//    {
//        List<InstantiableRoom> result = new List<InstantiableRoom>();

//        InstantiableRoom node = DijkstrasAlgo(start, end);

//        // While there's still previous node, we will continue.
//        while (node != null)
//        {
//            result.Add(node);
//            node = node.parent;
//        }

//        // Reverse the list so that it will be from start to end.
//        result.Reverse();
//        return result;
//    }

//    private InstantiableRoom DijkstrasAlgo(InstantiableRoom start, InstantiableRoom end)
//    {
//        // Nodes that are unexplored
//        List<InstantiableRoom> unexplored = new List<InstantiableRoom>();

//        // We add all the nodes we found into unexplored.
//        foreach (InstantiableRoom room in placedRooms)
//        {
//            if (room.walkable)
//            {
//                room.parent = null;
//                room.weightFromStart = int.MaxValue;
//                unexplored.Add(room);
//            }
//        }

//        // Set the starting node weight to 0;
//        start.weightFromStart = 0;

//        while (unexplored.Count > 0)
//        {
//            // Sort the unexplored by their weight in ascending order.
//            unexplored.Sort((x, y) => x.weightFromStart.CompareTo(y.weightFromStart));

//            // Get the lowest weight in unexplored.
//            InstantiableRoom current = unexplored[0];

//            //Remove the node, since we are exploring it now.
//            unexplored.Remove(current);

//            foreach (InstantiableRoom neighbour in current.attachedRooms)
//            {
//                // We want to avoid those that had been explored and is not walkable.
//                if (neighbour.walkable && unexplored.Contains(neighbour))
//                {
//                    // Get the weight of the object.
//                    int weight = current.weightFromStart + neighbour.weight;

//                    // If the added distance is less than the weight from the start.
//                    if (weight < neighbour.weightFromStart)
//                    {
//                        // We update the new distance as weight and update the new path now.
//                        neighbour.weightFromStart = weight;
//                        neighbour.parent = current;
//                    }
//                }
//            }

//        }

//        print("Path completed!");

//        return end;
//    }
//}
