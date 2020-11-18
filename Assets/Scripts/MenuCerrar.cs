using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCerrar : MonoBehaviour
{
    // Start is called before the first frame update
    public void cerrar()
    {
        Application.Quit();
    }
    public void volver()
    {
        SceneManager.LoadScene("Registro");
    }
}
