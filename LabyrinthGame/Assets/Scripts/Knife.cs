using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public float attackCooldown = 0.5f; // to change
    public float attackRange = 20f;   // to change

    public int damage = 5; // to change

    private float attackTimer;

    public Animator knife;

    void Start()
    {
        
    }

    void Update()
    {
        // Can attack?
        if (Input.GetButtonDown("Fire1")) {
            Attack();
        }

        //cooldown
        if (attackTimer > 0f) {
            attackTimer -= Time.deltaTime;
        }
    }

    void Attack() {
        
        if (attackTimer <= 0f) {

            knife.SetBool("attack", true);

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, attackRange)) {
                if (hit.collider.CompareTag("Enemy")) {
                    EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();

                    if (enemyHealth != null) {
                        enemyHealth.TakeDamage(damage);
                    }
                }

                if (hit.collider.CompareTag("SpiderWeb")) {
                    SpiderWeb spiderWeb = hit.collider.GetComponent<SpiderWeb>();
                    if (spiderWeb != null) {
                        spiderWeb.TakeDamage(damage);
                    }
                }

                StartCoroutine(endAnimations());

                //start cooldown
                attackTimer = attackCooldown;
            }
            else {
                Debug.Log("Cannot shoot");
            }
        }

    }

    IEnumerator endAnimations() {
        yield return new WaitForSeconds(attackTimer);  
        knife.SetBool("attack", false);
    }

}
