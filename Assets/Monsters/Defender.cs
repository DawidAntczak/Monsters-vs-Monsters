using UnityEngine;

public class Defender : MonoBehaviour
{
    [SerializeField] private float spawnTime = 5f;
    [SerializeField] private int starCost;
    private Vector2Int gridPosition;

    public int StarCost
    {
        get { return starCost; }
    }

    public float SpawnTime
    {
        get { return spawnTime; }
    }

    public Vector2Int GridPosition
    {
        get { return gridPosition; }
        set { gridPosition = value; }
    }
}
