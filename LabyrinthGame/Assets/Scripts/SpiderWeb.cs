using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : MonoBehaviour {
    public int resistance = 10;
    public AudioSource sound;
    private int currentResistance;


    void Start() {
        currentResistance = resistance;
    }

    public void TakeDamage(int damageAmount) {
        currentResistance -= damageAmount;
        sound.Play();
        if (currentResistance <= 0) {
            StartCoroutine(DisableAfterDelay(0.5f));
        }
    }

    IEnumerator DisableAfterDelay(float delay) {

        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);

    }
}
