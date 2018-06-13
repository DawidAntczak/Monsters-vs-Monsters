using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    public void DealDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0f)
        {
            DestroyGameObject();
        }
        float ratio = currentHealth / maxHealth;
        spriteRenderer.color = new Color(1f, ratio, ratio);
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}