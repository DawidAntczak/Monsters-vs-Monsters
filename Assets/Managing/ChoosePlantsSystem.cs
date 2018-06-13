using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosePlantsSystem : MonoBehaviour
{
    public static ChoosePlantsSystem Instance { get; private set; }
    [SerializeField] private RectTransform choosenLayout;
    [SerializeField] private RectTransform availableLayout;
    
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
    }

    private void AddToChoosen(DefenderButton defender)
    {
        defender.transform.SetParent(choosenLayout);
    }

    private void RemoveFromChoosen(DefenderButton defender)
    {
        defender.transform.SetParent(availableLayout);
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
