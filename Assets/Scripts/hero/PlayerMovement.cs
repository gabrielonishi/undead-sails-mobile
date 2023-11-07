using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Base stats
    public float speed = 5.0f;

    // Object info
    private Animator animator;

    // In-program Variables
    private Vector2 direction;
    private float lastHorizontalInput;

    [SerializeField] private InputActionReference moveActionToUse;
    private void Start()
    {
        animator = GetComponent<Animator>();
        lastHorizontalInput = 1;
    }

    private void Update()
    {
        Vector2 moveDirection = moveActionToUse.action.ReadValue<Vector2>();
        transform.Translate(moveDirection * speed * Time.deltaTime);
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        direction = new Vector2(Math.Abs(horizontalInput), verticalInput);
        animator.SetFloat("Speed", Math.Abs(direction.magnitude));
        transform.Translate(direction * speed * Time.deltaTime);
        if ((horizontalInput > 0 && lastHorizontalInput < 0) || (horizontalInput < 0 && lastHorizontalInput > 0))
        {
            this.transform.Rotate(0f, 180.0f, 0f);
        }
        if (horizontalInput != 0) lastHorizontalInput = horizontalInput;
    }
}
