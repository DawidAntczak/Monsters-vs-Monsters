using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smuggler : MonoBehaviour
{
    [Tooltip("Min spawn distance from the right screen edge")]
    [SerializeField] private float minAdvantage = 200f;
    [Tooltip("Max spawn distance from the right screen edge")]
    [SerializeField] private float maxAdvantage = 500f;
    [SerializeField] private GameObject package;
    private Movable attacker;
    private Animator animator;
    private int animPlantTriggerHash;
    private bool planting = false;

    void Start ()
    {
        transform.Translate(Vector3.left * Random.Range(minAdvantage, maxAdvantage));
        attacker = GetComponent<Movable>();
        animator = GetComponent<Animator>();
        SetupAnimatorHashes();
    }

    void Update ()
    {
        if (!planting)
        {
            attacker.MoveForward(attacker.WalkingSpeed);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Health>() && collision.GetComponent<PinkMushroom>() == null)
        {
            animator.SetTrigger(animPlantTriggerHash);
            planting = true;
        }
    }

    private void SetupAnimatorHashes()
    {
        animPlantTriggerHash = Animator.StringToHash("plant trigger");
    }

    private void AnimPlant()
    {
        GameObject go = Instantiate(package, transform.position, Quaternion.identity, ObjectsOrganizer.Instance.ProjectileParent);
        if(gameObject.layer == Layers.defenderLayer)
        {
            go.layer = Layers.defenderProjectileLayer;
        }
        Destroy(gameObject);
    }
}
