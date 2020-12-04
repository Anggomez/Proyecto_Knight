using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotZoneCheck : MonoBehaviour
{
    private comportamientoMinotauro comportamientoMino;
    private bool enRango;
    private Animator anim;

    private void Awake()
    {
        comportamientoMino = GetComponentInParent<comportamientoMinotauro>();
        anim = GetComponentInParent<Animator>();

    }
    private void Update()
    {
        if(enRango && !anim.GetCurrentAnimatorStateInfo(0).IsName("minotauro_ataque"))
        {
            comportamientoMino.Rotar();
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            enRango = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            enRango = false;
            gameObject.SetActive(false);
            comportamientoMino.areaActivacion.SetActive(true);
            comportamientoMino.enRango = false;
            comportamientoMino.SeleccionarObjetivo();
        }
    }

}

