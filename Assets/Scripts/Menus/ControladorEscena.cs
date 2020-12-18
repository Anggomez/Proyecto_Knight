using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorEscena : MonoBehaviour
{
    [SerializeField]private float contadorMinotauros;
    public GameObject panel;
    private void Update()
    {
        if(contadorMinotauros == 7)
        {
            panel.SetActive(true);
        }
    }
    public void aumentarContadorMinotauros()
    {
        contadorMinotauros++;
    }
}
