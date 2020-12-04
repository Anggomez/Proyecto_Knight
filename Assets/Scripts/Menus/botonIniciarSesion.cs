using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class botonIniciarSesion : MonoBehaviour
{
    public TextMeshProUGUI textoEditableUsuario;
    public TextMeshProUGUI textoEditableContrasegna;
    public TextMeshProUGUI textoEditableEmail;
    public string cadenaConexion;
    private Button BotonLogin;
    void Start()
    {
        Debug.Log("Start");
        BotonLogin = GameObject.Find("BotonIniciarSesion").GetComponent<Button>();
        cadenaConexion = @"Data Source=DESKTOP-NFM0FSI,1433; Initial Catalog=KnightDB; User Id=admin; password=admin";
        BotonLogin.onClick.AddListener(() => cargarEscena(textoEditableUsuario.text, textoEditableContrasegna.text, textoEditableEmail.text));
    }
    public void cargarEscena(String usuario, String contrasegna, String email)
    {
        Debug.Log("CargarEscena");
        if (comprobarUsuario(usuario, contrasegna, email))
        {
       
            SceneManager.LoadScene("Main");
        }

    }
    public Boolean comprobarUsuario(String usuario, String Contraseña, String email)
    {
     
        SqlConnection sqlconn = new SqlConnection(cadenaConexion);
            sqlconn.Open();
        string comandoSQL = "select * from Usuario where Usuario='" + usuario + "' and contrasegna ='" + Contraseña + "' and email='" + email + "';";
        Debug.Log(comandoSQL);
        SqlCommand cmd = new SqlCommand(comandoSQL, sqlconn);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                sqlconn.Close();
                Debug.Log("True");
                return true;
            }
            else
            {
                sqlconn.Close();
                Debug.Log("False");
                return false;
            }
        

    }
}
