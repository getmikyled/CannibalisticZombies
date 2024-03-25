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
    /// This LightSwitch class helps interactions regarding objects that are light switches
    /// The player will interact based on an input, and when hovering specific text will show up on screen
    /// This class is a child of the Interaction Object Class
    ///
    public class LightSwitch : InteractionObject
    {
        /// this is a private boolean that checks if the light is either on or off, we assume its off at the start  
        private bool lightOn = false;
    
        //-/////////////////////////////////////////////////////////////////////
        ///
        /// OnInteract is called in Player Interaction Controller, where the keybind is pressed
        /// This method is overrides the method in InteractionObject to have specific behavior concerning lightswitches
        /// The boolean lightOn is changed depending on the previous state, and the keyBindUI is set to not be active
        ///
        public override void OnInteract()
        {
            /// check if the light is On
            if (lightOn)
            {
                /// then turn off the light
                lightOn = false;
            }
            else
            {
                /// else just make it off
                lightOn = true;
            }
        }

        //-/////////////////////////////////////////////////////////////////////
        ///
        /// Get specific UiText for the lightSwitch Object
        /// 
        public override string GetUiText()
        {

            /// if the door is open 
            if(lightOn)
            {
                /// show a specific text to turn off light
                return "Press E to turn off Light";
            }
            /// else just show UI to turn onLight
            return "Press E to turn on Light";
        }


    }
}
