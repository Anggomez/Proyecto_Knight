using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class MenuPrincipal : MonoBehaviour
{
    #region metodos publicos
    public AudioMixer mixer;

    public TextMeshProUGUI textoEditableUsuario, textoEditableContrasegna, textoEditableEmail;

    public GameObject panelSalir, panelSeleccionarNivel, FondoTapa, panelAjustes;

    public Button BotonIniciarSesion, BotonRegistrarse;

    public Toggle pantallaCompleta;

    public Dropdown desplegableDificultad, desplegableResolucion;
    #endregion
    #region metodos privados
    private int posicionDesplegableResolucion;
    private string cadenaConexion, Resolucion;
    #endregion
    #region metodos de Unity
    void Start()
    {
        BotonRegistrarse = GameObject.Find("BotonRegistrarse").GetComponent<Button>();
        BotonIniciarSesion = GameObject.Find("BotonIniciarSesion").GetComponent<Button>();
        cadenaConexion = @"Data Source=DESKTOP-NFM0FSI,1433; Initial Catalog=KnightDB; User Id=admin; password=admin";
        BotonIniciarSesion.onClick.AddListener(() => cargarEscena());
        BotonRegistrarse.onClick.AddListener(() => introducirUsuario());
    }
    private void Update()
    {
        posicionDesplegableResolucion = desplegableResolucion.value;
        Resolucion = desplegableResolucion.options[posicionDesplegableResolucion].text;
    }
    #endregion
    #region metodos de botones
    public void AbrirPanelSalir()
    {
        panelSalir.SetActive(true);
    }
    public void CerrarPanelSalir()
    {
        panelSalir.SetActive(false);
    }
    public void cerrarJuego()
    {
        Application.Quit();
    }
    public void abrirAjustes()
    {
        panelAjustes.SetActive(true);
    }
    public void cerrarAjustes()
    {
        if (Resolucion == "640x480")
        {
            if (pantallaCompleta == true)
            {
                Screen.SetResolution(640, 480, true, 60);
            }
            else
            {
                Screen.SetResolution(640, 480, false, 60);
            }

        }
        else
        {
            if (pantallaCompleta == true)
            {
                Screen.SetResolution(800, 600, true);
            }
            else
            {
                Screen.SetResolution(800, 600, false);
            }
        }

        panelAjustes.SetActive(false);
    }
    public void cargarNivel1()
    {
        Debug.Log("cargandoNivel1");
        SceneManager.LoadScene("Main");
    }
    public void cambiarVolumen(float sliderValue)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }
    #endregion
    #region metodos varios
    public void cargarEscena()
    {
        if (comprobarUsuario())
        {
            FondoTapa.SetActive(true);
            panelSeleccionarNivel.SetActive(true);
        }

    }
    public void introducirUsuario()
    {
        if (!comprobarUsuario())
        {
            SqlConnection sqlconn = new SqlConnection(cadenaConexion);
            sqlconn.Open();
            String sqlcadena = "Insert into Usuario(usuario, contrasegna, email)values('angel', 'angel1234', 'angeltaustez@gmail.com')";
            //String sqlcadena = "Insert into Usuario(usuario, contrasegna, email)values('" + textoEditableUsuario + "', '" + textoEditableContrasegna + "', '" + textoEditableEmail + "')";
            SqlCommand cmd = new SqlCommand(sqlcadena, sqlconn);
            cmd.ExecuteNonQuery();
            sqlcadena = "Insert into DatosUsuario(IDUsuario,nivel1Completado,nivel1Tiempo,nivel2Completado,nivel2Tiempo,nivel3Completado,nivel3Tiempo,nivel4Completado,nivel4Tiempo,nivel5Completado,nivel5Tiempo,nivel6Completado,nivel6Tiempo,nivel7Completado,nivel7Tiempo,dificultad,volumen)values(2, 0, null, 0, null, 0, null, 0, null, 0, null, 0, null, 0, null, 'normal', 0)";
            cmd = new SqlCommand(sqlcadena, sqlconn);
            cmd.ExecuteNonQuery();
        }

    }
    public Boolean comprobarUsuario()
    {
        SqlConnection sqlconn = new SqlConnection(cadenaConexion);
        sqlconn.Open();

        //String sqlcadena = "select * from Usuario where usuario = 'angel' and contrasegna = 'angel1234' and email = 'angeltaustez@gmail.com'";
        String sqlcadena = String.Format("select * from Usuario where usuario like '%{0}%' and contrasegna like '%{1}%' and email like '%{2}%'", textoEditableUsuario.text ,textoEditableContrasegna.text ,textoEditableEmail.text);

        // String sqlcadena = "select * from Usuario where usuario = '" + textoEditableUsuario.text + "' and contrasegna = '" + textoEditableContrasegna.text + "' and email = '" + textoEditableEmail.text + "'";
        // sqlcadena.Replace("'", "''");
        SqlCommand cmd = new SqlCommand(sqlcadena, sqlconn);
        var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            sqlconn.Close();
            return true;
        }
        else
        {
            sqlconn.Close();
            return false;
        }
    }
    #endregion
}
