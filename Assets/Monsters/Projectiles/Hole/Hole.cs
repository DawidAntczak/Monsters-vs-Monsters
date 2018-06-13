using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] private float disappearingTime = 10f;

	void Start ()
    {
        Destroy(gameObject, disappearingTime);
	}
}