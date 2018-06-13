using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManEatingPlant : MonoBehaviour
{
    [SerializeField] private BoxCollider2D attackRangeCollider;
    [SerializeField] private SpriteRenderer eatedAttacker;
    [SerializeField] private float attackCooldown = 10f;

    private int animAttackTriggerHash;
    private int animEatedTriggerHash;
    private Animator animator;
    private bool isReadyToAttack = true;
    private BoxCollider2D [] enemyColliders;
    private Health currentTarget;
    private ContactFilter2D contactFilter;

    void Start()
    {
        SetupAnimatorHashes();
        animator = GetComponent<Animator>();
        enemyColliders = new BoxCollider2D[1];
        contactFilter = new ContactFilter2D();
        contactFilter.NoFilter();
        contactFilter.SetLayerMask(1 << Layers.enemyLayer);
    }
	
	void Update ()
    {
        if(!isReadyToAttack)
            { return; }

        
        enemyColliders[0] = null;
        attackRangeCollider.OverlapCollider(contactFilter, enemyColliders);
        if (enemyColliders[0] != null)
        {
            currentTarget = enemyColliders[0].GetComponent<Health>();
            if (currentTarget)
            {
                Attack();
            }
        }
    }

    private void SetupAnimatorHashes()
    {
        animAttackTriggerHash = Animator.StringToHash("attack trigger");
        animEatedTriggerHash = Animator.StringToHash("eated trigger");
    }

    private void Attack()
    {
        animator.SetTrigger(animAttackTriggerHash);

        isReadyToAttack = false;
        StartCoroutine(WaitForReady());
    }

    private void AnimAttack()
    {
        if(currentTarget)
        {
            currentTarget.DealDamage(Mathf.Infinity);
            eatedAttacker.enabled = true;
        }
        else
        {
            StopAllCoroutines();
            animator.SetTrigger(animEatedTriggerHash);
            isReadyToAttack = true;
        }
    }

    private IEnumerator WaitForReady()
    {
        yield return new WaitForSeconds(attackCooldown);
        animator.SetTrigger(animEatedTriggerHash);
        isReadyToAttack = true;
        eatedAttacker.enabled = false;
    }
}
