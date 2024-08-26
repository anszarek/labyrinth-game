using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public AudioSource painSound;
    public AudioSource deathSound;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount) {
        painSound.Play();
        currentHealth -= damageAmount;
        gameObject.GetComponent<Animator>().SetTrigger("TakeDamage");

        if (currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {

        deathSound.Play(); 

        gameObject.GetComponent<Animator>().SetBool("Death", true);

    }
}
