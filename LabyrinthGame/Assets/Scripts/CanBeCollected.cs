using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanBeCollected : MonoBehaviour{
    
    private GameObject ob;
    public GameObject interactionUI;
    public GameObject objToActivate;
    public string myText;
    public TextMeshProUGUI interactionText;

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
            Debug.Log("In reach");
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
            interactionUI.SetActive(false);
            objToActivate.SetActive(true);
            ob.SetActive(false);
        }
    }
}
