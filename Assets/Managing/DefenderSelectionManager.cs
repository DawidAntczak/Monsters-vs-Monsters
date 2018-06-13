using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class DefenderSelectionManager : MonoBehaviour
{
    public static DefenderSelectionManager Instance { get; private set; }
    private Dictionary<DefenderButton, int> buttonIndexesDict = new Dictionary<DefenderButton, int>();
    private DefenderButton[] defenders;
    private Image [] images;
    private int selectedDefenderIndex = -1;

    private void Awake()
    {
        Instance = this;
    }

    public void GiveChosenDefenders(DefenderButton[] chosenDefenders)
    {
        foreach(DefenderButton def in chosenDefenders)
        {
            def.transform.SetParent(transform);
            def.transform.localScale = Vector3.one;
        }
        Init();
    }

    private void Init()
    {
        defenders = transform.GetComponentsInChildren<DefenderButton>();
        images = new Image[defenders.Length];
        for (int i = 0; i < defenders.Length; i++)
        {
            buttonIndexesDict.Add(defenders[i], i);
            images[i] = defenders[i].GetComponent<Image>();
            images[i].color = Color.grey;
        }
    }

    public void InformSystemAboutClick(DefenderButton defenderButton)
    {
        int newSelectedIndex = buttonIndexesDict[defenderButton];
        if (selectedDefenderIndex == newSelectedIndex)
            { return; }

        if(selectedDefenderIndex >= 0)
        {
            images[selectedDefenderIndex].color = Color.grey;
        }

        selectedDefenderIndex = newSelectedIndex;
        images[selectedDefenderIndex].color = Color.white;
    }

    public Defender CurrentDefender()
    {
        if (selectedDefenderIndex < 0)
        {
            return null;
        }
        else
        {
            return defenders[selectedDefenderIndex].DefenderPrefab;
        }
    }

    public bool IsReadyToSpawn()
    {
        if(selectedDefenderIndex < 0)
            { return false; }

        return defenders[selectedDefenderIndex].IsReadyToSpawn();
    }

    public void NoticeWasSpawned()
    {
        defenders[selectedDefenderIndex].NoticeWasSpawned();
    }
}
