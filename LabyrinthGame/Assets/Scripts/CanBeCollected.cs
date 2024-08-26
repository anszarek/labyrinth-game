using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanBeCollected : MonoBehaviour{
    
    private GameObject ob;
    public GameObject interactionUI;
    public GameObject objToActivate;
    public GameObject objToDeactivate;
    public string myText;
    public TextMeshProUGUI interactionText;

    public AudioSource sound;


    private bool inReach;


    void Start() {
        ob = this.gameObject;
        interactionUI.SetActive(false);
        objToActivate.SetActive(false);
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
            //objToActivate.SetActive(true);
            objToDeactivate.SetActive(false);
            //ob.GetComponent<BoxCollider>().enabled = false;
            //ob.SetActive(false);
            StartCoroutine(DisableAfterDelay(0.3f));
        }
    }

    IEnumerator DisableAfterDelay(float delay) {

        yield return new WaitForSeconds(delay);
        objToActivate.SetActive(true);
        //objToDeactivate.SetActive(false);
        ob.SetActive(false);

    }
}
