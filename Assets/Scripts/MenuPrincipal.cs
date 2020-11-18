using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;

//using System.Runtime.Hosting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class MenuPrincipal : MonoBehaviour
{
    public void cerrarJuego()
    {
        SceneManager.LoadScene("Cerrar");
    }    
    public void abrirAjustes()
    {
        SceneManager.LoadScene("Ajustes");
    }
}
