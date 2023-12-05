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
    private int arrowDamage = 15;

    [SerializeField] 
    float invulnerabilityDuration = 1.0f;

    [SerializeField]
    private float forceDamping = 0.2f;

    // Object info
    private Rigidbody2D rb;
    [SerializeField]
    private FloatingHealthBar healthBar;

    // Components Outside of GameObject
    private Animator animator;
    PlayerInventory inventory;

    // In-program Variables
    private bool isInvulnerable = false;
    void Start()
    {
        inventory = PlayerInventory.Instance;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        maxHealth = inventory.getHealthTotal();
        currentHealth = maxHealth;
    }
    void FixedUpdate()
    {
        // Reduce the force gradually using damping
        rb.velocity *= (1 - forceDamping);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow") && !isInvulnerable)
        {
            TakeDamage(arrowDamage);
            StartCoroutine(InvulnerabilityCooldown());
        }
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Hurt");
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
            
        if (currentHealth <= 0)
        {
            animator.SetBool("IsDead", true);
            animator.SetTrigger("Dies");
            Dies();
        }
    }

    IEnumerator InvulnerabilityCooldown()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (rb != null)
        {
            rb.AddForce(direction * force * 20, ForceMode2D.Impulse);
        }
    }

    public void Dies()
    {

        SoundManager.Instance.PlaySound(deathSound);


        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
        
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        playerMovement.enabled = false;

        
        LevelManager levelManager = FindObjectOfType<LevelManager>();

        if (inventory.getAlreadyWatchedAd()){
            levelManager.GameOver();
        }
        else {
            levelManager.PlayerDied();
        }

        this.enabled = false;
    }
}
