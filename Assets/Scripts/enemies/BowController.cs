using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController : MonoBehaviour
{
    private GameObject playerGameObject;
    private Transform playerTransform;  
    private void Start()
    {
        playerGameObject = GameObject.Find("Player");
        playerTransform = playerGameObject.transform;
    }

    void Update()
    {
        Vector2 bowPosition = transform.position;

        if (playerTransform == null)
            return;

        Vector2 direction = playerTransform.position - transform.position;

    }
}
