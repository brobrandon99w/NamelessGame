using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{

    public enum ItemType
    {
        pistol,
        pistolAmmo,
        flashlightOn,
        flashlightOff,
        healthDrink,
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.pistol:
                return ItemAssets.Instance.pistolSprite;
            case ItemType.pistolAmmo:
                return ItemAssets.Instance.pistolAmmoSprite;
            case ItemType.healthDrink:
                return ItemAssets.Instance.healthDrinkSprite;
            case ItemType.flashlightOn:
                return ItemAssets.Instance.flashlightOnSprite;
            case ItemType.flashlightOff:
                return ItemAssets.Instance.flashlightOffSprite;
        }
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.pistolAmmo:
                return true;
            case ItemType.healthDrink:
                return true;
            case ItemType.flashlightOn:
                return false;
            case ItemType.flashlightOff:
                return false;
            case ItemType.pistol:
                return false;
        }
    }
}
