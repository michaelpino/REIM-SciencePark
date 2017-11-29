using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControladorLogin : MonoBehaviour {

    public InputField usuario;
    public InputField pass;
    public Text mensajeAlUsuario;

    string mensajeDesdeBD = "";
    string idAlumnoTemp = "00000000";
    string sexoAlumnoTemp = "";
    bool respuestaUsuario = false;
    bool respuestaPass = false;

    // Use this for initialization
    void Start () {
        PlayerPrefs.SetString("idAlumno", idAlumnoTemp);
        PlayerPrefs.SetString("idSesion", "|00000000|");
        PlayerPrefs.SetString("sexoAlumno", "");
        PlayerPrefs.SetString("sesionIniciada", "false");
    }

    // Update is called once per frame
    void Update () {
        if (respuestaUsuario && respuestaPass && !sexoAlumnoTemp.Equals(""))
        {
            Debug.Log("Ingreso correcto :D");            
            IniciarReim(); 
        }
    }

    public void IniciarReim()
    {
        PlayerPrefs.SetString("idAlumno", idAlumnoTemp);
        PlayerPrefs.SetString("sexoAlumno", sexoAlumnoTemp);
        PlayerPrefs.SetString("idSesion", "|" + idAlumnoTemp + "|" + DateTime.Now.ToString() + "|");
        //PlayerPrefs.SetString("sesionIniciada", "true");
        GuardarLogBD();
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void IniciarReimModoPrueba()
    {
        PlayerPrefs.SetString("idAlumno", "98979695");
        PlayerPrefs.SetString("sexoAlumno", "true");
        PlayerPrefs.SetString("idSesion", "|" + "98979695" + "|" + DateTime.Now.ToString() + "|");
        GuardarLogBD();
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }


    public void IntentarLogin()
    {
        //Debug.Log("Usuario: " + usuario.text);
        //Debug.Log("Pass: " + pass.text);
        mensajeAlUsuario.text = "Conectando..";
        if (usuario.text.Length == 0)
        {
            mensajeAlUsuario.text = "Necesitas ingresar tu RUT";
        }
        else if (usuario.text.Length < 8)
        {
            mensajeAlUsuario.text = "Necesitas completar tu RUT";
        }
        else if (pass.text.Length == 0)
        {
            mensajeAlUsuario.text = "Necesitas ingresar tu clave";
        }
        else if (usuario.text.Equals("98979695"))
        {
            mensajeAlUsuario.text = "Conectando Modo Pruebas!";
        }
        else
        {
            ConsultarBD(usuario.text, pass.text);
        }   
    }

    void ConsultarBD(string usuarioConsultado, string passConsultado)
    {
        respuestaUsuario = false;
        respuestaPass = false;

        // Set this before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://reim-primero-basico.firebaseio.com/");
        
        // Get the root reference location of the database.
        DatabaseReference referenciaFirebase = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance
        .GetReference("alumnos")
        .GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                mensajeAlUsuario.text = "Sin conexion con BD";
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Child(usuarioConsultado).Exists)
                {
                    respuestaUsuario = true;
                    if (snapshot.Child(usuarioConsultado).Child("pass").GetValue(true).ToString().Equals(passConsultado))
                    {
                        sexoAlumnoTemp = snapshot.Child(usuarioConsultado).Child("sexo").GetValue(true).ToString();
                        idAlumnoTemp = usuarioConsultado;
                        respuestaPass = true;
                    }
                    else
                    {
                        mensajeAlUsuario.text = "Revisa tu clave de acceso";
                    }
                }
                else
                {
                    mensajeAlUsuario.text = "Revisa tu RUT";
                }
            }
        });
    }


    public void GuardarLogBD()
    {
        // Set this before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://reim-primero-basico.firebaseio.com/");
        // Get the root reference location of the database.
        DatabaseReference referenciaFirebase = FirebaseDatabase.DefaultInstance.RootReference;
        referenciaFirebase.Child("logInicioSesion").Child("ciencias").Push().SetValueAsync(PlayerPrefs.GetString("idSesion"));
    }
}
