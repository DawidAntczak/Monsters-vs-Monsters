using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] private float speed = 500;
    [SerializeField] private float damage = 10f;
	
	void Update ()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();
        if(!health)
            { return; }

        health.DealDamage(damage);
        if (collision.GetComponent<Fence>())
        {
            Destroy(gameObject);
        }
    }
}
