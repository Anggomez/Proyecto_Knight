using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagenDeDespues : MonoBehaviour
{
    [SerializeField]
    private float tiempoActivacion = 0.1f;
    private float tiempoActivado;
    private float alpha;
    [SerializeField]
    private float alphaSet = 0.8f;
    private float multiplicadorAlpha = 0.85f;

    private Transform jugador;

    private SpriteRenderer SR;
    private SpriteRenderer jugadorSR;

    private Color color;

    private void OnEnable()
    {
        SR = GetComponent<SpriteRenderer>();
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        jugadorSR = jugador.GetComponent<SpriteRenderer>();
        alpha = alphaSet;
        SR.sprite = jugadorSR.sprite;
        transform.position = jugador.position;
        transform.rotation = jugador.rotation;
        tiempoActivado = Time.time;
    }

    private void Update()
    {
        alpha *= multiplicadorAlpha;
        color = new Color(1f, 1f, 1f, alpha);
        SR.color = color;

        if(Time.time >= (tiempoActivado + tiempoActivacion))
        {
            ImagenDeDespuesPool.Instancia.aniadirAlPool(gameObject);
        }

    }
}
