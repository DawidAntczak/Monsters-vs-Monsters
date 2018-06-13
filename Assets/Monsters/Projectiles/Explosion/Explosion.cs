using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private ParticleSystem particleSys;

	void Start ()
    {
        particleSys = GetComponent<ParticleSystem>();
        Destroy(gameObject, particleSys.main.duration);
	}
}
