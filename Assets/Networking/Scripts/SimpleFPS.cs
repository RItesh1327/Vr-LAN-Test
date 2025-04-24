using Fusion;
using System;
using TMPro;
using UnityEngine;

public class SimpleFPS : NetworkBehaviour
{
    [SerializeField] float m_MoveSpeed = 5f;
    [SerializeField] float m_RotationSensitivity = 2f;
    [SerializeField] Transform m_RotationTransform;
    [SerializeField] float m_MaxX = 90f;
    [SerializeField] float m_MinX = -90f;
    [SerializeField] float m_Gravity = 9.81f;
    [SerializeField] TextMeshPro m_Display;
    Vector2 move;
    Vector2 lookDelta;
    float xRotation;
    NetworkCharacterController characterController;
    Vector3 velocity;

    public override void Spawned()
    {
        characterController = GetComponent<NetworkCharacterController>();
        characterController.gravity = Physics.gravity.y * 2.0f;

        
    }
    private void Start()
    {
        //if(!HasStateAuthority)
        //{
        //    GetComponentInChildren<Camera>().enabled = false;
        //}

        if (HasInputAuthority)
        {
            FindAnyObjectByType<CameraFollowTarget>().SetFollowTargetTransform(m_RotationTransform);
        }
        m_Display.text = HasInputAuthority ? "Local player" : "Networked player";
    }
    //void Update()
    //{
    //    HandleMovement();
    //    HandleRotation();
    //}

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority)
            return;
        if(GetInput(out NetInput input))
        {
            move = input.MovementDirection;
            lookDelta = input.RotationDelta;
        }
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        Vector3 movementDirection = (transform.forward * move.y) + (transform.right * move.x);
        if (movementDirection.magnitude > 1) movementDirection.Normalize(); // Prevent diagonal speed boost


        characterController.MoveDefault(movementDirection);
    }

    private void HandleRotation()
    {
        // Rotate player body (Y-axis)
        transform.Rotate(Vector3.up * lookDelta.x * m_RotationSensitivity * Time.deltaTime);

        // Rotate camera up/down (X-axis) with clamping
        xRotation -= lookDelta.y * m_RotationSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, m_MinX, m_MaxX);
        m_RotationTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
