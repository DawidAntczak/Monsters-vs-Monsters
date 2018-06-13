using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] private float damageAmount = 200f;
    private bool particleInstantiated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();
        if (!health)
            { return; }

        health.DealDamage(damageAmount);
        if (!particleInstantiated)
        {
            GameObject go = Instantiate(explosionParticle, transform.position, Quaternion.identity, ObjectsOrganizer.Instance.ProjectileParent);
            particleInstantiated = true;
            Destroy(gameObject);
        }
    }
}
