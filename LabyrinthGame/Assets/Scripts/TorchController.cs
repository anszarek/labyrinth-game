using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class TorchController : MonoBehaviour
{
    public Animator torchAnim;
    public Animator wallAnim;

    public GameObject interactionUI;
    public string myText;
    public TextMeshProUGUI interactionText;

    private bool inReach;

    void Start()
        {
            interactionUI.SetActive(false);
            torchAnim.SetBool("moved", false);
            wallAnim.SetBool("moved", false);
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

    void Update()
    {
        if (inReach && Input.GetButtonDown("Interact")) {
            interactionUI.SetActive(true);
            bool isMoved = torchAnim.GetBool("moved");
            torchAnim.SetBool("moved", !isMoved);
            wallAnim.SetBool("moved", !isMoved);
        }
    }

}
