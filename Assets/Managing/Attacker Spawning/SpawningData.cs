using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spawning Data", menuName = "Spawning System", order = 1)]
public class SpawningData : ScriptableObject
{
    [SerializeField] private GameObject[] attackers;
    [SerializeField] private RuntimeAnimatorController spawningAC;

    public GameObject[] Attackers
    {
        get { return attackers; }
    }

    public RuntimeAnimatorController SpawningAC
    {
        get { return spawningAC; }
    }
}
