using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Transform pfItemWorld;

    public Sprite pistolSprite;
    public Sprite pistolAmmoSprite;
    public Sprite healthDrinkSprite;
    public Sprite flashlightOnSprite;
    public Sprite flashlightOffSprite;
}
