using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OpenChest : MonoBehaviour {

    private GameObject ob;
    public GameObject interactionUI;
    public GameObject objToUnlock;
    public TextMeshProUGUI interactionText;
    private bool inReach;
    public string myText;


    void Start()
    {
        ob = this.gameObject;
        interactionUI.SetActive(false);
        objToUnlock.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Reach") {
            inReach = true;
            interactionText.text = myText;
            interactionUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Reach") {
            inReach = false;
            interactionUI.SetActive(false);
            interactionText.text = "";
        }
    }

    void Update()
    {
        if(inReach && Input.GetButtonDown("Interact")) {
            interactionUI.SetActive(false);
            objToUnlock.SetActive(true);
            ob.GetComponent<Animator>().SetBool("open", true);
            ob.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
