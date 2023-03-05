using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage; // Public variable to store weapon damage
    private PolygonCollider2D collider;

    private void Start()
    {
        // Find the weapon's collider and disable it when the weapon spawns in
        collider = GetComponentInChildren<PolygonCollider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }

    // Public method to enable the weapon's collider
    public void EnableCollider()
    {
        collider.enabled = true;
    }

    // Public method to disable the weapon's collider
    public void DisableCollider()
    {
        collider.enabled = false;
    }
}