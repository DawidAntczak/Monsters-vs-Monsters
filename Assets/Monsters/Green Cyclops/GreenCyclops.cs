using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCyclops : MonoBehaviour
{
    [SerializeField] private GameObject laserBeam;
    [SerializeField] private GameObject gun;
    [SerializeField] private float attackInterval = 3f;
    private Animator animator;
    private Movable attacker;
    private int animBlockedHash;
    private bool isBlocked = false;
    private GameObject currentBlockingObject = null;

    void Start ()
    {
        animator = GetComponent<Animator>();
        attacker = GetComponent<Movable>();
        SetupAnimatorHashes();

        StartCoroutine(HandleAttack());
    }
	
	void Update ()
    {
		if(!isBlocked)
        {
            attacker.MoveForward(attacker.WalkingSpeed);
        }
        else
        {
            if (currentBlockingObject == null || currentBlockingObject.layer == gameObject.layer)
            {
                isBlocked = false;
                animator.SetBool(animBlockedHash, false);
                currentBlockingObject = null;
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Health>())
        {
            isBlocked = true;
            animator.SetBool(animBlockedHash, true);
            currentBlockingObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == currentBlockingObject)
        {
            currentBlockingObject = null;
        }
    }

    private IEnumerator HandleAttack()
    {
        while(true)
        {
            yield return new WaitForSeconds(attackInterval);
            GameObject go = Instantiate(laserBeam, gun.transform.position, transform.localRotation, ObjectsOrganizer.Instance.ProjectileParent);
            if(gameObject.layer == Layers.defenderLayer)
            {
                go.layer = Layers.defenderProjectileLayer;
            }
        }
    }

    private void SetupAnimatorHashes()
    {
        animBlockedHash = Animator.StringToHash("is blocked");
    }
}
