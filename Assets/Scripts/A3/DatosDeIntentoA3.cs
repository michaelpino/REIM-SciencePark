using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatosDeIntentoA3 : MonoBehaviour {

    public DateTime horaYFechaInicio = new DateTime();
    public DateTime horaYFechaTermino = new DateTime();
    public string horaFechaInicio = "";
    public string horaFechaTermino = "";
    public string idActividad = "";
    public string nombreActividad = "";
    public string alumno = "";
    public string duracion = "";
    public int animalesAcertados = 0;
    public int animalesEquivocados = 0;
    public int tiempoAgotado = 0;
    public int instruccionesLeidas = 0;
    public bool seRetiro = false;
    public List<string> registroDeEnvios = new List<string>();

    //funcion que registra cada envio de un animal hacia un portal
    public void registrarEnvio(string animal, string portal, bool resultado)
    {
        string registro = "|" + DateTime.Now.ToLongTimeString() + "|" + animal + "|" + portal + "|" + resultado.ToString() + "|";
        registroDeEnvios.Add(registro);
    }

    //funcion que registra el inicio del intento
    public void registrarInicio()
    {
        horaYFechaInicio = DateTime.Now;
        horaFechaInicio = horaYFechaInicio.ToString();
    }

    public void registrarAcierto()
    {
        animalesAcertados++;
    }

    public void registrarEquivocacion()
    {
        animalesEquivocados++;
    }

    public void registrarTiempoAgotado()
    {
        tiempoAgotado++;
    }

    public void registrarInstruccionesLeidas()
    {
        instruccionesLeidas++;
    }

    public void registrarRetirada()
    {
        seRetiro = true;
    }

    public void GuardarBD(string idActividadAux, string nombreActividadAux, string alumnoAux)//, string idDatoActividad)
    {
        horaYFechaTermino = DateTime.Now;
        horaFechaTermino = horaYFechaTermino.ToString();
        TimeSpan ts = horaYFechaTermino - horaYFechaInicio;
        duracion = ts.TotalSeconds.ToString();
        idActividad = idActividadAux;
        nombreActividad = nombreActividadAux;
        alumno = alumnoAux;
        // Set this before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://reim-primero-basico.firebaseio.com/");
        // Get the root reference location of the database.
        DatabaseReference referenciaFirebase = FirebaseDatabase.DefaultInstance.RootReference;
        string json = JsonUtility.ToJson(this);
        //referenciaFirebase.Child("ciencias").Child(alumno).Child("registroIntentos").Child(DateTime.Now.ToString()).SetRawJsonValueAsync(json);
        referenciaFirebase.Child("ciencias").Child(alumno).Child("registroIntentos").Child(nombreActividad).Push().SetRawJsonValueAsync(json);
    }

}
