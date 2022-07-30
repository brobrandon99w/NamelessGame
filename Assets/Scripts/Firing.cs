using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firing : MonoBehaviour
{
    [SerializeField] private Player player;
    public Transform firePoint;
    public Animator animator;
    public GameObject weaponPrefab;
    public GameObject weapon0; // Fist
    public GameObject weapon1; // Pistol

    private bool isReloading = false;

    // Weapon fire rates
    public float fistFireRate = 15f;
    public float pistolFireRate = 15f;

    // This handles the fire rate of weapons
    private float nextTimeToFire = 0f;

    // This handles the ammo sizes of weapons
    public int pistolMaxAmmo = 7;

    // This holds the current ammo of weapons
    private int currentPistolAmmo;

    // This holds the current ammo loaded in the current weapon
    private int loadedAmmo;

    // This handles the reload time of weapons
    public float pistolReloadTime = 2f;

    // Change weapon to fist if the ammo count of a weapon reaches zero
    private int changeWeapon;

    public float weaponForce;

    // Used to alternate between arm swings
    private bool leftSwing = false;

    // Update is called once per frame
    void Update()
    {
        changeWeapon = -1;

        currentPistolAmmo = player.getAmmo("Pistol");

        // If reloading, don't do anything
        if (isReloading)
            return;

        // Current weapon is a pistol that needs to be reloaded
        if (weaponPrefab == weapon1 && loadedAmmo <= 0 && currentPistolAmmo > 0)
        {
            // Weapon now empty
            if (changeWeapon == 1)
            {
                weaponPrefab = weapon0;
            }
            else
            {
                StartCoroutine(Reload());
                return;
            }
        }

        if (Input.GetButtonDown("Fire1") && Input.GetKey("mouse 1"))
        {
            // Current weapon is fist
            if (weaponPrefab == weapon0 && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 5f / fistFireRate;
                Shoot();
            }
            // Current weapon is pistol
            if (weaponPrefab == weapon1 && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 5f / pistolFireRate;
                changeWeapon = player.setAmmo("Pistol");
                // Weapon was not found
                if (changeWeapon == 0)
                {
                    weaponPrefab = weapon0;
                    animator.SetBool("PistolEquipped", false);
                }
                else
                    Shoot();
            }
            // Weapon now empty
            if (changeWeapon == 1)
                weaponPrefab = weapon0;
        }

        void Shoot()
        {

            // Update amount of pistol ammo loaded too
            if (weaponPrefab == weapon1)
            {
                loadedAmmo--;
            }
            // Alternate between different arm swings if the fist is the weapon
            if (weaponPrefab == weapon0)
            {
                if (leftSwing == true)
                {
                    animator.SetBool("leftPunch", true);
                    leftSwing = false;
                }
                else
                {
                    animator.SetBool("leftPunch", false);
                    leftSwing = true;
                }    
            }
            animator.SetTrigger("weaponFire");
            GameObject bullet = Instantiate(weaponPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * weaponForce, ForceMode2D.Impulse);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        // Weapon is pistol
        if (weaponPrefab == weapon1)
        {
            // Wait the pistol reload time
            animator.SetBool("Reloading", true);
            yield return new WaitForSeconds(pistolReloadTime);
            animator.SetBool("Reloading", false);

            if (currentPistolAmmo <= pistolMaxAmmo)
            {
                loadedAmmo = currentPistolAmmo;
            }
            else
                loadedAmmo = pistolMaxAmmo;
        }
        isReloading = false;
    }

    public bool isReloadingWeapon()
    {
        return isReloading;
    }

    public void SetWeapon(string weapon)
    {
        // Stop reloading if we are
        isReloading = false;
        animator.SetBool("Reloading", false);
        if (weapon == "Fist")
        {
            weaponPrefab = weapon0;
        }
        if (weapon == "Pistol")
        {
            weaponPrefab = weapon1;
            currentPistolAmmo = player.getAmmo("Pistol");
            if (currentPistolAmmo <= pistolMaxAmmo)
            {
                loadedAmmo = currentPistolAmmo;
            }
            else
                loadedAmmo = pistolMaxAmmo;
        }
        if (weapon == "FlashlightOn")
        {
            weaponPrefab = null;
        }    
    }

    // Get the current weapon
    public string GetCurrentWeapon()
    {
        string weapon = "Fist";
        if (weaponPrefab == weapon1)
        {
            weapon = "Pistol";
        }
        else if (weaponPrefab == null)
        {
            weapon = "FlashlightOn";
        }
        return weapon;
    }
}
