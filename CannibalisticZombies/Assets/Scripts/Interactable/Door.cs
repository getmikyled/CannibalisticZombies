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


        ///  closedRotation that the door is placed
        private Quaternion closedRotation;

        /// when facing away from the door
        private Quaternion openRotationAway;

        /// when facing towards to the open
        private Quaternion openRotationTowards;

        /// the rotation angle of the door
        private float rotateAngle = 90f; 

        ///  the speed of the door closing/opening
        [SerializeField]
        private float doorSpeed = 50f;

        ///  stored coroutine to avoid multiple coroutines running at once
        private Coroutine storedCoroutine;


        public void Start()
        {
            closedRotation = Quaternion.Euler(0f, 0f, 0f) * pivot.transform.localRotation;
            openRotationAway =  Quaternion.Euler(0f, -90f, 0f) * pivot.transform.localRotation;
            openRotationTowards =  Quaternion.Euler(0f, 90f, 0f) * pivot.transform.localRotation;
        }
    
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
            
        }

        //-/////////////////////////////////////////////////////////////////////
        ///
        /// Get specific UiText for the door Object
        /// 
        public override string GetUiText()
        {
            /// if the door is open 
            if(doorOpen)
            {
                /// show a specific text to close door
                return "Press E to Close Door";
            }
            /// else just show UI to open the door
            return "Press E to Open Door";
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
            if(storedCoroutine!= null)
            {
                /// stop a coroutine if its currently happening
                StopCoroutine(storedCoroutine);
            }
            if(InFrontOfDoor())
            {
                storedCoroutine = StartCoroutine(RotateDoor(openRotationTowards));
            }
            else
            {
                storedCoroutine = StartCoroutine(RotateDoor(openRotationAway));
            }

            /// door is open
            doorOpen = true;
         
        }

        //-/////////////////////////////////////////////////////////////////////
        ///
        /// Closes door (back to original starting position)
        /// Dependency: calls RotateDoor coroutine 
        /// 
        private void CloseDoor()
        {
            /// oppisote of the rotateAngle used to open the door
            // Quaternion closeRotation = Quaternion.Euler(0f, -rotateAngle, 0f) * pivot.transform.localRotation;
            doorOpen = false;
            /// stop the coroutine that is currently running (avoid spamming bugs)
            StopCoroutine(storedCoroutine);
            /// start a new coroutine
            storedCoroutine =  StartCoroutine(RotateDoor(closedRotation));
            
        }

        //-/////////////////////////////////////////////////////////////////////
        ///
        /// This coroutine slowly rotates the door (either opening or closing the door)
        /// The direction it rotates is determined by the parameter dirRotation.
        /// CoRoutine : https://docs.unity3d.com/Manual/Coroutines.html
        /// Slerp: https://docs.unity3d.com/ScriptReference/Quaternion.Slerp.html
        /// 
        /// 
        private IEnumerator RotateDoor(Quaternion finalRotation)
        {
            /// the starting rotation
            Quaternion startingRotation = pivot.transform.localRotation;

            /// calculate the total length of the journey
            float totalLength = Quaternion.Angle(startingRotation, finalRotation);
            
            /// get the start time
            float startTime = Time.time;

            /// while the angle between the current location and finalrotation is not 0 -> keep rotating the door
            while (Quaternion.Angle(pivot.transform.localRotation, finalRotation) > 0.001f)
            {

                /// calculate the fraction of the total which will be used for the interpolation factor of slerp
                float fractionOfTotal  = ((Time.time - startTime) * doorSpeed) / totalLength;
                
                /// slerp the door to rotate it naturally
                pivot.transform.localRotation = Quaternion.Slerp(startingRotation, finalRotation, fractionOfTotal);

                yield return null;
            }

            /// make sure that it rotates to the final expected rotation.
            pivot.transform.localRotation = finalRotation;
        }

    }
}
