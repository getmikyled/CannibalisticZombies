using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace CannibalisticZombies
{
  
    //-/////////////////////////////////////////////////////////////////////
    /// 
    /// This Door class helps interactions regarding objects that are doors
    /// The player will interact based on an input, and when hovering specific text will show up on screen
    /// This class is a child of the Interaction Object Class
    ///
    public class Door : InteractionObject
    {

        /// this is a private boolean that checks if the door is either closed or open, we assume its closed at the start  
        private bool doorOpen = false;

        /// pivot of the door that will be rotated in the script
        [SerializeField] 
        private GameObject pivot;

        /// the rotation angle of the door
        private float rotateAngle = 90f; 
    
        //-/////////////////////////////////////////////////////////////////////
        ///
        /// OnInteract is called in Player Interaction Controller, where the keybind is pressed
        /// This method is overrides the method in InteractionObject to have specific behavior concerning doors
        /// The boolean doorOpen is changed depending on the previous state, and the keyBindUI is set to not be active
        ///
        public override void OnInteract()
        {

            /// check if the door is Open
            if (doorOpen)
            {
                /// if the door is open make it closed
                CloseDoor();
            }
            else
            {
                /// else just make it open 
                OpenDoor();
            }
            /// dont let the UI show up on screen once its interacted with
            base.SetKeyBindUI(false);
            
        }

        //-/////////////////////////////////////////////////////////////////////
        ///
        /// OnPointerEnter is a method in the UnityEngine UI
        /// When the user hovers over an object, the ingame UI for keybinds is set to active
        /// This method is overriden from the method in Interaction Object to have specific behavior regarding the UI that is presented
        /// https://docs.unity3d.com/2018.1/Documentation/ScriptReference/UI.Selectable.OnPointerEnter.html
        /// 
        public override GameObject OnPointerEnter()
        {
            /// if the door is open 
            if(doorOpen)
            {
                /// show a specific text to close door
                base.SetTextOfKeyBind("Press E to Close Door");
            }
            else
            {
                /// else just show UI to open the door
                base.SetTextOfKeyBind("Press E to Open Door");
            }
            /// set the UI to true and make it show up on screen
            return base.OnPointerEnter();
        }


        //-/////////////////////////////////////////////////////////////////////
        ///
        /// Check if the player is facing the door, or is behind the door.
        /// This will be used to determine which way the door should swing.
        /// https://stackoverflow.com/questions/46662998/rotate-a-door-in-both-directions-in-unity
        /// 
        private bool InFrontOfDoor()
        {

            /// get the position of the player
            Transform playerTransform = PlayerInteractionController.instance.GetPlayerTransform();
            /// get the direction of the player
            Vector3 playerTransformDir = playerTransform.position - transform.position;
            /// get direction of door
            Vector3 doorTransformDir = transform.forward;
            /// check if in front using dotProduct
            bool front = Vector3.Dot(doorTransformDir, playerTransformDir) > 0;

            /// if front is true, that means player is in front of door
            return front;
        }

        //-/////////////////////////////////////////////////////////////////////
        ///
        /// Open the door depending on which way the user is facing.
        /// The door opens away from the player
        /// Dependency: calls RotateDoor coroutine 
        /// 
        private void OpenDoor()
        {
            if(InFrontOfDoor())
            {
                rotateAngle = 90f;
            }
            else
            {
                rotateAngle = -90f;
            }

            /// combine angle of the door and the angle we want to rotate (openRotation)
            Quaternion openRotation = Quaternion.Euler(0f, rotateAngle, 0f) * pivot.transform.localRotation;
            doorOpen = true;
            /// use coroutine 
            StartCoroutine(RotateDoor(openRotation));
            
        }

        //-/////////////////////////////////////////////////////////////////////
        ///
        /// Closes door (back to original starting position)
        /// Dependency: calls RotateDoor coroutine 
        /// 
        private void CloseDoor()
        {
            /// oppisote of the rotateAngle used to open the door
            Quaternion closeRotation = Quaternion.Euler(0f, -rotateAngle, 0f) * pivot.transform.localRotation;
            doorOpen = false;
            StartCoroutine(RotateDoor(closeRotation));
            
        }

        //-/////////////////////////////////////////////////////////////////////
        ///
        /// This coroutine slowly rotates the door (either opening or closing the door)
        /// The direction it rotates is determined by the parameter dirRotation.
        /// CoRoutine : https://docs.unity3d.com/Manual/Coroutines.html
        /// Slerp: https://docs.unity3d.com/ScriptReference/Quaternion.Slerp.html
        /// 
        /// 
        private IEnumerator RotateDoor(Quaternion dirRotation)
        {
            /// take one second to rotate the door 
            float duration = 1f;
            /// counter for while loop
            float counter = 0f;

            Quaternion startingRotation = pivot.transform.localRotation;

            while(counter < duration)
            {
                /// slowly rotate based on counter
                pivot.transform.localRotation = Quaternion.Slerp(startingRotation, dirRotation, counter / duration);
                /// increment
                counter += Time.deltaTime;
                yield return null;
            }

            /// make sure the door ends up in expected rotation 
            pivot.transform.localRotation = dirRotation;
 
        }

    }
}
