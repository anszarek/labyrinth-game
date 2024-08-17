using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMiniGame : MonoBehaviour
{
    [SerializeField] Transform topPivot;
    [SerializeField] Transform bottomPivot;

    [SerializeField] Transform myObject;

    float objectPosition;
    float objectDestination;

    float objectTimer;
    [SerializeField] float timerMultiplicator = 3f;

    float objectSpeed;
    [SerializeField] float smoothMotion = 1f;

    [SerializeField] Transform areaToStay;
    float areaPosition;
    [SerializeField] float areaSize = 0.1f;
    [SerializeField] float areaPower = 0.5f;
    float areaProgress;
    float areaMoveVelocity;
    [SerializeField] float areaMovePower = 0.01f;
    [SerializeField] float areaGravityPower = 0.005f;
    [SerializeField] float areaProgressDegradationPower = 0.1f;

    private void Update() {
        Objects();
        Area();
    }

    private void Objects() {
        objectTimer -= Time.deltaTime;
        if (objectTimer < 0f) {
            objectTimer = UnityEngine.Random.value * timerMultiplicator;
            objectDestination = UnityEngine.Random.value;
        }

        objectPosition = Mathf.SmoothDamp(objectPosition, objectDestination, ref objectSpeed, smoothMotion);
        myObject.position = Vector3.Lerp(bottomPivot.position, topPivot.position, objectPosition);
    }
    private void Area() {
        if (Input.GetKeyDown("space")) {
            areaMoveVelocity += areaMovePower * Time.deltaTime;
        }
        areaMoveVelocity -= areaGravityPower * Time.deltaTime;

        areaPosition += areaMoveVelocity;
        areaPosition = Mathf.Clamp(areaPosition, areaSize / 2, 1 - areaSize / 2);
        areaToStay.position = Vector3.Lerp(bottomPivot.position,topPivot.position, areaPosition);

    }


}
