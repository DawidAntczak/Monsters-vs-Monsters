using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSun : MonoBehaviour
{
    [SerializeField] private int sunAmount = 10;
    [SerializeField] private float timeToDisappear = 15f;

    private void Start()
    {
        Destroy(gameObject, timeToDisappear);
    }

    public void Collect()
    {
        SunCountDisplay.Instance.AddStars(sunAmount);
        Destroy(gameObject);
    }
}
