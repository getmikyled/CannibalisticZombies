using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannibalisticZombies
{
    public class PlayerCharacterController : MonoBehaviour
    {
        [SerializeField] float moveSpeed;
        [SerializeField] float groundDrag;

        [SerializeField] Transform orientation;

        private float horizontalInput;
        private float verticalInput;

        [SerializeField] float pHeight;
        private bool grounded;
        [SerializeField] LayerMask ground;

        private Vector3 moveDir;

        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        private void Update()
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, pHeight * 0.5f + 0.2f, ground);
            PInput();
            if (grounded) rb.drag = groundDrag;
            else rb.drag = 0;
        }

        private void FixedUpdate()
        {
            Move();
            Debug.Log(moveSpeed);
            Debug.Log(grounded);
        }
        void PInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }

        void Move()
        {
            moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        private void maxSpeed()
        {
            Vector3 fVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (fVel.magnitude > moveSpeed)
            {
                Vector3 mVel = fVel.normalized * moveSpeed;
                rb.velocity = new Vector3(mVel.x, rb.velocity.y, mVel.z);

            }
        }
    }
}
