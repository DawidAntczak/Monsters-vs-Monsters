using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CruncherMonster : MonoBehaviour
{
    [SerializeField] private float damageAmount = 20f;
    private Movable attacker;
    private Animator animator;
    private bool isAttacking = false;
    private int animIsAttackingHash;
    private Health currentTarget;

    void Start ()
    {
        attacker = GetComponent<Movable>();
        animator = GetComponent<Animator>();

        SetupAnimHashes();
	}
	
	void Update ()
    {
        if(!isAttacking)
        {
            attacker.MoveForward(attacker.WalkingSpeed);
        }
        else
        {
            if (currentTarget == null || currentTarget.gameObject.layer == gameObject.layer)
            {
                isAttacking = false;
                animator.SetBool(animIsAttackingHash, false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();

        if(!health)
            { return; }

        currentTarget = health;
        isAttacking = true;
        animator.SetBool(animIsAttackingHash, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();
        if(health && health == currentTarget)
        {
            currentTarget = null;
        }
    }

    public void AnimAttack()
    {
        if(currentTarget)
        {
            currentTarget.DealDamage(damageAmount);
        }
    }

    private void SetupAnimHashes()
    {
        animIsAttackingHash = Animator.StringToHash("is attacking");
    }
}
