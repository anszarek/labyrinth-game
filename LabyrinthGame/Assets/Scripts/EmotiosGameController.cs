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

                float thisPulse = Mathf.Clamp(currentValue, 70, 100);
                float normalizedPulse = Mathf.InverseLerp(70, 100, thisPulse);

                
                // light
                phoneFlashlight.range = Mathf.Lerp(originalRange, 20f, normalizedPulse);

                

                if (currentValue >= 90) {

                    //color

                    HeartRateValue.color = Color.red;

                    //add heartbeat sound

                    if (heartbeatSound != null && !heartbeatSound.isPlaying) {
                        heartbeatSound.Play();
                    }

                    //change the traps cooldown

                    if (trapControllers != null) {
                        foreach (TrapController trapController in trapControllers) {
                            if (trapController != null) {
                                float randomCooldown = Random.Range(1f, 4f);
                                trapController.attackCooldown = randomCooldown;
                            }
                        }
                    }

                    //enemies changes (area of detection player, attack power and frequency)

                    foreach (EnemyController enemyController in enemyControllers) {
                        if (enemyController != null) {
                            for (int i = 0; i < enemyControllers.Length; i++) {
                                if (enemyControllers[i] != null) {
                                    if (i == 0) {
                                        // First enemy (monster)
                                        enemyControllers[i].sightDistance = 35f;
                                        enemyControllers[i].damage = 30;
                                        enemyControllers[i].attackCooldown = 0.8f;
                                    }
                                    else {
                                        // Other enemies (spiders)
                                        enemyControllers[i].sightDistance = 22f;
                                        enemyControllers[i].damage = 20;
                                        enemyControllers[i].attackCooldown = 0.8f;
                                    }
                                }
                            }
                        }
                    }

                }
                else if (currentValue >= 80 && currentValue < 90) {

                    //color
                    HeartRateValue.color = originalColor;

                    //sound

                    if (heartbeatSound != null && heartbeatSound.isPlaying) {
                        heartbeatSound.Stop();
                    }

                    //change the traps cooldown

                    if (trapControllers != null) {
                        foreach (TrapController trapController in trapControllers) {
                            if (trapController != null) {
                                float randomCooldown = Random.Range(4f, 6f);
                                trapController.attackCooldown = randomCooldown;
                            }
                        }
                    }

                    //enemies changes (area of detection player, attack power and frequency)

                    foreach (EnemyController enemyController in enemyControllers) {
                        if (enemyController != null) {
                            for (int i = 0; i < enemyControllers.Length; i++) {
                                if (enemyControllers[i] != null) {
                                    if (i == 0) {
                                        // First enemy (monster)
                                        enemyControllers[i].sightDistance = 28f;
                                        enemyControllers[i].damage = 25;
                                        enemyControllers[i].attackCooldown = 1f;
                                    }
                                    else {
                                        // Other enemies (spiders)
                                        enemyControllers[i].sightDistance = 18f;
                                        enemyControllers[i].damage = 15;
                                        enemyControllers[i].attackCooldown = 1f;
                                    }
                                }
                            }
                        }
                    }

                }
                else {

                    //color
                    HeartRateValue.color = originalColor;

                    //sound

                    if (heartbeatSound != null && heartbeatSound.isPlaying) {
                        heartbeatSound.Stop();
                    }

                    //trap
                    if (trapControllers != null) {
                        foreach (TrapController trapController in trapControllers) {
                            if (trapController != null) {
                                trapController.attackCooldown = originalTrapCooldown;
                            }
                        }
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
            else {

            }

        }
    }
}
