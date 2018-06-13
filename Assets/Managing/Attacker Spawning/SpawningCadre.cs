using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningCadre : MonoBehaviour
{
    public static SpawningCadre Instance { get; private set; }

    [SerializeField] private RectTransform battleArenaRectTran;
    [SerializeField] private RectTransform grassAreaRectTran;

    private void Awake()
    {
        Instance = this;
    }

    public RectTransform BattleArenaRectTran
    {
        get { return battleArenaRectTran; }
    }

    public RectTransform GrassAreaRectTran
    {
        get { return grassAreaRectTran; }
    }
}
