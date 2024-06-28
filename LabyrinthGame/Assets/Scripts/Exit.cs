using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public GameObject interactionUI;
    public string myText;
    public TextMeshProUGUI interactionText;
    public GameObject UITextInfo;

    public GameObject inventoryKey;
    public GameObject fadeFX;

    public string nextSceneName;

    private bool inReach;


    void Start()
    {
        interactionUI.SetActive(false);
        UITextInfo.SetActive(false);
        inventoryKey.SetActive(false);
        fadeFX.SetActive(false);
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
            UITextInfo.SetActive(false);
        }
    }

    void Update()
    {
        if (inReach && Input.GetButtonDown("Interact") && !inventoryKey.activeInHierarchy) {
            interactionUI.SetActive(true);
            UITextInfo.SetActive(true);
        }
        if (inReach && Input.GetButtonDown("Interact") && inventoryKey.activeInHierarchy) {
            interactionUI.SetActive(false);
            UITextInfo.SetActive(false);
            fadeFX.SetActive(true);
            StartCoroutine(endingGame());
        }

    }

    IEnumerator endingGame() {
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene(nextSceneName);
    }
}
