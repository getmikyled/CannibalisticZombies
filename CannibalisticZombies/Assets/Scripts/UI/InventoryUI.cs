using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace CannibalisticZombies 
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;
        [SerializeField] private TextMeshProUGUI inventoryText;
        [SerializeField] private Image inventoryImage;
        [SerializeField] private TextMeshProUGUI weightText;
        [SerializeField] private Image weightImage;
        [SerializeField] private GameObject inventoryUI;

        // number of characters per line
        public static int LINELENGTH = 40;

        // title of Inventory
        [SerializeField] private string initialText = "Inventory\n";
        // Start is called before the first frame update
        void Start()
        {
            MakeTestInventory();
            inventory.onSlotUpdated.AddListener(UpdateInventoryText);
            InitializeInventoryText();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                
                inventoryText.enabled = !inventoryText.enabled;
                inventoryImage.visible = !inventoryImage.visible;
                weightText.enabled = !weightText.enabled;
                weightImage.visible = !weightImage.visible;
            }
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
            weightText.text = WriteWeight();
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
            string spacing = " -";

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
            firstHalf = slot.GetPickupItemSO().itemName +
                    amountText;
            secondHalf = " (" + slot.GetWeight() +
                    " weight)\n";

            // construct and space line
            finalLine = firstHalf;
            for (int i = firstHalf.Length; i < LINELENGTH - secondHalf.Length; i += spacing.Length) 
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
            weightText.text = WriteWeight();
        }

        private string WriteWeight() 
        {
            string WeightLine = "Weight: ";
            WeightLine += inventory.GetCurrentWeight() + "/" + inventory.GetMaxWeight();
            
                return  WeightLine;
        }

        //-//////////////////////////////////////////////////////////////////////
        // Creates an Inventory for testing
        // Used in Start
        private void MakeTestInventory() 
        {
            inventory = new Inventory();
            PickupItemSO gun = ScriptableObject.CreateInstance<PickupItemSO>();
            PickupItemSO flash = ScriptableObject.CreateInstance<PickupItemSO>();
            PickupItemSO ammo = ScriptableObject.CreateInstance<PickupItemSO>();

            gun.itemType = ItemType.Weapon;
            gun.itemName = "Gun";
            gun.weight = 1.5f;

            flash.itemType = ItemType.Throwable;
            flash.itemName = "flash";
            flash.weight = 1.0f;

            ammo.itemType = ItemType.Consumable;
            ammo.itemName = "ammo";
            ammo.weight = 0.1f;

            inventory.AddItem(gun);
            inventory.AddItem(flash);
            inventory.AddItem(ammo, 300);
        }

        

    }
}
