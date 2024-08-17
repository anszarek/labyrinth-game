using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelchairController : MonoBehaviour
{
    private GameObject ob;
    public Animator wheelchairAnim;
    public BoxCollider triggerCollider;
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
