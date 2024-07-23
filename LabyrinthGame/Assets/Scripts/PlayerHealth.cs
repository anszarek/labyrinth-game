using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;  //to private
    public Slider slider;

    public Image damageEffect;
    public float duration = 0.1f;
    public float fadeSpeed = 1.5f;

    public string nextSceneName;
    public float delay = 0.5f;
    public GameObject fadeout;

    private float durationTimer;

    private void Start() {
        currentHealth = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
        damageEffect.color = new Color(damageEffect.color.r, damageEffect.color.g, damageEffect.color.b, 0);
    }

    void Update() {
        if ( damageEffect.color.a > 0 ) {
            
            if (currentHealth < 30) {
                return;
            }

            durationTimer += Time.deltaTime;
            if (durationTimer > duration ) {
                float tempAlpha = damageEffect.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                damageEffect.color = new Color(damageEffect.color.r, damageEffect.color.g, damageEffect.color.b, tempAlpha);
            }
        }
    }

    public void TakeDamage(int damageAmount) {
        currentHealth -= damageAmount;
        slider.value = currentHealth;
        durationTimer = 0;
        damageEffect.color = new Color(damageEffect.color.r, damageEffect.color.g, damageEffect.color.b, 1);
        //gameObject.GetComponent<Animator>().SetTrigger("TakeDamage");

        if (currentHealth <= 0) {
            Die();
        }
    }

    public void Heal(int amount) {
        currentHealth += amount;
        
        if (currentHealth >  maxHealth) {
            currentHealth = maxHealth;
        }
        
        slider.value = currentHealth;
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
