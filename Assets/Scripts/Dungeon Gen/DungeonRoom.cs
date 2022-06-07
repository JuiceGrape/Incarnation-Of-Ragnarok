using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "ScriptableObjects/Room", order = 1)]
public class DungeonRoom : ScriptableObject
{
    public int width = 1;
    public int height = 1;

    public int GetWeight()
    {
        return width * height;
    }

    public bool CanRotate()
    {
        return width != height;
    }

    public GameObject[] tiles;
}