using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderTrapController : MonoBehaviour
{
    private GameObject ob;
    public Animator spiderAnim;
    public CapsuleCollider triggerCollider;
    private bool inReach;

    void Start() {
        ob = this.gameObject;
        triggerCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Reach") {
            inReach = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Reach") {
            inReach = false;
        }
    }

    void Update() {
        if (inReach) {
            ob.GetComponent<Animator>().SetBool("start", true);
            triggerCollider.enabled = false;
        }
    }
}
