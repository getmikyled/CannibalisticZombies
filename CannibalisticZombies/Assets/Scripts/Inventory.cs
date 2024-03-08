using System.Collections;
using System.Collections.Generic;
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
    /// returns true if successful <summary>
    /// returns false if unsuccesful (no change to inventory)
    public bool RemoveItem(Item item, int amount = 1) 
    {
        foreach (InventorySlot slot in inventory) 
        {
            if (slot.GetItem() == item) { return slot.RemoveItem(amount); }
        }
        return false;
    }
}


