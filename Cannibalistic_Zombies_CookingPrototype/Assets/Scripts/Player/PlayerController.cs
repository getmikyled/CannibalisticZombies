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

        public void SetPlayerState(PlayerState argState)
        {
            playerState = argState;
        }

        private void Update()
        {
            TryMovePlayer();
        }

        private void TryMovePlayer()
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            
            if (horizontalInput != 0f)
            {
                transform.forward = new Vector3(horizontalInput, 0, 0);
            }
            else
            {
                transform.forward = new Vector3(0, 0, -1);
            }
        }
    }

}