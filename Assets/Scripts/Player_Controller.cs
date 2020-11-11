using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    private float movementInputDirection;

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }
    private void CheckInput()
    {
       // movementInputDirection = movementInputDirection.GetAxisRaw("Horizontal");
    }
    private void ApplyMovement()
    {

    }
}
