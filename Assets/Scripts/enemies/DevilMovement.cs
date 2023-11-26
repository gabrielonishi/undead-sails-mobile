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

    // GameObject info
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Components Outside of GameObject
    private Transform playerTransform;
    private GameObject playerGameObject;

    // In-program Variables
    private Vector2 direction;
    private float distance;
    private float lastHorizontalPosition = -1.0f;
    private Vector2 new_direction;

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

        Debug.Log(distance);    

        if (distance < tooCloseDist){
            // Get away from player
            animator.SetFloat("Speed", Math.Abs(direction.magnitude));
            new_direction = new Vector2(Math.Abs(direction.normalized.x), - direction.normalized.y);
        }
        else if (distance > tooFarDist){
            // Get closer to player
            animator.SetFloat("Speed", Math.Abs(direction.magnitude));
            new_direction = new Vector2(-Math.Abs(direction.normalized.x), direction.normalized.y);
        }
        else {
            // Stay still
            animator.SetFloat("Speed", 0);
            new_direction = new Vector2(0, 0);
        }
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
}
