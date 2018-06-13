using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpawningSystem : MonoBehaviour
{
    public static SpawningSystem Instance { get; private set; }
    public Transform[] SpawnersTransforms { get; private set; }
    [SerializeField] private GameObject spawnerPrefab;
    private SpawningData spawningData;
    private int rows;

    private void Awake()
    {
        Instance = this;
        spawningData = LevelConfig.Instance.SpawningData;
        GetComponent<Animator>().runtimeAnimatorController = spawningData.SpawningAC;
    }

    void Start ()
    {
        rows = LevelConfig.Instance.Rows;
        float oneFieldHeight = SpawningCadre.Instance.BattleArenaRectTran.rect.height / rows;

        Vector3 startPos = GetBottomSpawnCorner() + new Vector2(0f, oneFieldHeight / 2f);
        SpawnSpawners(startPos, oneFieldHeight);
    }

    private void SpawnSpawners(Vector2 startPosition, float oneFieldHeight)
    {
        SpawnersTransforms = new Transform[rows];
        for (int i = 0; i < rows; i++)
        {
            Vector3 position = new Vector3(startPosition.x, startPosition.y + i * oneFieldHeight, -1f);
            SpawnersTransforms[i] = Instantiate(spawnerPrefab, position, Quaternion.identity, transform).transform;
        }
    }

    private Vector2 GetBottomSpawnCorner()
    {
        Vector3[] corners = new Vector3[4];
        SpawningCadre.Instance.GrassAreaRectTran.GetWorldCorners(corners);
        float x = corners[3].x;
        SpawningCadre.Instance.BattleArenaRectTran.GetWorldCorners(corners);
        float y = corners[3].y;
        return new Vector2(x, y);
    }

    private void AnimSpawn(int code)
    {
        int spawnerIndex = code / 100;
        int enemyIndex = code % 100;
        GameObject attackerObj;
        Transform spawnerTran;
        try
        {
            attackerObj = spawningData.Attackers[enemyIndex];
            spawnerTran = SpawnersTransforms[spawnerIndex];
        }
        catch
        {
            Debug.LogError("Invalid argument in animator function call!");
            return;
        }
        Movable attacker = Instantiate(attackerObj, spawnerTran.position + new Vector3(0f, 0f, Random.Range(0, 0.1f)),
            Quaternion.identity, ObjectsOrganizer.Instance.AttackerParent).GetComponent<Movable>();
        attacker.LaneIndex = spawnerIndex;
    }

    private void NotifyEndOfSpawning()
    {
        StartCoroutine(CheckForWin());
    }

    private IEnumerator CheckForWin()
    {
        Transform attackerParent = ObjectsOrganizer.Instance.AttackerParent;
        bool areEnemies = true;
        while (areEnemies)
        {
            if (attackerParent.childCount > 0)
            {
                areEnemies = true;
                yield return null;
            }
            else
            {
                areEnemies = false;
            }
        }
        LevelConfig.Instance.WinLoseManager.Win();
    }
}
