using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private float speed;
	[SerializeField] private float damage;
    private bool hasDeliveredDamage = false;

	void Start ()
	{
		
	}
	
	void Update ()
	{
		transform.Translate(Vector3.right * Time.deltaTime * speed);
	}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(hasDeliveredDamage)
            { return; }

        Movable target = collider.gameObject.GetComponent<Movable>();
        if (target)
        {
            Health health = target.GetComponent<Health>();
            if(health)
            {
                hasDeliveredDamage = true;
                health.DealDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
