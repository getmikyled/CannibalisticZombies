using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


public class Item // Placeholder until Item class implemented
{
    public int weight;
} 
public class InventorySlot
{

    private Item item; 
    private int amount; // Amount of the item stored

    //-/////////////////////////////////////////////////////////////////////
    /// Constructor
    public InventorySlot(Item newItem, int startAmount = 1)
    {
        item = newItem;
        amount = startAmount;
    }

    //-/////////////////////////////////////////////////////////////////////
    /// return item
    public Item GetItem()
    {
        return item;
    }

    //-/////////////////////////////////////////////////////////////////////
    /// return amount
    public int GetAmount() 
    { 
        return amount;
    }


    //-/////////////////////////////////////////////////////////////////////
    /// adds amount of item to the slot, default add 1
    public void AddItem(int addedAmount = 1)
    {
        amount += addedAmount;
    }

    //-/////////////////////////////////////////////////////////////////////
    /// removes amount of item, default remove 1
    /// returns true if successfully removed removeAmount 
    /// returns false if not enough available or input is nonpositive int <summary>
    /// resets slot if no items stored
    public bool RemoveItem(int removedAmount = 1)
    {
        if (removedAmount < 0)// 
        { 
            //Debug.Log("Invalid item amount request: " + removedAmount);
            return false;
        }
        if (amount <=removedAmount) 
        {
            amount -= removedAmount;
            if (amount == 0) { Reset(); }
            return true;
        }
        //Debug.Log("Less than " + removedAmount + item + " available (" + amount + ")");
        return false;
    }

    //-/////////////////////////////////////////////////////////////////////
    /// Returns weight of slot based on item weight
    public int GetWeight()
    {
        return amount * item.weight;
    }

    //-/////////////////////////////////////////////////////////////////////
    /// Resets slot to default values
    public void Reset() 
    {
        item = null;
        amount = 0;
    }

    //-/////////////////////////////////////////////////////////////////////
    /// returns true if no items in slot
    public bool IsEmpty() 
    {
        return amount == 0;
    }
}
