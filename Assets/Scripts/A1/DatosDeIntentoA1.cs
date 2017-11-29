using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatosDeIntentoA1 : MonoBehaviour {

    public DateTime horaYFechaInicio = new DateTime();
    public DateTime horaYFechaTermino = new DateTime();
    public string horaFechaInicio = "";
    public string horaFechaTermino = "";
    public string idActividad = "";
    public string nombreActividad = "";
    public string alumno = "";
    public string duracion = "";
    public int fertilizanteAgregadoCorrectamente = 0;
    public int fertilizanteAgregadoIncorrectamente = 0;
    public bool tiempoAgotado = false;
    public int aguaAgregada = 0;
    public int cambioPosicionMacetero = 0;
    public int instruccionesLeidas = 0;
    public int seRetiro = 0;
    public List<string> registroDeAcciones = new List<string>();


    public void registrarFertilizanteAgregado(bool resultado)
    {
        string registro = "|" + DateTime.Now.ToLongTimeString() + "|"+ "Se Agrego fertilizante" + "|" + resultado.ToString() + "|";
        registroDeAcciones.Add(registro);
        if (resultado)
        {
            fertilizanteAgregadoCorrectamente++;
        }else
        {
            fertilizanteAgregadoIncorrectamente++;
        }
    }

    public void registrarAguaAgregada()
    {
        string registro = "|" + DateTime.Now.ToLongTimeString() + "|" + "Se Agrego agua" + "|";
        registroDeAcciones.Add(registro);
        aguaAgregada++;
    }

    public void registrarMovimientoMacetero()
    {
        string registro = "|" + DateTime.Now.ToLongTimeString() + "|" + "Se de lugar movio el macetero" + "|";
        registroDeAcciones.Add(registro);
    }

    //funcion que registra el inicio del intento
    public void registrarInicio()
    {
        horaYFechaInicio = DateTime.Now;
        horaFechaInicio = horaYFechaInicio.ToString();
    }

    /*public void registrarAcierto()
    {
        fertilizanteAgregadoCorrectamente++;
    }

    public void registrarEquivocacion()
    {
        fertilizanteAgregadoIncorrectamente++;
    }*/

    public void registrarTiempoAgotado()
    {
        tiempoAgotado = true;
    }

    public void registrarInstruccionesLeidas()
    {
        instruccionesLeidas++;
    }

    public void registrarRetirada()
    {
        seRetiro++;
    }

    public void GuardarBD(string idActividadAux, string nombreActividadAux, string alumnoAux)
    {
        horaYFechaTermino = DateTime.Now;
        horaFechaTermino = horaYFechaTermino.ToString();
        TimeSpan ts = horaYFechaTermino - horaYFechaInicio;
        duracion = ts.Seconds.ToString();
        idActividad = idActividadAux;
        nombreActividad = nombreActividadAux;
        alumno = alumnoAux;
        // Set this before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://reim-primero-basico.firebaseio.com/");
        // Get the root reference location of the database.
        DatabaseReference referenciaFirebase = FirebaseDatabase.DefaultInstance.RootReference;
        string json = JsonUtility.ToJson(this);
        //Debug.Log(json);
        //referenciaFirebase.Child("ciencias").Child(alumno).Child("registroIntentos").Child(DateTime.Now.ToString()).SetRawJsonValueAsync(json);
        referenciaFirebase.Child("ciencias").Child(alumno).Child("registroIntentos").Child(nombreActividad).Push().SetRawJsonValueAsync(json);
    }
}
