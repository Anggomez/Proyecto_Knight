using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorJugador : MonoBehaviour
{
    private float movementInputDirection; // guarda la dirección 
   
    private Rigidbody2D rb; // Referencia al componente Rigidbody
    
    private bool isFacingRight = true; 

    public float movementSpeed = 10.0f;

    public float jumpForce = 10.0f;

    // Se le llama como primer método
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // asignamos el componente Rigidbody
    }

    // Se le llama 1 vez por frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
    }
    private void FixedUpdate() // Se diferencia del Update en que este, se puede ejecutar 0, 1 o varias veces por frame
    {
        ApplyMovement();
    }
    private void CheckInput() // Comprueba la dirección
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal"); // Recibe el eje horizontal, en el cual has movido el personaje
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }
    private void ApplyMovement() // Ordena el movimiento
    {
        rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
    }
    private void CheckMovementDirection() //Comprueba la dirección a la que mira el personaje
    {
        if (isFacingRight && movementInputDirection <0) // si se ha movido a la izquierda (menor que 0 por orientación en el eje)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0){
            Flip();
        }
    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    private void Flip() // Da la vuelta al personaje y cambia el booleano que comprueba que este mirando a la derecha
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f); // Rota al personaje, en los ejes XYZ
    }
}
