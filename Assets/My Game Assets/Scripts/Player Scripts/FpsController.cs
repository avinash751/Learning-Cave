using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsController : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] Transform cameraPositionHolder;
    [SerializeField] Rigidbody rb;

    [Header("Camera Rotation Settings")]
    float mouseAxisX;
    float mouseAxisY;
    [Range(0, 500)][SerializeField] float cameraSensetivity;

    [Header("Movemnt Settings")]
    float inputMoveX;
    float inputMoveZ;
    [Range(0, 50)][SerializeField] public float moveMaxSpeed;
    [Range(0, 100)][SerializeField] public float moveSpeed;
    [Range(0, 20)][SerializeField] float slowDownMultiplier;

    void Start()
    {
        InitilizeRefrences();
        InitilizePlayerStartRotation();
    }

    void Update()
    {
        RotateCameraAndCameraHolderBasedOnMouseInput();
        Move();   
    }

    void InitilizePlayerStartRotation()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouseAxisX = 0;
        mouseAxisY = 0;
        cameraPositionHolder.rotation = Quaternion.identity;
    }

    void InitilizeRefrences()
    {
        rb = rb ?? GetComponent<Rigidbody>();
        cameraPositionHolder = cameraPositionHolder ?? transform.parent;
    }

    void RotateCameraAndCameraHolderBasedOnMouseInput()
    {
        UpdateMouseInputForRotationAndMovement();
        UpdateCameraHolderRotationBasedOnMouseXInput();
        UpdateCameraRotationBasedOnMouseYInput();
    }

    void UpdateMouseInputForRotationAndMovement()
    {
        mouseAxisX += Input.GetAxis("Mouse X") * cameraSensetivity * Time.deltaTime;
        mouseAxisY -= Input.GetAxis("Mouse Y") * cameraSensetivity * Time.deltaTime;

        inputMoveX = Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        inputMoveZ = Input.GetAxisRaw("Vertical") * Time.deltaTime; ;
    }

    void UpdateCameraHolderRotationBasedOnMouseXInput() 
    {
        cameraPositionHolder.localRotation = Quaternion.Euler(mouseAxisX * Vector3.up); 
    }

    void UpdateCameraRotationBasedOnMouseYInput()
    {
        mouseAxisY = Mathf.Clamp(mouseAxisY, -90, 90);
        Camera.main.transform.localRotation = Quaternion.AngleAxis(mouseAxisY, Vector3.right);

    }
    void Move()
    {
        var normalisedDirection = GetDirectionBasedOnInputForMoving();
        rb.velocity += (cameraPositionHolder.right * normalisedDirection.x + cameraPositionHolder.forward * normalisedDirection.z) * moveSpeed;

        TruncateVelocity(rb.velocity.magnitude > moveMaxSpeed ? moveMaxSpeed : rb.velocity.magnitude);
        SlowDown();
    }
    Vector3 GetDirectionBasedOnInputForMoving()
    {
        Vector3 Direction = new Vector3(inputMoveX, rb.velocity.y, inputMoveZ);
        Direction.Normalize();
        return Direction;
    }
    void TruncateVelocity(float maxSpeed)
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }
    void SlowDown()
    {
        if(inputMoveX<=0 && inputMoveZ <= 0)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * slowDownMultiplier);
        } 
    }

}
