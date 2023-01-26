using UnityEngine;
using UnityEngine.UI;

public class ChoosePlantsSystem : MonoBehaviour
{
    public static ChoosePlantsSystem Instance { get; private set; }
    [SerializeField] private RectTransform choosenLayout;
    [SerializeField] private RectTransform availableLayout;
    [SerializeField] private Button startButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DefenderButton [] defenders = LevelConfig.Instance.AvaibleDefenders;
        foreach(DefenderButton def in defenders)
        {
            Instantiate(def, availableLayout);
        }
        RecalculateIfCanStart();
    }

    public void ReportClicked(DefenderButton defender)
    {
        if(defender.transform.IsChildOf(choosenLayout))
        {
            RemoveFromChoosen(defender);
        }
        else if(choosenLayout.childCount < LevelConfig.Instance.MaxDefenders)
        {
            AddToChoosen(defender);
        }
        RecalculateIfCanStart();
    }

    private void AddToChoosen(DefenderButton defender)
    {
        defender.transform.SetParent(choosenLayout);
    }

    private void RemoveFromChoosen(DefenderButton defender)
    {
        defender.transform.SetParent(availableLayout);
    }

    private void RecalculateIfCanStart()
    {
        if (LevelConfig.Instance.MaxDefenders == choosenLayout.childCount)
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }

    public void Confirmed()
    {
        DefenderButton[] chosenDefender = new DefenderButton[choosenLayout.childCount];
        for(int i = 0; i < choosenLayout.childCount; i++)
        {
            chosenDefender[i] = choosenLayout.GetChild(i).GetComponent<DefenderButton>();
        }
        LevelConfig.Instance.StartGame();
        DefenderSelectionManager.Instance.GiveChosenDefenders(chosenDefender);
        Destroy(gameObject);
    }
}
