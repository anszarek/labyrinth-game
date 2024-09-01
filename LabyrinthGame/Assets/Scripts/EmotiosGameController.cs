using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EmotiosGameController : MonoBehaviour
{
    public Text HeartRateValue;
    public Light phoneFlashlight;
    public EnemyController[] enemyControllers;
    public TrapController[] trapControllers;
    public AudioSource heartbeatSound;

    //store value
    private Color originalColor;
    private float originalRange;
    private float[] originalSightDistances;
    private int[] originalDamage;
    private float[] originalAttackCooldown;
    private float originalTrapCooldown = 6f;



    private void Start() {
        
        originalColor = HeartRateValue.color;

        if (phoneFlashlight != null) {
            originalRange = phoneFlashlight.range;
        }

        if (enemyControllers != null && enemyControllers.Length > 0) {
            originalSightDistances = new float[enemyControllers.Length];
            originalDamage = new int[enemyControllers.Length];
            originalAttackCooldown = new float[enemyControllers.Length];

            for (int i = 0; i < enemyControllers.Length; i++) {
                if (enemyControllers[i] != null) {
                    originalSightDistances[i] = enemyControllers[i].sightDistance;
                    originalDamage[i] = enemyControllers[i].damage;
                    originalAttackCooldown[i] = enemyControllers[i].attackCooldown;
                }
            }
        }

        if (heartbeatSound != null) {
            heartbeatSound.Stop();
        }
    }

    void Update()
    {
        if (HeartRateValue != null) {
            int currentValue;
            if(int.TryParse(HeartRateValue.text, out currentValue)) {

                if (currentValue > 95) {
                    
                    //color
                    HeartRateValue.color = Color.red;

                    //change the visibility
                    phoneFlashlight.range = 15f;

                    //change the traps cooldown

                    if (trapControllers != null) {
                        foreach (TrapController trapController in trapControllers) {
                            if (trapController != null) {
                                float randomCooldown = Random.Range(1f, 10f);
                                trapController.attackCooldown = randomCooldown;
                            }
                        }
                    }

                    //add heartbeat sound

                    if (heartbeatSound != null && !heartbeatSound.isPlaying) {
                        heartbeatSound.Play();
                    }

                    //enemies changes (area of detection player, attack power and frequency)

                    foreach (EnemyController enemyController in enemyControllers) {
                        if (enemyController != null) {
                            for (int i = 0; i < enemyControllers.Length; i++) {
                                if (enemyControllers[i] != null) {
                                    if (i == 0) {
                                        // First enemy (monster)
                                        enemyControllers[i].sightDistance = 30f;
                                        enemyControllers[i].damage = 30;
                                        enemyControllers[i].attackCooldown = 1.5f;
                                    }
                                    else {
                                        // Other enemies (spiders)
                                        enemyControllers[i].sightDistance = 20f;
                                        enemyControllers[i].damage = 15;
                                        enemyControllers[i].attackCooldown = 1.5f;
                                    }
                                }
                            }
                        }
                    }

                } else {
                    //color
                    HeartRateValue.color = originalColor;

                    //visibility
                    phoneFlashlight.range = originalRange;

                    //trap
                    if (trapControllers != null) {
                        foreach (TrapController trapController in trapControllers) {
                            if (trapController != null) {
                                trapController.attackCooldown = originalTrapCooldown;
                            }
                        }
                    }

                    //sound
                    if (heartbeatSound != null && heartbeatSound.isPlaying) {
                        heartbeatSound.Stop();
                    }

                    //enemy
                    if (enemyControllers != null) {
                        for (int i = 0; i < enemyControllers.Length; i++) {
                            if (enemyControllers[i] != null) {
                                enemyControllers[i].sightDistance = originalSightDistances[i];
                                enemyControllers[i].damage = originalDamage[i];
                                enemyControllers[i].attackCooldown = originalAttackCooldown[i];
                            }
                        }
                    }
                }
            }
            else{

            }

        }
    }
}
