using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannibalisticZombies.Restaurant
{
    ///-////////////////////////////////////////////////////////////
    ///
    /// Defines the possible player states
    ///
    public enum PlayerState
    {
        Default,
        Serving,
        Cooking
    }

    ///-////////////////////////////////////////////////////////////
    ///
    public class PlayerController : MonoBehaviour
    {
        public PlayerState playerState { get; private set; }

        private Rigidbody rb;

        [Header("Properties")]
        [SerializeField] private float movementSpeed = 1f;

        private float horizontalInput;
        private float verticalInput;
        private Vector3 moveDirection;

        public void SetPlayerState(PlayerState argState)
        {
            playerState = argState;
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            TryMovePlayer();
            LimitSpeed();
        }

        private void TryMovePlayer()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            moveDirection = (transform.forward * verticalInput) + (transform.right * horizontalInput);
            rb.AddForce(moveDirection.normalized * movementSpeed, ForceMode.Force);
        }

        private void LimitSpeed()
        {
            Vector3 currentVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (currentVelocity.magnitude > movementSpeed)
            {
                Vector3 limitedVelocity = currentVelocity.normalized * movementSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }
    }

}