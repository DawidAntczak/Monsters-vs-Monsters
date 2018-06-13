using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfig : MonoBehaviour
{
    public static LevelConfig Instance { get; private set; }
    
    [SerializeField] private int rows = 5;
    [SerializeField] private int columns = 9;
    [SerializeField] private int maxDefenders = 4;
    [SerializeField] private DefenderButton [] avaibleDefenders;
    [SerializeField] private GameObject chooseDefenderCanvas;
    [SerializeField] private GameObject levelCanvas;
    [SerializeField] private GameObject gameSpaceCanvas;
    [SerializeField] private WinLoseManager winLoseManager;
    [SerializeField] private SpawningSystem spawningSystem;
    [SerializeField] private SpawningData spawningData;
    public Transform LevelCanvasTransform { get; private set; }

    private void Awake()
    {
        Instance = this;
        winLoseManager = Instantiate(winLoseManager, transform);
    }

    private void Start()
    {
        Instantiate(chooseDefenderCanvas);
    }

    public void StartGame()
    {
        LevelCanvasTransform = Instantiate(levelCanvas).transform;
        Transform gameSpaceTransform = Instantiate(gameSpaceCanvas).transform;
        spawningSystem = Instantiate(spawningSystem, transform);
    }

    public int Rows
    {
        get { return rows; }
    }

    public int Columns
    {
        get { return columns; }
    }

    public int MaxDefenders
    {
        get { return maxDefenders; }
    }

    public DefenderButton[] AvaibleDefenders
    {
        get { return avaibleDefenders; }
    }

    public WinLoseManager WinLoseManager
    {
        get { return winLoseManager; }
    }

    public SpawningData SpawningData
    {
        get { return spawningData; }
    }
}
