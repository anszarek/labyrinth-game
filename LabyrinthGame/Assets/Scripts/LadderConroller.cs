using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LadderConroller : MonoBehaviour
{
    private GameObject ob;
    public GameObject interactionUI;
    public BoxCollider colliderToActivate;
    public TextMeshProUGUI interactionText;
    private bool inReach;
    public string myText;
    public LadderConroller ladderController;


    public AudioSource sound;


    void Start() {
        ob = this.gameObject;
        interactionUI.SetActive(false);
        colliderToActivate.enabled = false;
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
            sound.Play();
            interactionUI.SetActive(false);
            colliderToActivate.enabled = true;
            ob.GetComponent<Animator>().SetBool("move", true);
            ob.GetComponent<MeshCollider>().enabled = false;
            ladderController.enabled = false;
        }
    }
}
