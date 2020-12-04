using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class areaActivacion : MonoBehaviour
{
    private comportamientoMinotauro comportamientoMino;

    private void Awake()
    {
        comportamientoMino = GetComponentInParent<comportamientoMinotauro>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            comportamientoMino.tr = collider.transform;
            comportamientoMino.enRango = true;
            comportamientoMino.hotZone.SetActive(true);
        }
    }
}
