using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Your audio clip
    public AudioClip attackSound;

    // Base stats
    [SerializeField]
    private int attackDamage = 10;
    [SerializeField]
    private float attackDelay = 1, attackDistanceThreshold = 1.7f;
    [SerializeField]
    private float attackForce = 5f, attackRange = 0.4f;

    // Object info
    private Animator animator;

    // Outside GameObject components
    [SerializeField]
    private Transform attackPoint;
    [SerializeField]
    private LayerMask heroLayer;
    private Transform playerTransform;
    private GameObject playerGameObject;

    // In-program Variables
    private float passedTime = 1;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerGameObject = GameObject.Find("Player");
        playerTransform = playerGameObject.transform;
    }
    void Update()
    {
        float distance = Vector2.Distance(playerTransform.position, transform.position);
        //Debug.Log(distance);
        if (distance <= attackDistanceThreshold && passedTime >= attackDelay)
        {
            passedTime = 0;
            Attack();
        }

        if (passedTime < attackDelay)
        {
            passedTime += Time.deltaTime;
        }
    }

    private void Attack()
    {
        if (attackSound != null)
        {
            SoundManager.Instance.PlaySound(attackSound);
        }
        else
        {
            Debug.LogError("Attack sound not set");
        }

        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, heroLayer);
        Collider2D player = null;

        foreach (Collider2D enemy in hitEnemies)
        {
            player = enemy;
            break;
        }

        if (player != null)
        {
            PlayerHealth heroHealthComponent = player.GetComponent<PlayerHealth>();

            if (heroHealthComponent != null)
            {
                Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
                heroHealthComponent.TakeDamage(attackDamage);
                heroHealthComponent.ApplyKnockback(knockbackDirection, attackForce);
            }
        }
    }
}
