using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagenDeDespuesPool : MonoBehaviour
{
    #region declaraciones
    [SerializeField]
    private GameObject ImagenDeDespuesPrefab;
    private Queue<GameObject> objetosDisponibles = new Queue<GameObject>();
    public static ImagenDeDespuesPool Instancia { get; private set; }
    #endregion
    #region metodos

    private void Awake()
    {
        Instancia = this;
        CrecerPool();
    }

    private void CrecerPool()
    {
        for (int i = 0; i < 10; i++)
        {
            var instanciaAniadida = Instantiate(ImagenDeDespuesPrefab);
            instanciaAniadida.transform.SetParent(transform);
            aniadirAlPool(instanciaAniadida);
        }
    }

    public void aniadirAlPool(GameObject instance)
    {
        instance.SetActive(false);
        objetosDisponibles.Enqueue(instance);
    }

    public GameObject sacarDelPool()
    {
        if (objetosDisponibles.Count == 0)
        {
            CrecerPool();
        }

        var instance = objetosDisponibles.Dequeue();
        instance.SetActive(true);
        return instance;
    }
    #endregion
}