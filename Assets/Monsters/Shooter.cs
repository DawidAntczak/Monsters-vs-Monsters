using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject gun;
    [SerializeField] private BoxCollider2D attackZone;
    private Animator animator;
    private int animIsAttackingHash;
    private int enemyLayerMask;

    private void Start()
    {
        animator = GetComponent<Animator>();
        SetupAnimHashes();
        enemyLayerMask = 1 << Layers.enemyLayer;
    }

    private void Update()
    {
        if(IsEnemyAhead())
        {
            animator.SetBool(animIsAttackingHash, true);
        }
        else
        {
            animator.SetBool(animIsAttackingHash, false);
        }
    }

    private void AnimFire()
    {
        Instantiate(projectile, gun.transform.position, Quaternion.identity, ObjectsOrganizer.Instance.ProjectileParent);
    }

    private void SetupAnimHashes()
    {
        animIsAttackingHash = Animator.StringToHash("is attacking");
    }

    private bool IsEnemyAhead()
    {
        return attackZone.IsTouchingLayers(enemyLayerMask);      
    }
}
