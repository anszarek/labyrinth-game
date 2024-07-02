using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;  //to private

    public string nextSceneName;
    public float delay = 0.5f;
    public GameObject fadeout;


    private void Start() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount) {
        currentHealth -= damageAmount;
        //gameObject.GetComponent<Animator>().SetTrigger("TakeDamage");

        if (currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        //gameObject.GetComponent<Animator>().SetBool("Death", true);
        fadeout.SetActive(true);
        Invoke("LoadNextScene", delay);

    }

    private void LoadNextScene() {
        SceneManager.LoadScene(nextSceneName);
    }
}
