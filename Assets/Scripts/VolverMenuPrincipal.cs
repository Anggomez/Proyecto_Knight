using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VolverMenuPrincipal : MonoBehaviour
{
    // Start is called before the first frame update
    public void volver()
    {
        SceneManager.LoadScene("Registro");
    }
}
