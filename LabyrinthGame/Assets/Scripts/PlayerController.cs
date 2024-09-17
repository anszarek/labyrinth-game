using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    //Camera
    public Camera playerCamera;

    //Movement
    public float walkSpeed = 4f;
    public float runSpeed = 5.5f;
    public float gravity = 10f;
    
    private float currentSpeedX;
    private float currentSpeedY;
    
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    float rotationY = 0;

    //Camera 
    public float lookSpeed = 2f;
    public float lookXLimit = 75f;
    public float cameraRotationSmooth = 5f;

    //Sounds
    public AudioClip[] footstepSounds;
    public Transform footstepAudioPosition;
    public AudioSource audioSource;

    private bool isWalking = false;
    private bool isFootstepCoroutineRunning = false;
    private AudioClip[] currentFootstepSounds;

    //Can move?
    private bool canMove = true;

    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentFootstepSounds = footstepSounds;
    }


    void Update()
    {
        //Player movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        if (canMove) {
            if(isRunning) {
                currentSpeedX = runSpeed * Input.GetAxis("Vertical");
                currentSpeedY = runSpeed * Input.GetAxis("Horizontal");
            }
            else {
                currentSpeedX = walkSpeed * Input.GetAxis("Vertical");
                currentSpeedY = walkSpeed * Input.GetAxis("Horizontal");
            }
        }else {
            currentSpeedX = 0;
            currentSpeedY = 0;
        }
        
        
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * currentSpeedX) + (right * currentSpeedY);
        
        if (!characterController.isGrounded) {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        characterController.Move(moveDirection * Time.deltaTime);

        //Camera movement

        if(canMove) {
            rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            rotationY += Input.GetAxis("Mouse X") * lookSpeed;

            Quaternion targetRotationX = Quaternion.Euler(rotationX, 0, 0);
            Quaternion targetRotationY = Quaternion.Euler(0, rotationY, 0);

            playerCamera.transform.localRotation = Quaternion.Slerp(playerCamera.transform.localRotation, targetRotationX, Time.deltaTime * cameraRotationSmooth);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationY, Time.deltaTime * cameraRotationSmooth);
        }

        if ((currentSpeedX != 0f || currentSpeedY != 0f) && !isWalking && !isFootstepCoroutineRunning) {
            isWalking = true;
            StartCoroutine(PlayFootstepSounds(2.0f / (isRunning ? runSpeed : walkSpeed)));
        }
        else if (currentSpeedX == 0f && currentSpeedY == 0f) {
            isWalking = false;
        }

    }

    // Play footstep sounds 
    IEnumerator PlayFootstepSounds(float footstepDelay) {
        isFootstepCoroutineRunning = true;

        while (isWalking) {
            if (currentFootstepSounds.Length > 0) {
                int randomIndex = Random.Range(0, currentFootstepSounds.Length);
                audioSource.transform.position = footstepAudioPosition.position;
                audioSource.clip = currentFootstepSounds[randomIndex];
                audioSource.Play();
                yield return new WaitForSeconds(footstepDelay);
            }
            else {
                yield break;
            }
        }

        isFootstepCoroutineRunning = false;
    }

    // Detect surface
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Floor")) {
            currentFootstepSounds = footstepSounds;
        }
    }
}
