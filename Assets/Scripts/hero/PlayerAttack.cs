using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    // Your audio clip
    public AudioClip punchSound;
    public AudioClip kickSound;
    public AudioClip swordSound;

    // Base stats
    [SerializeField]
    private float punchCooldown, punchRange, punchForce;
    private int punchDamage;

    [SerializeField]
    public float kickCooldown, kickRange, kickForce;
    private int kickDamage;

    [SerializeField]
    public float swordCooldown, swordRange, swordForce;
    private int swordDamage;

    // Object info
    private Animator animator;
    [SerializeField]
    private Transform attackPoint;
    [SerializeField]
    private LayerMask enemyLayers;
    [SerializeField]
    private GameObject SlashAnimation;
    [SerializeField]
    private Animator SlashAnimator;
    [SerializeField]
    private InputActionReference punchButton, kickButton, slashButton;

    // Outside components
    PlayerInventory inventory;

    // In-program Variables
    private float lastPunchTime, lastKickTime, lastSwordTime;


    void Start()
    {
        inventory = PlayerInventory.Instance;
        punchDamage = inventory.getPunchDamage();
        kickDamage = inventory.getKickDamage();
        swordDamage = inventory.getSwordDamage();
        lastPunchTime = Time.time;
        lastKickTime = Time.time;
        lastSwordTime = Time.time;
        animator = GetComponent<Animator>();
        SlashAnimation.SetActive(false);
    }

    private void OnEnable()
    {
        punchButton.action.Enable();
        punchButton.action.performed += ctx => PunchButtonPressed();
        kickButton.action.Enable();
        kickButton.action.performed += ctx => KickButtonPressed();
        slashButton.action.Enable();
        slashButton.action.performed += ctx => SlashButtonPressed();
    }

    private void OnDisable()
    {
        punchButton.action.Disable();
        punchButton.action.performed -= ctx => PunchButtonPressed();
        kickButton.action.Disable();
        kickButton.action.performed -= ctx => KickButtonPressed();
        slashButton.action.Disable();
        slashButton.action.performed -= ctx => SlashButtonPressed();
    }

    private void PunchButtonPressed()
    {
        if ((Time.time - lastPunchTime) > punchCooldown)
        {
            Punch();
        }
    }

    private void KickButtonPressed()
    {
        if ((Time.time - lastKickTime > kickCooldown) && kickDamage > 0)
        {
            Kick();
        }
    }

    private void SlashButtonPressed()
    {
        if ((Time.time - lastSwordTime > swordCooldown) && swordDamage > 0)
        {
            Sword();
        }
    }

    private void Punch()
    {

        SoundManager.Instance.PlaySound(punchSound);

        lastPunchTime = Time.time;
        animator.SetTrigger("Punch");
        SlashAnimation.SetActive(true);
        SlashAnimator.SetTrigger("Punch");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, punchRange, enemyLayers);
        Collider2D firstEnemy = null;

        foreach (Collider2D enemy in hitEnemies)
        {
            firstEnemy = enemy;
            break;
        }

        if (firstEnemy != null)
        {
            EnemyHealth enemyHealthComponent = firstEnemy.GetComponent<EnemyHealth>();
            if (enemyHealthComponent != null)
            {
                Vector2 knockbackDirection = (firstEnemy.transform.position - transform.position).normalized;
                enemyHealthComponent.TakeDamage(punchDamage);
                enemyHealthComponent.ApplyKnockback(knockbackDirection, punchForce);
            }
        }
    }

    private void Kick()
    {

        SoundManager.Instance.PlaySound(kickSound);

        lastKickTime = Time.time;
        animator.SetTrigger("Kick");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, kickRange, enemyLayers);
        Collider2D firstEnemy = null;

        foreach (Collider2D enemy in hitEnemies)
        {
            firstEnemy = enemy;
            break;
        }

        if (firstEnemy != null)
        {
            EnemyHealth enemyHealthComponent = firstEnemy.GetComponent<EnemyHealth>();
            if (enemyHealthComponent != null)
            {
                Vector2 knockbackDirection = (firstEnemy.transform.position - transform.position).normalized;
                enemyHealthComponent.TakeDamage(kickDamage);
                enemyHealthComponent.ApplyKnockback(knockbackDirection, kickForce);
            }
        }
    }

    private void Sword()
    {

        SoundManager.Instance.PlaySound(swordSound);


        lastSwordTime = Time.time;
        animator.SetTrigger("Sword");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, swordRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealthComponent = enemy.GetComponent<EnemyHealth>();
            Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
            enemyHealthComponent.TakeDamage(swordDamage);
            enemyHealthComponent.ApplyKnockback(knockbackDirection, swordForce);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, punchRange);
    }
}
