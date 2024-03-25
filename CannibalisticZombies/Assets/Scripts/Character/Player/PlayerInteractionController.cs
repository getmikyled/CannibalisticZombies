using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


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

        ///  This is a variable that is used to grab the canvas associated with the keyBind UI
        [SerializeField] 
        private GameObject keyBindUI;

        ///  This is a variable that is used to grab the text associated with the keyBind UI
        [SerializeField] 
        private Text keyBindText;

        // refrence to playerInteractionController
        public static PlayerInteractionController instance;

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

            /// Create ref to playerInteractionController on Awake
            if(instance != null && instance != this)
            {
                Destroy(instance);
            }
            else
            {
                instance = this;
            }

        }

        //-/////////////////////////////////////////////////////////////////////
        ///
        /// Get access to player's position using this getter method.
        ///
        public Transform GetPlayerTransform()
        {
            return transform;
        }

        //-/////////////////////////////////////////////////////////////////////
        ///
        /// Update is called once per frame
        ///
        private void Update()
        {
                /// get the object from where the mouse is located
                PointerEventData eventData = new PointerEventData(EventSystem.current);
                /// make sure the location of that object is the mousePosition
                eventData.position = Input.mousePosition;


                /// create a list of all the raycast results found
                List<RaycastResult> results = new List<RaycastResult>();

                /// raycast is executed on scene and then  the evenData and then put into results
                raycaster.Raycast(eventData, results);


                bool hovering = false;

                /// iterate through each result (should always be one based on how the raycaster component is configured)
                foreach (RaycastResult result in results)
                {
                    /// the raycast only captures objects in the interactable layer, so each result will have the Ineraction Object script.
                    InteractionObject interactableObject = result.gameObject.GetComponent<InteractionObject>();
                    if (interactableObject != null)
                    {
                        hovering = true;
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            /// call the OnInteract method when the player presses E
                            interactableObject.OnInteract(); 
                            /// set the ui to false
                            keyBindUI.SetActive(false);
                            /// make sure only the first object is interacted with, the configuration of raycaster already does this but its good to have extra precautions
                            break; 
                        }
                        else
                        {
                            /// get the Ui text set the Ui on screen
                            keyBindText.text = interactableObject.GetUiText();
                            keyBindUI.SetActive(true);
                        }
                    }
                }
                /// if not hovering then cancel
                if (keyBindUI != null && !hovering)
                {
                    keyBindUI.SetActive(false);
                }
        }
    }
}


