using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movable))]
public class Lizard : MonoBehaviour
{
    private Animator animator;
    private Movable attacker;

    void Start ()
    {
        animator = GetComponent<Animator>();
        attacker = GetComponent<Movable>();
        transform.position = new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z);
    }
	
	void Update ()
    {
		
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject obj = collider.gameObject;

        if (!obj.GetComponent<Defender>())
        {
            return;
        }

        //attacker.Attack(obj);
        animator.SetBool("is attacking", true);
    }
}
