using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : MonoBehaviour
{
    [SerializeField] private LayerMask weaponLayer; // Layer mask for weapons that can hit the dummy
    [SerializeField] private float hitTorque = 50f; // Torque applied when the dummy is hit by a weapon

    // public SpringJoint2D springJoint; // Spring joint attached to the dummy
    public float torque = 2f; // Torque applied to the dummy

    private Rigidbody2D rb; // Reference to the rigidbody component of the dummy

    private void Start()
    {
        // Get the rigidbody component of the dummy
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that collided with the dummy is a weapon
        int otherLayer = other.gameObject.layer;
        if (((1 << otherLayer) & weaponLayer.value) != 0)
        {
            // Calculate the direction of the force applied by the weapon
            Vector2 forceDirection = other.transform.position - transform.position;

            // Apply torque in the opposite direction to make the dummy wobble away from the direction of the hit
            float torque = hitTorque * Mathf.Sign(Vector3.Cross(forceDirection, transform.up).z);
            rb.AddTorque(torque);

            // // Enable the spring joint to make the dummy bounce back
            // springJoint.enabled = true;
        }
    }
}