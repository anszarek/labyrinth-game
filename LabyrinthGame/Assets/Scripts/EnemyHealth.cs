using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount) {
        currentHealth -= damageAmount;
        gameObject.GetComponent<Animator>().SetTrigger("TakeDamage");

        if (currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        gameObject.GetComponent<Animator>().SetBool("Death", true);
    }
}
