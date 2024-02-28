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
    public class InteractionObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        ///  This is a variable that is used to grab the canvas associated with the keyBind UI
        [SerializeField] public GameObject keyBindUI;

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
        /// OnPointerEnter is a method in the UnityEngine UI
        /// When the user hovers over an object, the ingame UI for keybinds is set to active
        /// It is also virtual because child specific behavior is needed for specific objects
        /// https://docs.unity3d.com/2018.1/Documentation/ScriptReference/UI.Selectable.OnPointerEnter.html
        /// 
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            /// show the UI on screen
            keyBindUI.SetActive(true);
        }

        //-/////////////////////////////////////////////////////////////////////
        ///
        /// OnPointerExit is a method in the UnityEngine UI
        /// When the user is no longer hovering over an object, the ingame UI for keybinds is not set to active
        /// https://docs.unity3d.com/2018.3/Documentation/ScriptReference/UI.Selectable.OnPointerExit.html
        /// 
        public void OnPointerExit(PointerEventData eventData)
        {
            /// dont show UI on screen
            keyBindUI.SetActive(false);
        }
    }
}
