using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CannibalisticZombies 
{
    public class InventoryUI : MonoBehaviour
    {
        public Inventory inventory;
        public TextMeshProUGUI inventoryText;

        // number of characters per line
        public static int LINELENGTH = 40;

        // title of Inventory
        public string initialText = "Inventory\n";
        // Start is called before the first frame update
        void Start()
        {
            inventory.onSlotUpdated.AddListener(UpdateInventoryText);
            InitializeInventoryText();
        }


        //-//////////////////////////////////////////////////////////////////////
        // used in Start to create the initial Inventory text list
        // more intensive than update
        private void InitializeInventoryText()
        {
            string outputText = initialText;

            foreach (string key in inventory.inventory.Keys)
            {
                InventorySlot slot = inventory.inventory[key];
                outputText += WriteSlotEntry(slot);
            }
            inventoryText.text = outputText;
        }

        //-//////////////////////////////////////////////////////////////////////
        // used in other methods to write a new line in inventory
        // returns empty string if null/empty slot
        // displays item amount if more than 1
        private string WriteSlotEntry(InventorySlot slot) 
        {


            // amount of item in slot, to display
            int amount = slot.GetAmount();

            // amount of item in slot
            string amountText = "";

            // text to be centered left
            string firstHalf;

            // text to be centered right
            string secondHalf;

            // text used to space the left and right
            string spacing = " .";

            // string to store final line print
            string finalLine;


            // configure strings details
            if (slot.GetAmount() == 0) 
            {
                return "";
            }
            if (amount > 1) 
            {
                amountText = " x" + amount;
            }

            // write line components
            firstHalf = slot.GetPickupItemSO().name +
                    amountText;
            secondHalf = slot.GetWeight() +
                    "weight\n";

            // construct and space line
            finalLine = firstHalf;
            for (int i = 0; i < LINELENGTH - firstHalf.Length - secondHalf.Length; i += spacing.Length) 
            {
                finalLine += spacing;
            }
            finalLine += secondHalf;

            return finalLine;

        }

        //-//////////////////////////////////////////////////////////////////////
        // updates one line of inventory text
        // listens to onSlotUpdated Unity Event in inventory
        private void UpdateInventoryText(string itemName)
        {
            InventorySlot slot = inventory.inventory[itemName];
            string internalText = inventoryText.text;
            int index = internalText.IndexOf(itemName);
            if (index == -1) 
            {
                inventoryText.text += WriteSlotEntry(slot);
            }
            else 
            {
                inventoryText.text.Remove(index, LINELENGTH);
                inventoryText.text.Insert(index, WriteSlotEntry(slot));
            }
        }


    }
}
