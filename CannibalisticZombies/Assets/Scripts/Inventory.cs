using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory 
{
    private List<InventorySlot> inventory;
    private int maxWeight;

    public Inventory() 
    {
        inventory = new List<InventorySlot>();
    }

    //-/////////////////////////////////////////////////////////////////////
    /// adds amount of new item to existing InventorySlot with said item
    /// if maxWeight allows
    public void AddItem(Item newItem, int amount = 1)
    {
        // return if adding goes overweight
        if (GetCurrentWeight()+newItem.weight * amount > maxWeight) { return; }
        foreach (InventorySlot slot in inventory) 
        {
            if (slot.GetItem() == newItem) 
            {
                slot.AddItem(amount);
                return;
            }
        }
        inventory.Add(new InventorySlot(newItem, amount));
    }

    //-/////////////////////////////////////////////////////////////////////
    /// returns weight of all inventory slots
    public int GetCurrentWeight() 
    {
        int weight = 0;
        foreach (InventorySlot slot in inventory) 
        {
            weight += slot.GetWeight();
        }
        return weight;
    }


    //-/////////////////////////////////////////////////////////////////////
    /// removes amount (default 1) of an item from the inventory 
    /// returns true if successful 
    /// returns false if unsuccesful (no change to inventory)
    /// removes empty Slots if created
    public bool RemoveItem(Item item, int amount = 1) 
    {
        foreach (InventorySlot slot in inventory)   
        {
            if (slot.GetItem() == item) // checks all slots for first instance with sought item
            {
                if (slot.RemoveItem(amount)) // attempts to remove item from slot
                {
                    if (slot.IsEmpty()) {inventory.Remove(slot);}
                    
                    return true;
                }
                // Debug.Log("Not enough " + item + " in inventory")
                return false;
            }
        }
        // Debug.Log("No " + item + " in inventory")
        return false;
    }
}


