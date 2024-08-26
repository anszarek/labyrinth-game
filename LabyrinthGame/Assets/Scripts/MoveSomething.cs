using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveSomething : MonoBehaviour
{
    public Animator moveAnim;

    public GameObject interactionUI;
    public string myText;
    public TextMeshProUGUI interactionText;

    public AudioSource sound;

    private bool inReach;

    void Start() {
        interactionUI.SetActive(false);
        moveAnim.SetBool("moved", false);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Reach") {
            inReach = true;
            interactionText.text = myText;
            interactionUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Reach") {
            inReach = false;
            interactionUI.SetActive(false);
            interactionText.text = "";
        }
    }

    void Update() {
        if (inReach && Input.GetButtonDown("Interact")) {
            interactionUI.SetActive(true);
            sound.Play();
            bool isMoved = moveAnim.GetBool("moved");
            moveAnim.SetBool("moved", !isMoved);
        }
    }
}
