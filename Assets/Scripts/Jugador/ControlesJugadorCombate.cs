﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlesJugadorCombate : MonoBehaviour
{
    #region variables privadas
    [SerializeField]
    private bool combateHabilitado;
    [SerializeField]
    private float contadorEntrada, radioAtaque1, danioAtaque1;
    [SerializeField]
    private Transform posicionAtaque;
    [SerializeField]
    private LayerMask queEsDaniable;
    
    private bool recibioEntrada, estaAtacando, esPrimerAtaque;

    private float tiempoUltimaEntrada = Mathf.NegativeInfinity;

    private Animator anim;
    #endregion
    #region metodos de Unity
    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("puedeAtacar", combateHabilitado);
    }

    private void Update()
    {
        ControlarEntradas();
        comprobarAtaques();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(posicionAtaque.position, radioAtaque1);
    }
    #endregion
    #region metodos propios
    private void ControlarEntradas()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (combateHabilitado)
            {
                //Attempt combat
                recibioEntrada = true;
                tiempoUltimaEntrada = Time.time;
            }
        }
    }

    private void comprobarAtaques()
    {
        if (recibioEntrada)
        {
            //Perform Attack1
            if (!estaAtacando)
            {
                recibioEntrada = false;
                estaAtacando = true;
                esPrimerAtaque = !esPrimerAtaque;
                anim.SetBool("ataque1", true);
                anim.SetBool("primerAtaque", esPrimerAtaque);
                anim.SetBool("estaAtacando", estaAtacando);
            }
        }

        if(Time.time >= tiempoUltimaEntrada + contadorEntrada)
        {
            //Wait for new input
            recibioEntrada = false;
        }
    }

    private void comprobarHitBoxAtaque() //comprueba si hay un enemigo o varios golpeado/s y envia mensaje de daño a cada uno
    {
        Collider2D[] objetosDetectados = Physics2D.OverlapCircleAll(posicionAtaque.position, radioAtaque1, queEsDaniable);

        foreach (Collider2D collider in objetosDetectados)
        {
            collider.transform.parent.SendMessage("daniar", danioAtaque1);
            //Instantiate hit particle
        }
    }

    private void acabarAtaque1()
    {
        estaAtacando = false;
        anim.SetBool("estaAtacando", estaAtacando);
        anim.SetBool("ataque1", false);
    }
    #endregion
}
