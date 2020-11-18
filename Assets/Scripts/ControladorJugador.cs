using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorJugador : MonoBehaviour
{
    #region variables privadas
    private Animator anim;
   
    private Rigidbody2D rb; // Referencia al componente Rigidbody
    private int cantidadSaltosDisponibles;
    private float movementInputDirection; // guarda la dirección 
   
    private bool isFacingRight = true;
    private bool isWalking;
    private bool estaEnSuelo;
    private bool puedeSaltar;
    private bool isTouchingWall;
    #endregion
    #region variables publicas
    public int cantidadSaltos=2;
    public float movementSpeed = 10.0f; // Velocidad de movimiento
    public float jumpForce = 10.0f; // Fuerza de salto
    public float comprobarSueloRadio; // Radio que se usa como paremetro de una única función
    public LayerMask layerSuelo; // capa del suelo
    public Transform comprobarSuelo; // Objeto hijo
    #endregion
    #region Metodos de Unity
    // Se le llama como primer método
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // asignamos el componente Rigidbody
        anim = GetComponent<Animator>(); // asignamos el componente del animator
        cantidadSaltosDisponibles = cantidadSaltos;
    }

    // Se le llama 1 vez por frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
    }
    private void FixedUpdate() // Se diferencia del Update en que este, se puede ejecutar 0, 1 o varias veces por frame
    {
        ApplyMovement();
        comprobarAlrededores();
    }
    private void OnDrawGizmos() // Se usa para dibujar formas en la vista de una escena
    {
        Gizmos.DrawWireSphere(comprobarSuelo.position, comprobarSueloRadio);
    }

    #endregion
    #region metodos propios
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
    private void UpdateAnimations() // actualiza las animaciones segun el estado actual
    {
        anim.SetBool("isWalking", isWalking); // Cambia el parametro del animador a true, si se esta moviendo
        anim.SetBool("isGrounded", estaEnSuelo); // si se esta en el suelo
        
    }
    private void CheckMovementDirection() //Comprueba la dirección a la que mira el personaje
    {// El primer if lo uso para orientarlo, y el segundo, para las animaciones
        if (isFacingRight && movementInputDirection <0) // si se ha movido a la izquierda (menor que 0 por orientación en el eje)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0){
            Flip();
        }
        // if (rb.velocity.x != 0) no funciona
        if (rb.velocity.x > 0.01f || rb.velocity.x < -0.01f) // Si esta moviendose en el eje X(Horizontalmente)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }
    private void Jump()
    {
        if (puedeSaltar)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            cantidadSaltosDisponibles--;
        }
        
    }
    private void CheckIfCanJump()
    {
        if (estaEnSuelo && rb.velocity.y <= 0.01)
        {
            cantidadSaltosDisponibles = cantidadSaltos;

        }
        if (cantidadSaltosDisponibles <= 0)
        {
            puedeSaltar = false;
        }
        else
        {
            puedeSaltar = true;
        }
    }
    private void Flip() // Da la vuelta al personaje y cambia el booleano que comprueba que este mirando a la derecha
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f); // Rota al personaje, en los ejes XYZ
    }
    private void comprobarAlrededores()
    {
        estaEnSuelo = Physics2D.OverlapCircle(comprobarSuelo.position, comprobarSueloRadio, layerSuelo);
    }
    #endregion
}
