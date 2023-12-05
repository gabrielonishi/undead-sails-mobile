    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    public class EnemyHealth : MonoBehaviour
    {
        // Your audio clip
        public AudioClip damageSound;
        public AudioClip deathSound;
        
        // Base stats
        private int currentHealth;
        [SerializeField]
        private int maxHealth;
        private int coinsDropped;

        [SerializeField]
        private float knockbackDuration = 0.5f;
        [SerializeField]
        private int fadeOutTime = 3;

        // Object info
        private Rigidbody2D rb;
        [SerializeField]
        private FloatingHealthBar healthBar;
        [SerializeField]
        private Canvas coinCanvas;
        private Animator animator;

        [SerializeField]
        private bool isDevil;

        // Components Outside of GameObject
        private LevelManager levelManager;
        [SerializeField]
        private TextMeshProUGUI coinsText;

        // In-program Variables
        void Start()
        {
            levelManager = FindObjectOfType<LevelManager>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            coinsDropped = Random.Range(1, 4);
            coinsText.text = "+" + coinsDropped.ToString() + " Coins";
            coinCanvas.gameObject.SetActive(false);
            currentHealth = maxHealth;
        }

        private void Update()
        {
            if(rb.velocity.x > 0 || rb.velocity.y > 0)
            {
                StartCoroutine(ResetKnockback());
            }
        }

        public void TakeDamage(int damage)
        {

            SoundManager.Instance.PlaySound(damageSound);

            animator.SetTrigger("Hurt");
            currentHealth -= damage;

            healthBar.UpdateHealthBar(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                animator.SetBool("IsDead", true);
                animator.SetTrigger("Dying");
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            rb.velocity = Vector2.zero;
        }

        private void OnCollisionEnter(Collision collision)
        {
            rb.velocity = Vector2.zero;
        }
        private IEnumerator ResetKnockback()
        {
            yield return new WaitForSeconds(knockbackDuration);
            rb.velocity = Vector2.zero;
        }

        public void Dies()
        {

            SoundManager.Instance.PlaySound(deathSound);

            levelManager.enemyDied(coinsDropped);
            coinCanvas.gameObject.SetActive(true);

            Collider2D[] colliders = GetComponents<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = false;
            }

            if (isDevil)
            {
                DevilController devilController = GetComponent<DevilController>(); 
                devilController.enabled = false;
            } else {
                EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
                EnemyAttack enemyAttack = GetComponent<EnemyAttack>();
                enemyAttack.enabled = false;
                enemyMovement.enabled = false;
            }
            StartCoroutine(FadeOutTimer());
        }

        private IEnumerator FadeOutTimer()
        {
            yield return new WaitForSeconds(fadeOutTime);
            Destroy(gameObject);
        }
    }
