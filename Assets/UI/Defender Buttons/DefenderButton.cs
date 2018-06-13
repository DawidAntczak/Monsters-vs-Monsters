using UnityEngine;
using UnityEngine.UI;

public class DefenderButton : MonoBehaviour
{
    [SerializeField] private Defender defenderPrefab;
    [SerializeField] private Image fillableImage;
    private float timeNeededToSpawn;
    private float timePassed = 0;

    public Defender DefenderPrefab
    {
        get { return defenderPrefab; }
    }

    void Start()
    {
        SetTheCostLabel();
        gameObject.AddComponent<Button>().onClick.AddListener(Clicked);

        timePassed = timeNeededToSpawn = defenderPrefab.SpawnTime;
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
        fillableImage.fillAmount = timePassed / timeNeededToSpawn;
    }

    private void Clicked()
    {
        if(ChoosePlantsSystem.Instance)
        {
            ChoosePlantsSystem.Instance.ReportClicked(this);
        }
        else
        {
            DefenderSelectionManager.Instance.InformSystemAboutClick(this);
        }
    }

    private void SetTheCostLabel()
    {
        Text costLabel = GetComponentInChildren<Text>();
        costLabel.text = defenderPrefab.StarCost.ToString();
    }

    public bool IsReadyToSpawn()
    {
        return timePassed >= timeNeededToSpawn;
    }

    public void NoticeWasSpawned()
    {
        timePassed = 0f;
    }
}
