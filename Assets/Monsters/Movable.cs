using UnityEngine;
using System.Collections;

public class Movable : MonoBehaviour
{
    public int LaneIndex { get; set; }
    [SerializeField] private float movingSpeed;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
	
    public void MoveForward(float currentSpeed)
    {
        rectTransform.Translate(Vector2.right * Time.deltaTime * currentSpeed);
    }

    public void ChangeToNeighborLane()
    {
        int maxInd = LevelConfig.Instance.Rows - 1;
        if (maxInd > 0)
        {
            if (LaneIndex == 0)
            {
                ChangeToLane(1);
            }
            else if (LaneIndex == maxInd)
            {
                ChangeToLane(maxInd-1);
            }
            else
            {
                int add = Random.Range(0, 2) == 0 ? -1 : 1;
                ChangeToLane(LaneIndex + add);
            }
        }
    }

    private void ChangeToLane(int index)
    {
        StopAllCoroutines();
        float startY = SpawningSystem.Instance.SpawnersTransforms[LaneIndex].position.y;
        float endY = SpawningSystem.Instance.SpawnersTransforms[index].position.y;

        StartCoroutine(ChangeYPosition(startY, endY, index));
    }

    private IEnumerator ChangeYPosition(float startY, float endY, int targetLaneIndex)
    {
        yield return new WaitForSeconds(1f);
        float timeNeeded = 1f;
        float timeElapsed = 0f;
        float newY;
        while (Mathf.Abs(timeElapsed - timeNeeded) > Mathf.Epsilon)
        {
            yield return null;
            timeElapsed = Mathf.Clamp(timeElapsed + Time.deltaTime, 0f, timeNeeded);
            newY = timeElapsed/timeNeeded * (endY - startY);
            transform.position = new Vector2(transform.position.x, startY + newY);
        }
        LaneIndex = targetLaneIndex;
    }

    public float WalkingSpeed
    {
        get { return movingSpeed; }
    }
}
