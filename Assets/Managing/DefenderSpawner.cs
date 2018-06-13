using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefenderSpawner : MonoBehaviour
{
    public static DefenderSpawner Instance { get; private set; }
    [SerializeField] private float raycastDistance = 200f;
    private Camera myCamera;
    private int columns;
    private int rows;
    private Vector2 oneFieldNormalizedSize;
    private Vector2 oneFieldSize;
    private Vector2 battleAreaCorner;
    private RectTransform battleAreaRectTran;
    private Rect battleAreaRect;
    private Canvas canvas;
    private GameObject [ , ] grid;
    private int collectableLayerMask;

    private void Awake()
    {
        Instance = this;
        gameObject.AddComponent<Button>().onClick.AddListener(() => HandleClick());
    }

    private void Start()
    {
        myCamera = Camera.main;
        canvas = GetComponentInParent<Canvas>();
        battleAreaRectTran = GetComponent<RectTransform>();
        battleAreaRect = battleAreaRectTran.rect;

        columns = LevelConfig.Instance.Columns;
        rows = LevelConfig.Instance.Rows;
        oneFieldNormalizedSize = new Vector2(1f / columns, 1f / rows);
        oneFieldSize = new Vector2(battleAreaRect.width * canvas.scaleFactor / columns, battleAreaRect.height * canvas.scaleFactor / rows);
        battleAreaCorner = GetBattleAreaLeftBottomCorner();

        grid = new GameObject[columns, rows];
        collectableLayerMask = 1 << Layers.collectableLayer;
    }

    private void HandleClick()
    {
        RaycastHit2D hit = Physics2D.Raycast(myCamera.ScreenToWorldPoint(Input.mousePosition), Vector3.zero, raycastDistance, collectableLayerMask);
        if(hit)
        {
            CollectableSun sun = hit.collider.GetComponent<CollectableSun>();
            if (sun)
            {
                sun.Collect();
            }
            else
            {
                Debug.LogError("I don't know about this collectable element.");
            }
        }
        else
        {
            Defender currentDefender = DefenderSelectionManager.Instance.CurrentDefender();
            bool isReady = DefenderSelectionManager.Instance.IsReadyToSpawn();

            if (currentDefender && isReady)
            {
                StartSpawnProcedure(currentDefender);
            }
        }
    }

    private void StartSpawnProcedure(Defender currentDefender)
    {
        Vector2 mousePosition = Input.mousePosition;
        if (currentDefender == null)
            { return; }

        Vector2Int positionInGrid = CalculatePositionOnGrid(mousePosition);
        bool isSomethingPlanted = (grid[positionInGrid.x, positionInGrid.y] != null
            && grid[positionInGrid.x, positionInGrid.y].GetComponent<Defender>());
        if (currentDefender.GetComponent<Shovel>() && isSomethingPlanted)
        {
            Destroy(grid[positionInGrid.x, positionInGrid.y]);
            return;
        }

        if (isSomethingPlanted)
            { return; }

        int defenderCost = currentDefender.StarCost;
        if (SunCountDisplay.Instance.UseStars(defenderCost) != SunCountDisplay.Status.SUCCESS)
            {  return; }


        float addX = oneFieldSize.x * (positionInGrid.x + 0.5f);
        float addY = oneFieldSize.y * (positionInGrid.y + 0.5f);
        Vector2 roundedSpawnPosition = new Vector2(battleAreaCorner.x + addX, battleAreaCorner.y + addY);

        SpawnDefender(currentDefender.gameObject, roundedSpawnPosition, positionInGrid);
        DefenderSelectionManager.Instance.NoticeWasSpawned();
    }

    public void SpawnDefender(GameObject currentDefender, Vector2 roundedPosition, Vector2Int positionInGrid)
    {
        GameObject newDefender = Instantiate(currentDefender, roundedPosition, Quaternion.identity, ObjectsOrganizer.Instance.DefenderParent);
        grid[positionInGrid.x, positionInGrid.y] = newDefender;
        Defender defComp = newDefender.GetComponent<Defender>();
        if (defComp)
        {
            defComp.GridPosition = positionInGrid;
        }
    }

    private Vector2Int CalculatePositionOnGrid(Vector2 mousePosition)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(battleAreaRectTran, mousePosition, myCamera, out localPoint);
        localPoint = Rect.PointToNormalized(battleAreaRect, localPoint);
        return new Vector2Int((int)(localPoint.x / oneFieldNormalizedSize.x), (int)(localPoint.y / oneFieldNormalizedSize.y));
    }

    private Vector2 GetBattleAreaLeftBottomCorner()
    {
        Vector3[] corners = new Vector3[4];
        battleAreaRectTran.GetWorldCorners(corners);
        return corners[0];
    }
}
