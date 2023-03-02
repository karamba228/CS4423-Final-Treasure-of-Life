using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private Image healthBar;

    private void Start()
    {
        currentHealth = maxHealth;

        // Create the healthbar as a child of the enemy object
        GameObject healthBarObj = new GameObject("HealthBar");
        healthBarObj.transform.SetParent(transform);

        // Add an Image component to the healthbar object
        healthBar = healthBarObj.AddComponent<Image>();
        healthBar.color = Color.red;
        healthBar.type = Image.Type.Filled;
        healthBar.fillMethod = Image.FillMethod.Horizontal;
        healthBar.fillAmount = 1f;
        healthBar.rectTransform.sizeDelta = new Vector2(1.5f, 0.2f);
        healthBar.rectTransform.localPosition = new Vector2(0f, 1.2f);
    }

    private void Update()
    {
        // Update the healthbar's fill amount based on the current health percentage
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }

    public void TakeDamage(int damage)
    {
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