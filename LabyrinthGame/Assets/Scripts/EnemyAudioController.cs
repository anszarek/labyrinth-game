using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioController : MonoBehaviour
{
    public AudioSource walkSound;
    public AudioSource runSound;
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) {
            if (runSound.isPlaying) {
                runSound.Stop();
            }
            if (!walkSound.isPlaying) {
                walkSound.Play();
            }

        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run")) {
            if (walkSound.isPlaying) {
                walkSound.Stop();
            }
            if (!runSound.isPlaying) {
                runSound.Play();
            }
        }
        else {
            if (walkSound.isPlaying) {
                walkSound.Stop();
            }
            if (runSound.isPlaying) {
                runSound.Stop();
            }
        }
    }
}
