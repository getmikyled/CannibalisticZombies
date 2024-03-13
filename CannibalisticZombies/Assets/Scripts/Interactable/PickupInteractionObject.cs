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
    /// This PickupInteractionObject helps interactions regarding objects that can be picked up
    /// The player will interact based on an input, and when hovering specific text will show up on screen
    /// This class is a child of the Interaction Object Class
    /// 
    public class PickupInteractionObject : InteractionObject
    {
        //-/////////////////////////////////////////////////////////////////////
        ///
        /// OnInteract is called in Player Interaction Controller, where the keybind is pressed
        /// This method is overrides the method in InteractionObject to have specific behavior concerning pickupobjects
        /// For now, the game object is not set active, and the keybindUI is also set to not active
        ///
        public override void OnInteract()
        {
            /// remove game Object from screen
            gameObject.SetActive(false);
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
            Debug.Log("Interacted");
            /// set what text should show up
            base.SetTextOfKeyBind("Press E to Pickup");
        
           /// set the UI to true and make it show up on screen
            return base.OnPointerEnter();
        }
    }
}
