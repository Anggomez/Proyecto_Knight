using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagenDeDespues : MonoBehaviour
{
    [SerializeField]
    private float tiempoActivacion = 0.1f, tiempoActivado, alpha;
    [SerializeField]
    private float alphaSet = 0.8f, multiplicadorAlpha = 0.85f;

    private Transform jugador;

    private SpriteRenderer SR, jugadorSR;

    private Color color;

    private void OnEnable() // Cuando se habilita
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

    private void Update() // Se llama en cada frame, e instancia una imagen con los ajustes dados(Cambia posicion y el alpha/intensidad)
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
