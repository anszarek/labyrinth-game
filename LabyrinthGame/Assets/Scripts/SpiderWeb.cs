using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : MonoBehaviour {
    public int resistance = 10;
    private int currentResistance;


    void Start() {
        currentResistance = resistance;
    }

    public void TakeDamage(int damageAmount) {
        currentResistance -= damageAmount;

        if (currentResistance <= 0) {
            gameObject.SetActive(false);
        }
    }
}
