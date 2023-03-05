using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // The delay between each attack
    [SerializeField] private float attackDelay = 0.5f;

    // The layer containing the enemies
    [SerializeField] private LayerMask enemyLayer;

    private ItemHolder itemHolder;
    private Weapon currentWeapon;
    private PolygonCollider2D currentWeaponCollider;

    private void Start()
    {
        // Get the ItemHolder component attached to this GameObject
        itemHolder = GetComponent<ItemHolder>();

        // Get the current weapon and disable its collider
        currentWeapon = GetCurrentWeapon();
        if (currentWeapon != null)
        {
            currentWeaponCollider = currentWeapon.GetComponentInChildren<PolygonCollider2D>();
            if (currentWeaponCollider != null)
            {
                currentWeaponCollider.enabled = false;
            }
        }
    }

    private void Update()
    {
        // Get the current weapon and its collider
        currentWeapon = GetCurrentWeapon();
        if (currentWeapon != null)
        {
            currentWeaponCollider = currentWeapon.GetComponentInChildren<PolygonCollider2D>();
        }
    }

    private Weapon GetCurrentWeapon()
    {
        // Get the current item from the ItemHolder
        GameObject currentItem = itemHolder.GetCurrentItem();

        // If there is no current item or the item is not a weapon, return null
        if (currentItem == null)
        {
            return null;
        }

        Weapon weapon = currentItem.GetComponent<Weapon>();

        if (weapon == null)
        {
            return null;
        }

        // Return the current weapon
        return weapon;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int otherLayer = other.gameObject.layer;

        // If the current weapon collider is enabled and the other collider is an enemy
        if (currentWeaponCollider.enabled && (((1 << otherLayer) & enemyLayer.value) != 0))
        {
            // Get the Enemy component from the other GameObject and apply damage
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentWeapon.damage);
            }
        }   
    }

    public void EnableCollider()
    {
        // If the current weapon collider exists, enable it and set isAttacking to true
        if (currentWeaponCollider != null)
        {
            currentWeaponCollider.enabled = true;
        }
    }

    public void DisableCollider()
    {
        // If the current weapon collider exists, disable it and set isAttacking to false
        if (currentWeaponCollider != null)
        {
            currentWeaponCollider.enabled = false;
        }
    }
}