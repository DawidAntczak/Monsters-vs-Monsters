using UnityEngine;

[RequireComponent(typeof(Movable))]
public class Fox : MonoBehaviour
{
    private Animator animator;
    private Movable attacker;

    private int animJumpHash;
    private int animIsAttackingHash;

    private void Start()
    {
        animator = GetComponent<Animator>();
        attacker = GetComponent<Movable>();

        InitAnimatorHashes();
    }

    private void Update()
    {
        attacker.MoveForward(attacker.WalkingSpeed);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject collidedObj = collider.gameObject;

        if(!collidedObj.GetComponent<Defender>())
            { return; }
        

        if(collidedObj.GetComponent<Fence>())
        {
            Jump();
        }
        else
        {
            Attack(collidedObj);
            animator.SetBool(animIsAttackingHash, true);
        }
    }

    private void InitAnimatorHashes()
    {
        animJumpHash = Animator.StringToHash("jump trigger");
        animIsAttackingHash = Animator.StringToHash("is attacking");
    }

    private void Jump()
    {
        animator.SetTrigger(animJumpHash);
    }

    private void Attack(GameObject attackedObj)
    {

    }
}
