using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{
    // Debugging utils
    public bool isStatic = false;

    // Base stats
    [SerializeField]
    private float speed = 5.0f;

    // GameObject info
    [SerializeField]
    private Transform attackPoint;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Components Outside of GameObject
    private Transform playerTransform;
    private GameObject playerGameObject;

    // In-program Variables
    private Vector2 direction;
    private float deltaAttackPointX;
    private float lastHorizontalPosition = -1.0f;
    private Vector2 new_direction;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        deltaAttackPointX = attackPoint.position.x - transform.position.x;
        playerGameObject = GameObject.Find("Player");
        playerTransform = playerGameObject.transform;
    }

    private void Update()
    {
        if (playerTransform == null || isStatic)
            return;

        direction = playerTransform.position - transform.position;
        animator.SetFloat("Speed", Math.Abs(direction.magnitude));
        UpdateDirection(direction.x);
        //UpdateSpriteDirection(direction);
        //UpdateAttackPoint(direction);
        // transform.Translate(direction.normalized * speed * Time.deltaTime);
        new_direction = new Vector2(- Math.Abs(direction.normalized.x), direction.normalized.y);
        transform.Translate(new_direction * speed * Time.deltaTime);
    }

    private void UpdateDirection(float horizontalPosition)
    {
        if ((horizontalPosition > 0 && lastHorizontalPosition < 0) || (horizontalPosition < 0 && lastHorizontalPosition > 0))
        {
            this.transform.Rotate(0f, 180.0f, 0f);
        }
        if (horizontalPosition != 0) lastHorizontalPosition = horizontalPosition;
    }
    //void UpdateSpriteDirection(Vector2 direction)
    //{
    //    if (direction.x > 0)
    //    {
    //        spriteRenderer.flipX = false;
    //    }
    //    else if (direction.x < 0)
    //    {
    //        spriteRenderer.flipX = true;
    //    }
    //}

    //private void UpdateAttackPoint(Vector2 direction)
    //{
    //    if (direction.x > 0)
    //    {
    //        Vector3 newPosition = transform.position;
    //        float updatedX = transform.position.x + deltaAttackPointX;
    //        newPosition.x = updatedX;
    //        attackPoint.position = newPosition;
    //    }
    //    else if (direction.x < 0)
    //    {
    //        Vector3 newPosition = transform.position;
    //        float updatedX = transform.position.x - deltaAttackPointX;
    //        newPosition.x = updatedX;
    //        attackPoint.position = newPosition;
    //    }
    //}
}
