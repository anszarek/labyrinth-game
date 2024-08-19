using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderTrapController : MonoBehaviour
{
    private GameObject ob;
    public Animator spiderTrapAnim;
    public BoxCollider triggerCollider;
    public GameObject model;
    public GameObject spider;

    

    void Start() {
        ob = this.gameObject;
        triggerCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Reach")) {
            ob.GetComponent<Animator>().SetBool("start", true);
            triggerCollider.enabled = false;

            StartCoroutine(EnableComponentsAfterDelay(2.0f));
        }
    }

    IEnumerator EnableComponentsAfterDelay(float delay) {
        
        yield return new WaitForSeconds(delay);
        model.SetActive(false);
        spider.SetActive(true);
        
    }
}
