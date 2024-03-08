using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    private List<InventorySlot> inventory;


    public Inventory() 
    {
        inventory = new List<InventorySlot>();
    }

    //-/////////////////////////////////////////////////////////////////////
    /// adds amount of new item to existing InventorySlot with said item 
    public void AddItem(Item newItem, int amount = 1)
    {
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
}
