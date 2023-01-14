using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilSunflower : MonoBehaviour
{
    [SerializeField] private float minInterval = 10f;
    [SerializeField] private float maxInterval = 15f;
    [Tooltip("Inaccuracy of position of spawned suns")]
    [SerializeField] private float dispersion = 20f;
    [SerializeField] private CollectableSun collectableSun;
    
	void Start ()
    {
        StartCoroutine(ProduceStars());
	}

    private IEnumerator ProduceStars()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
            BirthSun();
        }
    }

    private void BirthSun()
    {
        Instantiate(collectableSun,
                    transform.position + new Vector3(Random.Range(-dispersion, dispersion), Random.Range(-dispersion, dispersion), -0.5f),
                    Quaternion.identity, ObjectsOrganizer.Instance.CollectableParent);
    }
}
