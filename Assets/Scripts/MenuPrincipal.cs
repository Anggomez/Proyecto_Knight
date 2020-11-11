using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Runtime.Hosting;
using UnityEngine;

public class MenuPrincipal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void cargarEscena()
    {
       // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   
    }
    public void cerrarJuego()
    {
        Application.Quit();
    }
}
