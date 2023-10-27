using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Base stats
    public float speed = 5.0f;

    // Object info
    [SerializeField]
    private Transform attackPoint;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // In-program Variables
    private Vector2 direction;
    private float deltaAttackPointX;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        deltaAttackPointX = attackPoint.position.x - transform.position.x;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        direction = new Vector2(horizontalInput, verticalInput);
        UpdateSpriteDirection(direction);
        UpdateAttackPoint(direction);
        animator.SetFloat("Speed", Math.Abs(direction.magnitude));
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void UpdateSpriteDirection(Vector2 direction)
    {
        if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void UpdateAttackPoint(Vector2 direction)
    {
        if (direction.x > 0)
        {
            Vector3 newPosition = transform.position;
            float updatedX = transform.position.x + deltaAttackPointX;
            newPosition.x = updatedX;
            attackPoint.position = newPosition;
        }
        else if (direction.x < 0)
        {
            Vector3 newPosition = transform.position;
            float updatedX = transform.position.x - deltaAttackPointX;
            newPosition.x = updatedX;
            attackPoint.position = newPosition;
        }
    }
}
