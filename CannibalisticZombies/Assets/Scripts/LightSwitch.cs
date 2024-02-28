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
            /// dont let the UI show up on screen once its interacted with
            keyBindUI.SetActive(false);
        }

        //-/////////////////////////////////////////////////////////////////////
        ///
        /// OnPointerEnter is a method in the UnityEngine UI
        /// When the user hovers over an object, the ingame UI for keybinds is set to active
        /// This method is overriden from the method in Interaction Object to have specific behavior regarding the UI that is presented
        /// https://docs.unity3d.com/2018.1/Documentation/ScriptReference/UI.Selectable.OnPointerEnter.html
        /// 
        public override void OnPointerEnter(PointerEventData eventData)
        {
            /// grab the textComponent from the keyBindUI, which is a child (GetComponentInChildren) of the canvas that seralizes the keyBind Field.
            Text keyBindText = keyBindUI.GetComponentInChildren<Text>();

            /// if the door is open 
            if(lightOn)
            {
                /// show a specific text to turn off light
                keyBindText.text = "Press E to turn off Light";
            }
            else
            {
                /// else just show UI to turn onLight
                keyBindText.text = "Press E to turn on Light";
            }
            /// show the UI on the screen
            keyBindUI.SetActive(true);
        }


    }
}
