using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    public float moveSpeed;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDir;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        PInput();
    }

    private void FixedUpdate()
    {
        Move();
    }
    void PInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void Move()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDir.normalized * moveSpeed * 10f,ForceMode.Force);
    }
}