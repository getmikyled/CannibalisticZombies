using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Inventory: MonoBehaviour 
{
    public Dictionary<string, InventorySlot> inventory;
    private float maxWeight;
    private float currentWeight;
    public UnityEvent onWeightUpdated;

    public Inventory(float startWeight = 0f, float startMaxWeight = 100f) 
    {
        maxWeight = startMaxWeight;
        SetCurrentWeight(startWeight);
        inventory = new Dictionary<string, InventorySlot>();
        if (onWeightUpdated != null)
        {
            onWeightUpdated = new UnityEvent();
        }
        
    }

    //-/////////////////////////////////////////////////////////////////////
    /// adds amount of new item to existing InventorySlot with said item
    /// if maxWeight allows
    public bool AddItem(PickupItemSO newItem, int amount = 1)
    {
        // return false if adding goes overweight
        if (GetCurrentWeight() + newItem.weight * amount > maxWeight)  return false;
        // increases amount on existing item slot if possible
        if (inventory.ContainsKey(newItem.itemName)) 
        {
            string key = newItem.itemName;
            inventory[key].AddItem(amount);
        }
        // otherwise creates a new slot with item and amount
        else
        {
            inventory.Add(newItem.itemName, new InventorySlot(newItem, amount));

        }
        UpdateCurrentWeight(amount * newItem.weight);
        return true;
    }


    //-/////////////////////////////////////////////////////////////////////
    /// Removes amount of item from slot if possible <summary>
    /// if no or not enough items available, returns false
    /// otherwise returns true
    public bool RemoveItem(PickupItemSO item, int amount = 1) 
    {
        // attempt to remove item and amount from slot
        if (inventory.ContainsKey(item.itemName) && inventory[item.itemName].RemoveItem(amount))
        {
            UpdateCurrentWeight( - item.weight * amount);
            if (inventory[item.itemName].IsEmpty()) 
            {
                inventory.Remove(item.itemName);
            }
            return true;
        }
        return false;
    }

    //-/////////////////////////////////////////////////////////////////////
    /// returns currentWeight, updated with every inventory change
    public float GetCurrentWeight()
    {
        return currentWeight;
    }

    //-/////////////////////////////////////////////////////////////////////
    private void SetCurrentWeight(float newWeight) 
    {
        currentWeight = newWeight;
        onWeightUpdated.Invoke();
    }

    //-/////////////////////////////////////////////////////////////////////
    private void UpdateCurrentWeight(float weightDiff)
    {
        currentWeight += weightDiff;
        onWeightUpdated.Invoke();
    }

    //-/////////////////////////////////////////////////////////////////////
    public float GetMaxWeight() 
    { 
        return maxWeight; 
    }

    //-/////////////////////////////////////////////////////////////////////
    public void SetMaxWeight(float newMaxWeight) 
    { 
        maxWeight = newMaxWeight; 
    }
}



