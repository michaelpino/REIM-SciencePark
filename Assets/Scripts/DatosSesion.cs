using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatosSesion : MonoBehaviour {

    DateTime horaYFechaInicio = new DateTime();
    DateTime horaYFechaTermino = new DateTime();
    public string horaFechaInicio = "";
    public string horaFechaTermino = "";
    public string idSesion = "";
    public string idAlumno = "";
    public int instruccionesLeidas = 0;
    public int cambioDeVista = 0;
    public int ingresoIntroA1 = 0;
    public int ingresoIntroA2 = 0;
    public int ingresoIntroA3 = 0;
    public int inicioA1 = 0;
    public int inicioA2 = 0;
    public int inicioA3 = 0;
    private double duracionTemp = 0;
    public int duracion = 0;

    public void IniciaReestableceSesion () {
        if (PlayerPrefs.GetString("sesionIniciada").Equals("true"))
        {
            RecuperarPlayerPrefs();           
        }
        else
        {
            PlayerPrefs.SetString("sesionIniciada", "true");
            horaYFechaInicio = DateTime.Now;
            horaFechaInicio = horaYFechaInicio.ToString();
            PlayerPrefs.SetString("horaFechaInicio", horaFechaInicio);
        }
        idSesion = PlayerPrefs.GetString("idSesion");
        idAlumno = PlayerPrefs.GetString("idAlumno");
	}
	

    //Recupera los datos de la sesion desde PlayerPrefs
    public void RecuperarPlayerPrefs()
    {
        horaFechaInicio = PlayerPrefs.GetString("horaFechaInicio");
        instruccionesLeidas = PlayerPrefs.GetInt("instruccionesLeidas");
        cambioDeVista = PlayerPrefs.GetInt("cambioDeVista");
        ingresoIntroA1 = PlayerPrefs.GetInt("ingresoIntroA1"); ;
        ingresoIntroA2 = PlayerPrefs.GetInt("ingresoIntroA2"); ;
        ingresoIntroA3 = PlayerPrefs.GetInt("ingresoIntroA3"); ;
        inicioA1 = PlayerPrefs.GetInt("inicioA1"); ;
        inicioA2 = PlayerPrefs.GetInt("inicioA2"); ;
        inicioA3 = PlayerPrefs.GetInt("inicioA3"); ;
    }

    //Guarga los datos de la sesion desde PlayerPrefs
    public void GuardarPlayerPrefs()
    {
        PlayerPrefs.SetInt("instruccionesLeidas", instruccionesLeidas);
        PlayerPrefs.SetInt("cambioDeVista", cambioDeVista);
        PlayerPrefs.SetInt("ingresoIntroA1", ingresoIntroA1);
        PlayerPrefs.SetInt("ingresoIntroA2", ingresoIntroA2);
        PlayerPrefs.SetInt("ingresoIntroA3", ingresoIntroA3);
        PlayerPrefs.SetInt("inicioA1", inicioA1);
        PlayerPrefs.SetInt("inicioA2", inicioA2);
        PlayerPrefs.SetInt("inicioA3", inicioA3);
    }


    public void GuardarBD()
    {
        instruccionesLeidas = instruccionesLeidas / 2;      //Arreglo temporal...
        horaYFechaTermino = DateTime.Now;
        horaFechaTermino = horaYFechaTermino.ToString();
        horaYFechaInicio = DateTime.Parse(horaFechaInicio);
        TimeSpan ts = horaYFechaTermino - horaYFechaInicio;
        duracionTemp = ts.TotalSeconds;
        duracion = (int)duracionTemp;
        // Set this before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://reim-primero-basico.firebaseio.com/");
        // Get the root reference location of the database.
        DatabaseReference referenciaFirebase = FirebaseDatabase.DefaultInstance.RootReference;
        string json = JsonUtility.ToJson(this);
        //Debug.Log(json);
        referenciaFirebase.Child("ciencias").Child(idAlumno).Child("registroSesiones").Push().SetRawJsonValueAsync(json);
        PlayerPrefs.SetString("sesionIniciada", "false");
    }


    //****Aumentadores de contadores****
    public void registrarInstruccionesLeidas()
    {
        instruccionesLeidas++;
    }

    public void registrarCambioDeVista()
    {
        cambioDeVista++;
    }

    public void registrarIngresoIntroA1()
    {
        ingresoIntroA1++;
    }

    public void registrarIngresoIntroA2()
    {
        ingresoIntroA2++;
    }

    public void registrarIngresoIntroA3()
    {
        ingresoIntroA3++;
    }

    public void registrarInicioA1()
    {
        inicioA1++;
        GuardarPlayerPrefs();
    }

    public void registrarInicioA2()
    {
        inicioA2++;
        GuardarPlayerPrefs();
    }

    public void registrarInicioA3()
    {
        inicioA3++;
        GuardarPlayerPrefs();
    }
}
