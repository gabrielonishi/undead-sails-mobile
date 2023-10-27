using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Your audio clip
    public AudioClip deathSound;

    // Base stats
    public int currentHealth;
    public int maxHealth;

    [SerializeField]
    private float knockbackDuration = 0.5f;

    // Object info
    private Rigidbody2D rb;
    [SerializeField]
    private FloatingHealthBar healthBar;

    // Components Outside of GameObject
    private Animator animator;
    PlayerInventory inventory;

    // In-program Variables
    void Start()
    {
        inventory = PlayerInventory.Instance;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        maxHealth = inventory.getHealthTotal();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Hurt");
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
            
        // Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            animator.SetBool("IsDead", true);
            animator.SetTrigger("Dies");
            Dies();
        }
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (rb != null)
        {
            rb.AddForce(direction * force, ForceMode2D.Impulse);
            StartCoroutine(ResetKnockback());
        }
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration);
        rb.velocity = Vector2.zero;
    }

    public void Dies()
    {
        // Play the death sound using the SoundManager
        if (deathSound != null)
        {
            SoundManager.Instance.PlaySound(deathSound);
        }
        else
        {
            Debug.LogError("Death sound not set");
        }

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        playerMovement.enabled = false;

        
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        levelManager.GameOver();

        Destroy(inventory);

        this.enabled = false;
    }
}
