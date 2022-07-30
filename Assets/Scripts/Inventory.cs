using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;
    public List<Item> itemList;
    private Action<Item> useItemAction;
    public int inventorySize = 5;
    public int inventoryCurrentSize = 0;
    
    
    public Inventory(Action<Item> useItemAction)
    {
        this.useItemAction = useItemAction;
        itemList = new List<Item>();

        AddItem(new Item { itemType = Item.ItemType.pistol, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.flashlightOn, amount = 1 });
    }
    
    public bool AddItem(Item item)
    {
        bool itemAlreadyInInventory = false;
        foreach (Item inventoryItem in itemList)
        {
            if (inventoryItem.itemType == item.itemType)
            {
                itemAlreadyInInventory = true;
                break;
            }
        }
        if (inventoryCurrentSize < inventorySize || (itemAlreadyInInventory && item.IsStackable()))
        {
            if (item.IsStackable())
            {
                //bool itemAlreadyInInventory = false;
                foreach (Item inventoryItem in itemList)
                {
                    if (inventoryItem.itemType == item.itemType)
                    {
                        inventoryItem.amount += item.amount;
                        //itemAlreadyInInventory = true;
                    }
                }
                if (!itemAlreadyInInventory)
                {
                    itemList.Add(item);
                    inventoryCurrentSize++;
                }
            }
            else
            {
                itemList.Add(item);
                inventoryCurrentSize++;
            }
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
            return false;
        }
        return true;
    }

    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }
            if (itemInInventory != null && itemInInventory.amount <= 0)
            {
                itemList.Remove(itemInInventory);
                inventoryCurrentSize--;
            }
        }
        else
        {
            itemList.Remove(item);
            inventoryCurrentSize--;
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    // It is assumed the item will be stackable. This is used to find the ammo count of weapons
    public int getAmount(Item item)
    {
        int itemAmount = 0;
        foreach (Item inventoryItem in itemList)
        {
            if (inventoryItem.itemType == item.itemType)
            {
                itemAmount = inventoryItem.amount;
            }
        }
        return itemAmount;
    }

    public void UseItem(Item item)
    {
        useItemAction(item);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
