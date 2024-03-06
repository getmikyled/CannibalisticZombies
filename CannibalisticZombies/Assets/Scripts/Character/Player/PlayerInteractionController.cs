using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace CannibalisticZombies
{

    //-/////////////////////////////////////////////////////////////////////
    ///
    /// Author : Meghana Indukuri (meghana.indukuri@sjsu.edu)
    /// 
    /// PlayerInteractionController is control interactions with interactable objects in game 
    /// The player will interact based on this input (E)
    /// 
    public class PlayerInteractionController : MonoBehaviour
    {
        /// this private variable takes in Camera that is used for the player view
        private Camera playerCamera;

        /// this is the raycaster which is used to decide the radius of interaction and when the user interacts w something
        private PhysicsRaycaster raycaster;

        //-/////////////////////////////////////////////////////////////////////
        ///
        /// Awake is called before the first frame update & intializes variables that dont rely on the script
        ///
        private void Awake()
        {
            /// setting the playerCamera to the main Camera in our set up
            playerCamera = Camera.main;
            
            /// intalize raycaster from the camera
            raycaster = playerCamera.GetComponent<PhysicsRaycaster>();
        }

        //-/////////////////////////////////////////////////////////////////////
        ///
        /// Update is called once per frame
        ///
        private void Update()
        {
            /// check when the E button is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                /// get the object from where the mouse is located
                PointerEventData eventData = new PointerEventData(EventSystem.current);
                /// make sure the location of that object is the mousePosition
                eventData.position = Input.mousePosition;


                /// create a list of all the raycast results found
                List<RaycastResult> results = new List<RaycastResult>();

                /// raycast is executed on scene and then  the evenData and then put into results
                raycaster.Raycast(eventData, results);

                /// iterate through each result (should always be one based on how the raycaster component is configured)
                foreach (RaycastResult result in results)
                {
                    /// the raycast only captures objects in the interactable layer, so each result will have the Ineraction Object script.
                    InteractionObject interactableObject = result.gameObject.GetComponent<InteractionObject>();
                    
                    /// we dont need this line but its good to check anyways (check if the object actually exists)
                    if (interactableObject != null)
                    {
                        /// call the OnInteract method when the player presses E
                        interactableObject.OnInteract(); 
                        /// make sure only the first object is interacted with, the configuration of raycaster already does this but its good to have extra precautions
                        break; 
                    }
                }
            }
        }
    }
}


