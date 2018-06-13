using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wormhole : MonoBehaviour
{
    [SerializeField] private float damageAmount = 100f;
    [SerializeField] private Explosion explosionParticle;
    [SerializeField] private GameObject holePrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<FlyingMonster>())
            { return; }

        Health collidedHealth = collision.GetComponent<Health>();
        if (!collidedHealth)
            { return; }

        Explode();
        collidedHealth.DealDamage(damageAmount);
    }

    private void Explode()
    {
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        DefenderSpawner.Instance.SpawnDefender(holePrefab, transform.position, GetComponent<Defender>().GridPosition);
        Destroy(gameObject);
    }
}
