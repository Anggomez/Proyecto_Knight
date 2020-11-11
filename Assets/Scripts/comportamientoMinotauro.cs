﻿// Esta clase esta dirigida a asignar la IA al enemigo Minotauro
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class comportamientoMinotauro : MonoBehaviour
{
    #region Variables Publicas
    public float distanciaAtaque; //Distancia minima para atacar
    public float velocidadMovimiento; // Asigna velocidad
    public float contador; //Timer para el coolDown
    public Transform limiteIzquierdo; // Delimitador izquierdo
    public Transform limiteDerecho; // Delimitador derecho
    [HideInInspector] public Transform transformador;
    [HideInInspector] public bool enRango; // Comprueba si esta en rango
    public GameObject hotZone;
    public GameObject areaActivacion; 
    #endregion

    #region Variables Privadas
    private Animator anim; // Objeto animator
    private float distancia; //Guarda distancia Enemigo-Jugador
    private bool modoAtaque;
    private bool coolDown; // Cooldown del ataque
    private float intContador;
    #endregion
    
    void Awake() // Se ejecuta al lanzarse
    {
        SeleccionarObjetivo();
        intContador = contador; // Guarda el primer valor del contador
        anim = GetComponent<Animator>();
    }
    
    void Update() // Se ejecuta en cada frame
    {
        if (!modoAtaque)
        {
            Mover();
        }

        if (!dentroLimites() && !enRango && !anim.GetCurrentAnimatorStateInfo(0).IsName("Minotauro_ataque"))
        {
            SeleccionarObjetivo();
        }

       
        if (enRango)
        {
            logicaEnemigo();
        }
    }


   
    void logicaEnemigo()  // Logica del enemigo
    {
        distancia = Vector2.Distance(transform.position, transformador.position);

        if (distancia > distanciaAtaque) // Si no esta a rango para de atacar
        {
            PararAtaque();
        }
        else if (distanciaAtaque >= distancia && coolDown == false) // Si esta en rango y puede atacar ataca
        {
            atacar();
        }

        if (coolDown) // si no puede atacar, resetea el ataque
        {
            Cooldown();
            anim.SetBool("Ataque", false);
        }
    }

    void Mover() // Mueve al personaje
    {
        anim.SetBool("Andar", true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Minotauro_ataque"))
        {
            Vector2 targetPosition = new Vector2(transformador.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, velocidadMovimiento * Time.deltaTime);
        }
    }

    void atacar() // Indica que ataque al enemigo
    {
        contador = intContador; // Resetea el contador
        modoAtaque = true; // comprueba si el enemigo sigue atacando

        anim.SetBool("Andar", false);
        anim.SetBool("Ataque", true);
    }

    void Cooldown() // reduce el cooldown
    {
        contador -= Time.deltaTime;

        if (contador <= 0 && coolDown && modoAtaque)
        {
            coolDown = false;
            contador = intContador;
        }
    }

    void PararAtaque() // Para el ataque e inicia el cooldown
    {
        coolDown = false;
        modoAtaque = false;
        anim.SetBool("Ataque", false);
    }

    public void activarCooldown() // Se lanza desde animacion para resetear ooldown
    {
        coolDown = true;
    }

    private bool dentroLimites()
    {
        return transform.position.x > limiteIzquierdo.position.x && transform.position.x < limiteDerecho.position.x;
    }

    public void SeleccionarObjetivo() // Asigna la direccion y rotacion
    {
        float distanciaIzquierda = Vector3.Distance(transform.position, limiteIzquierdo.position);
        float distanciaDerecha = Vector3.Distance(transform.position, limiteDerecho.position);

        if (distanciaIzquierda > distanciaDerecha)
        {
            transformador = limiteIzquierdo;
        }
        else
        {
            transformador = limiteDerecho;
        }

        // Se podria cambiar el if por un operador ternario, pero asi es mas legible
        //target = distanceToLeft > distanceToRight ? leftLimit : rightLimit;

        Rotar(); 
    }

    public void Rotar()// intercambia la direccion
    {
        Vector3 rotacion = transform.eulerAngles;
        if (transform.position.x > transformador.position.x) 
        {
            rotacion.y = 180;
        }
        else
        {
            Debug.Log("Twist");
            rotacion.y = 0;
        }

        //Tambien se podria poner operador ternario aqui
        //rotation.y = (currentTarget.position.x < transform.position.x) ? rotation.y = 180f : rotation.y = 0f;

        transform.eulerAngles = rotacion;
    }
}
