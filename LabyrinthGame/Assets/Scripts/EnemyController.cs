using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform[] waypoints;
    public float idleTime = 2f;
    public float walkSpeed = 2f;
    public float runSpeed = 6f;
    public float sightDistance = 10f;
    public float attackDistance = 2f;

    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private Animator animator;
    private float idleTimer = 0;
    private Transform player;

    //attack 
    public int damage = 10;
    public float attackCooldown = 1f;
    private float attackTimer;

    private enum SpiderState { Idle, Walk, Run, Attack, Damage};  //Attack
    private SpiderState currentState = SpiderState.Idle;

    //private bool isRunningAnimation = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SetDestinationToWaypoint();
    }


    void Update()
    {
        if (attackTimer > 0f) {
            attackTimer -= Time.deltaTime;
        }
        switch (currentState) {
            case SpiderState.Idle:
                idleTimer += Time.deltaTime;
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isAttacking", false);

                if (idleTimer >= idleTime) {
                    NextWaypoint();
                }

                CheckForPlayerDetection();
                break;

            case SpiderState.Walk:
                idleTimer = 0f;
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
                animator.SetBool("isAttacking", false);

                if (agent.remainingDistance <= agent.stoppingDistance) {
                    currentState = SpiderState.Idle;
                }

                CheckForPlayerDetection();
                break;

            case SpiderState.Run:
                idleTimer = 0f;
                agent.speed = runSpeed;

                agent.SetDestination(player.position);
                //isRunningAnimation = true;
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
                animator.SetBool("isAttacking", false);


                if (Vector3.Distance(transform.position, player.position) > sightDistance) {
                    currentState = SpiderState.Walk;
                    agent.speed = walkSpeed;
                }

                if (Vector3.Distance(transform.position, player.position) <= attackDistance) {
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isAttacking", true);
                    currentState = SpiderState.Attack;
                }


                break;

            case SpiderState.Attack:
                idleTimer = 0f;
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
                //animator.SetBool("isAttacking", true);

                AttackPlayer();

                if (Vector3.Distance(transform.position, player.position) > attackDistance) {
                currentState = SpiderState.Walk;
                animator.SetBool("isAttacking", false);
                }

                break;

        }
    }


    private void AttackPlayer() {
        if (attackTimer <= 0f) {


            animator.SetBool("isAttacking", true);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, attackDistance)) {
                if (hit.collider.CompareTag("Player")) {
                    PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();

                    if (playerHealth != null) {
                        playerHealth.TakeDamage(damage);
                    }
                }

                StartCoroutine(endAnimations());
                //start cooldown
                attackTimer = attackCooldown;
            }
            else {
                StartCoroutine(endAnimations());
                attackTimer = attackCooldown;
            }
        }
    }

    private void CheckForPlayerDetection() {
        RaycastHit hit;
        Vector3 playerDirection = player.position - transform.position;

        if (Physics.Raycast(transform.position, playerDirection.normalized, out hit, sightDistance)) {
            if (hit.collider.CompareTag("Player")) {
                currentState = SpiderState.Run;
                Debug.Log("Player detected!");
            }
        }
    }

    private void NextWaypoint() {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        SetDestinationToWaypoint();
    }

    private void SetDestinationToWaypoint() {
        agent.SetDestination(waypoints[currentWaypointIndex].position);
        currentState = SpiderState.Walk;
        agent.speed = walkSpeed;
        animator.enabled = true;
    }

    IEnumerator endAnimations() {
        yield return new WaitForSeconds(attackTimer);
        animator.SetBool("isAttacking", false);
    }

    //draw///////////////////////////////////////////////////////
    //private void OnDrawGizmos() {
    //    Gizmos.color = currentState == SpiderState.Run ? Color.red : Color.green;
    //    Gizmos.DrawLine(transform.position, player.position);
    //}
    ////////////////////////////////////////////////////////////

}
