using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace CannibalisticZombies
{
  
    //-/////////////////////////////////////////////////////////////////////
    ///
    /// Author : Meghana Indukuri (meghana.indukuri@sjsu.edu)
    /// 
    /// This InteractionObject helps with UI and interaction features for each object in the game
    /// The player will interact based on this input (E)
    /// 
    public class InteractionObject : MonoBehaviour
    {

        //-/////////////////////////////////////////////////////////////////////
        ///
        /// OnInteract is called in Player Interaction Controller, where the keybind is pressed
        /// This method is virtual because this method is overriden by its children for child specific behavior
        ///
        public virtual void OnInteract()
        {
            /// print what was interacted with to the console
            Debug.Log("Player Interacted With:  " + gameObject.name);
        }

        //-/////////////////////////////////////////////////////////////////////
        ///
        /// get the specific UiText for the game object
        ///
        public virtual string GetUIText()
        {
            return "";
        }
        
        
    }
}
