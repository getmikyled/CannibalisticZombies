using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannibalisticZombies
{

    //-/////////////////////////////////////////////////////////////////////
    /// Enum is used here to create a fixed set of values
    /// The three different types of items that can be picked up in the game
    /// https://stackoverflow.com/questions/49182152/updating-an-enum-for-a-scriptableobject-based-on-another-enum-selection
    ///
    public enum ItemType
    {
        Consumable, 
        Throwable, 
        Weapon
    }

    //-/////////////////////////////////////////////////////////////////////
    /// Create scriptable objects: which are data containers for our pickup items
    /// 
    public class PickupItemSO : ScriptableObject
    {
        /// A refrence to the enum above, so that each pickable item is restricted to the three options above
        public ItemType ItemType; 
        /// name of the item
        public string ItemName;
        /// the description for the item
        public string ItemDescription;
        /// max number of copies we can hold of the item
        public int maxCount;
        /// duration of the cooldown if it has one
        public float useCoolDownDuration;
    }

}
