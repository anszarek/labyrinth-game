using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddPlayerHealth : MonoBehaviour
{
    private GameObject ob;
    public int healAmount = 10;
    public GameObject interactionUI;
    public string myText;
    public TextMeshProUGUI interactionText;

    public PlayerHealth playerHealth;

    private bool inReach;

    // Start is called before the first frame update
    void Start()
    {
        ob = this.gameObject;
        interactionUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inReach && Input.GetButtonDown("Interact")) {
            interactionUI.SetActive(false);
            playerHealth.Heal(healAmount);
            ob.SetActive(false);
        }
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

    /////////////////////////////
    ///


}
