using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DevilController : MonoBehaviour
{
    // Debugging utils
    public bool isStatic = false;

    // Base stats
    [SerializeField]
    private float speed = 5.0f, tooCloseDist = 3.0f, tooFarDist = 5.0f;

    [SerializeField]
    private float launchForce = 5.0f, attackDelay = 1.0f;

    // GameObject info
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Transform shotPoint;

    // Components Outside of GameObject
    private Transform playerTransform;
    private GameObject playerGameObject;
    public GameObject arrow;


    // In-program Variables
    private Vector2 direction;
    private float distance;
    private float lastHorizontalPosition = -1.0f;
    private Vector2 new_direction;
    private float passedTime = 0;
    private bool loadingAnimationPlaying = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerGameObject = GameObject.Find("Player");
        playerTransform = playerGameObject.transform;
    }

    private void Update()
    {
        if (playerTransform == null || isStatic)
            return;

        direction = playerTransform.position - transform.position;
        UpdateDirection(direction.x);
        distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance < tooCloseDist && !loadingAnimationPlaying){
            // Get away from player
            animator.SetFloat("Speed", Math.Abs(direction.magnitude));
            new_direction = new Vector2(Math.Abs(direction.normalized.x), - direction.normalized.y);
            transform.Translate(new_direction * speed * Time.deltaTime);
            loadingAnimationPlaying = false;
        }
        else if (distance > tooFarDist && !loadingAnimationPlaying){
            // Get closer to player
            animator.SetFloat("Speed", Math.Abs(direction.magnitude));
            new_direction = new Vector2(-Math.Abs(direction.normalized.x), direction.normalized.y);
            transform.Translate(new_direction * speed * Time.deltaTime);
            loadingAnimationPlaying = false;
        }
        else {
            // Stop moving and shoot arrow
            animator.SetFloat("Speed", 0);
            if (!loadingAnimationPlaying)
            {
                passedTime = 0;
                animator.SetTrigger("Load");
                loadingAnimationPlaying = true;
            }
            else
            {
                passedTime += Time.deltaTime;
            }

            if (passedTime >= attackDelay)
            {
                Shoot();
                passedTime = 0;
                loadingAnimationPlaying = false;
                animator.SetTrigger("GoToIdle");
            }
        }
        
    }

    private void UpdateDirection(float horizontalPosition)
    {   
        if ((horizontalPosition > 0 && lastHorizontalPosition < 0) || (horizontalPosition < 0 && lastHorizontalPosition > 0))
        {
            this.transform.Rotate(0f, 180.0f, 0f);
        }
        if (horizontalPosition != 0) lastHorizontalPosition = horizontalPosition;
    }

    private void Shoot()
    {
        // Shoot arrow at player
        GameObject newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);

        // Calculate the direction towards the player
        Vector2 shootDirection = (playerTransform.position - shotPoint.position).normalized;

        // Calculate the velocity needed to reach the player accounting for gravity
        Vector2 requiredVelocity = CalculateVelocityToReachTarget(playerTransform.position, shotPoint.position, launchForce);

        newArrow.GetComponent<Rigidbody2D>().velocity = requiredVelocity;
    }

    private Vector2 CalculateVelocityToReachTarget(Vector3 targetPosition, Vector3 initialPosition, float launchSpeed)
    {
        Vector3 displacement = targetPosition - initialPosition;
        float displacementX = displacement.x;
        float displacementY = displacement.y;

        // Calculate time to reach the target horizontally
        float timeToReachX = Mathf.Abs(displacementX / launchSpeed);

        // Calculate the vertical velocity needed to reach the target at the given time
        float verticalVelocity = (displacementY + 0.5f * Physics2D.gravity.magnitude * Mathf.Pow(timeToReachX, 2)) / timeToReachX;

        // Calculate the horizontal velocity
        float horizontalVelocity = displacementX / timeToReachX;

        return new Vector2(horizontalVelocity, verticalVelocity);
    }
}
