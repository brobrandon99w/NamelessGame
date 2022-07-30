using System;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;
    public Camera cam;

    [SerializeField] private UserInterfaceInventory uiInventory;
    [SerializeField] private Firing currentWeapon;
    public Animator animator;

    public Vector2 moveDirection;
    private Vector2 mousePos;
    private bool rmbHeld;
    private Inventory inventory;

    // Update is called once per frame
    void Update()
    {
        // Update animation to playerUnarmed if player is moving
        animator.SetFloat("Speed", Math.Abs(moveDirection.x) + Math.Abs(moveDirection.y));

        // If RMB is held, movement speed is slowed
        if (Input.GetKey("mouse 1") && !IsMouseOverUI())
        {
            rmbHeld = true;
            animator.SetBool("weaponAiming", true);
        }
        else
        {
            rmbHeld = false;
            animator.SetBool("weaponAiming", false);
        }

        // Processing Inputs
        ProcessInputs();
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void Start()
    {
        inventory = new Inventory(UseItem);
        uiInventory.SetPlayer(this);
        uiInventory.SetPlayer(this);
        uiInventory.SetInventory(inventory);
        //ItemWorld.SpawnItemWorld(new Vector3(1, 1), new Item { itemType = Item.ItemType.flashlightOn, amount = 1 });
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            bool full;
            // Touching item
            full = inventory.AddItem(itemWorld.GetItem());
            if (!full)
                itemWorld.DestroySelf();
        }
    }

    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.healthDrink:
                // Do something
                inventory.RemoveItem(new Item { itemType = Item.ItemType.healthDrink, amount = 1 });
                break;
            case Item.ItemType.pistol:
                // Unequip if the pistol is already equipped
                if (currentWeapon.GetCurrentWeapon() == "Pistol")
                {
                    currentWeapon.SetWeapon("Fist");
                    animator.SetBool("PistolEquipped", false);
                }
                else
                {
                    // Unequip flashlightOn if it is equipped
                    if (currentWeapon.GetCurrentWeapon() == "FlashlightOn")
                    {
                        animator.SetBool("flashlightEquipped", false);
                    }
                    currentWeapon.SetWeapon("Pistol");
                    animator.SetBool("PistolEquipped", true);
                }
                break;
            case Item.ItemType.flashlightOn:
                // Unequip if the flashlightOn is already equipped
                if (currentWeapon.GetCurrentWeapon() == "FlashlightOn")
                {
                    currentWeapon.SetWeapon("Fist");
                    animator.SetBool("flashlightEquipped", false);
                }
                else
                {
                    // Unequip pistol if it is equipped
                    if (currentWeapon.GetCurrentWeapon() == "Pistol")
                    {
                        animator.SetBool("PistolEquipped", false);
                    }
                    currentWeapon.SetWeapon("FlashlightOn");
                    animator.SetBool("flashlightEquipped", true);
                }
                break;
            case Item.ItemType.flashlightOff:
                // Do something
                break;
        }
    }

    // Returns true if ammo becomes zero or is zero, and false otherwise
    public int setAmmo(string weapon)
    {
        int ammoFound = 0;
        if (weapon == "Pistol")
        {
            ammoFound = inventory.getAmount(new Item { itemType = Item.ItemType.pistolAmmo, amount = 1 });
            if (ammoFound > 1)
                inventory.RemoveItem(new Item { itemType = Item.ItemType.pistolAmmo, amount = 1 });
            else if (ammoFound == 1)
            {
                inventory.RemoveItem(new Item { itemType = Item.ItemType.pistolAmmo, amount = 1 });
                animator.SetBool("PistolEquipped", false);
            }
        }
        return ammoFound;
    }

    // Returns the ammo count of the current specified weapon
    public int getAmmo(string weapon)
    {
        int ammoFound = 0;
        if (weapon == "Pistol")
        {
            ammoFound = inventory.getAmount(new Item { itemType = Item.ItemType.pistolAmmo, amount = 1 });
        }
        return ammoFound;
    }

    void FixedUpdate()
    {
        // Physics Calculations
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void Move()
    {
        // Speed is slowed by half if the player is holding rmb or reloading
        if (rmbHeld || currentWeapon.isReloadingWeapon())
        {
            animator.speed = .5F;
            rb.velocity = new Vector2(moveDirection.x * (moveSpeed - 3), moveDirection.y * (moveSpeed - 3));
        }
        else
        {
            animator.speed = 1F;
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        }

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
}
