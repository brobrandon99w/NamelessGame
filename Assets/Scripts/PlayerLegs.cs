using System;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLegs : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Firing currentWeapon;

    public Transform target;
    private Vector2 moveDirection;
    public Animator animator;

    void Update()
    {
        // If RMB is held, speed of animation is slowed
        if ((Input.GetKey("mouse 1") && !IsMouseOverUI()) || currentWeapon.isReloadingWeapon())
        {
            animator.speed = .5F;
        }
        else
        {
            animator.speed = 1F;
        }    

            moveDirection = player.moveDirection;
        // Update animation to playerLegs if player is moving
        animator.SetFloat("Speed", Math.Abs(moveDirection.x) + Math.Abs(moveDirection.y));
        // Turn the legs to the direction the player is moving
        if (moveDirection != Vector2.zero)
        {
            transform.up = moveDirection;
        }
    }

    private void FixedUpdate()
    {
        Follow();
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    // Follow the current position of the player
    void Follow()
    {
        transform.position = target.position;
    }
}
