using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [Header("Input Actions")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Movement")]
    public float walkSpeed = 15f;
    private Vector2 moveInput;
    private Rigidbody rb;

    [Header("Camera")]
    [SerializeField] private Transform cameraHolder; // Asigna en el inspector
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;

    private Vector2 lookInput;
    private float verticalRotation = 0f;

    // InputActions
    private InputAction moveAction;
    private InputAction lookAction;

    private void Awake() {
        walkSpeed = 15f;
        rb = GetComponent<Rigidbody>();

        var actionMap = playerControls.FindActionMap("Player");

        moveAction = actionMap.FindAction("Move");
        lookAction = actionMap.FindAction("Look");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
    }

    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleLook();
    }

    private void HandleMovement()
    {
        // Convertimos el input 2D a 3D (eje XZ)
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);

        // Convertimos a movimiento relativo a la dirección del jugador
        Vector3 moveVelocity = transform.TransformDirection(moveDirection) * walkSpeed;

        // Mantener la velocidad vertical del Rigidbody (gravedad)
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
    }

    private void HandleLook()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        // Rotar el jugador en Y (horizontal)
        transform.Rotate(Vector3.up * mouseX);

        // Rotar la cámara en X (vertical)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        cameraHolder.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

}
