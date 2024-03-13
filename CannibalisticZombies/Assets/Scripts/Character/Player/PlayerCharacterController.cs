using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannibalisticZombies
{
    ///-/////////////////////////////////////////////////////////////////////
    ///
    /// Player Movement Code
    /// 
    public class PlayerCharacterController : MonoBehaviour
    {
        private float moveSpeed;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float sprintStamina;
        [SerializeField] private float sprintDelay;
        [SerializeField] private float crouchSpeed;
        [SerializeField] private float groundDrag;

        [SerializeField] private Transform orientation;

        private float horizontalInput;
        private float verticalInput;
        private Vector3 moveDir;
        private KeyCode sprintKey = KeyCode.LeftShift;
        private KeyCode crouchKey = KeyCode.LeftControl;

        [SerializeField] private float pHeight;
        [SerializeField] private LayerMask ground;
        private float nYscale;
        private float cYscale;

        private bool grounded;
        private bool crun;
        private float rStart;
        private float rStop;

        private Rigidbody rb;

        private MovementState state;

        public enum MovementState{
            walking,
            sprinting,
            crouching
        }

        private void Start()
        {
            rStart = Time.time;

            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            crun = true;
            nYscale = transform.localScale.y;
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
        }
        void PInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(crouchKey))
            {
                transform.localScale = new Vector3(transform.localScale.x, cYscale, transform.localScale.z);
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            }

            if (Input.GetKeyDown(sprintKey))
            {
                rStart = Time.time;
            }

            if (Input.GetKeyUp(crouchKey))
            {
                transform.localScale = new Vector3(transform.localScale.x, nYscale, transform.localScale.z);
            }
            if (state == MovementState.sprinting && rStart + sprintStamina < Time.time)
            {
                crun = false;
                rStop = Time.time;
                state = MovementState.walking;
                moveSpeed = walkSpeed;
                //                Debug.Log("Run Stop");
          //      Debug.Log(rStart);
            }
            if ((!crun && rStop + sprintDelay < Time.time))
            {
                crun = true;
                //               Debug.Log("CAN RUN");
            }

            StateHandler();
         //   Debug.Log(Time.time);
       //                Debug.Log(moveSpeed);


        }

        void Move()
        {
            moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        private void StateHandler()
        {
            if (Input.GetKey(crouchKey))
            {
                state = MovementState.crouching;
                moveSpeed = crouchSpeed;

            }
            else if (Input.GetKey(sprintKey) && crun)
            {
                state = MovementState.sprinting;
                moveSpeed = sprintSpeed;

            }
            else
            {
                state = MovementState.walking;
                moveSpeed = walkSpeed;
            }
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
