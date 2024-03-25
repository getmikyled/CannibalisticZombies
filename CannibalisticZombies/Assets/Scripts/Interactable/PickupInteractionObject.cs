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
        }

        //-/////////////////////////////////////////////////////////////////////
        ///
        /// Get specific UiText for the pickup Interaction Object
        /// 
        public override string GetUiText()
        {
            return "Press E to Pickup";
        }
    }
}
