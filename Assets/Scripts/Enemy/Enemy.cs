using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private Slider healthSlider;

    private void Start()
    {
        currentHealth = maxHealth;

        // Find the health slider in the scene
        healthSlider = GetComponentInChildren<Slider>();
        Debug.Log(healthSlider);
        healthSlider.value = 1f;
    }

    private void Update()
    {
        // Calculate the health percentage and set the slider value
        float healthPercentage = (float)currentHealth / maxHealth;
        healthSlider.value = healthPercentage;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Enemy Taking Damage");
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player_Attack"))
        {
            // Apply damage or other effects to the dummy
        }
    }

    private void Die()
    {
        // TODO: Handle enemy death
    }
}