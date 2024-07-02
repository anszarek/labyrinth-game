using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public float attackCooldown = 3f;
    public float startAttackTime = 3f;
    public int damage = 2;

    private float attackTimer;
    private bool working = true;
    private float attackRange = 3f;

    public Animator trapAnim;

    private void Start() {
        trapAnim.SetBool("active", false);
        attackTimer = startAttackTime;
    }

    void Update()
    {
        if (working) {
            TrapActive();
        }
        

        if (attackTimer > 0f) {
            attackTimer -= Time.deltaTime;
        }


    }

    void TrapActive() {
        if (attackTimer <= 0f) {

            trapAnim.SetBool("active", true);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange)) {
                if (hit.collider.CompareTag("Player")) {
                    PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();

                    if (playerHealth != null) {
                        playerHealth.TakeDamage(damage);
                    }
                }

                StartCoroutine(endAnimations());
                attackTimer = attackCooldown;
            }
            else {
                StartCoroutine(endAnimations());
                attackTimer = attackCooldown;
            }
        }
    }
  
    
    IEnumerator endAnimations() {
        yield return new WaitForSeconds(attackTimer);
        trapAnim.SetBool("active", false);
    }



}
