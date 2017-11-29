using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatosDeActividadA3 : MonoBehaviour {

    DateTime horaYFechaInicio = new DateTime();
    DateTime horaYFechaTermino = new DateTime();
    public string horaFechaInicio = "";
    public string horaFechaTermino = "";
    public string nombreActividad = "Encuentra_mi_hogar";
    public string idActividad = DateTime.Now.ToString();         //Registra el id único que se usará para encontrar esta actividad
    public string idSesion = "";                          //Registra la sesion de reim a la que corresponde esta actividad
    public string idAlumno = "";
    public string duracion = "";
    public int mejorMarcador = 0;           //Mejor cantidad de correctas en alguno de los intento
    public int animalesCorrectosTotales = 0;           //Cantidad de correctas todos los intento
    public int animalesIncorrectosTotales = 0;           //Cantidad de correctas todos los intento
    public int instruccionesLeidas = 0;     //Veces que leyó las instrucciones en todos los intento
    public int retiradas = 0;               //Veces que se retiró en todos los intento
    public int intentos = 0;                //Veces que se llevó a cabo un intento
    public int tocoFondo = 0;               //Cuantas veces ha tocado en el fondo "no util"
    public int tocoBotonIzquierda = 0;      //Cuantas veces ha tocado el boton para ir a la Izquierda
    public int tocoBotonDerecha = 0;        //Cuantas veces ha tocado el boton para ir a la Derecha
    public int tocoFondoBotonera = 0;        //Cuantas veces ha tocado el boton para ir a la Derecha

    //funcion que registra el inicio de la actividad y registra un id para la misma
    public void registrarInicio()
    {
        horaYFechaInicio = DateTime.Now;
        horaFechaInicio = horaYFechaInicio.ToString();
    }

    //Actualiza el mejor marcador acorde a lo obtenido en el ultimo intento
    public void registrarMarcador(int aciertosActuales)
    {
        if (aciertosActuales > mejorMarcador)
        {
            mejorMarcador = aciertosActuales;
        }
    }

    public void registrarIntento()
    {
        intentos++;
    }

    public void registrarCorrecto()
    {
        animalesCorrectosTotales++;
    }

    public void registrarIncorrecto()
    {
        animalesIncorrectosTotales++;
    }

    public void registrarInstruccionesLeidas()
    {
        instruccionesLeidas++;
    }

    public void registrarRetirada()
    {
        retiradas++;
    }

    public void registrarToqueFondo()
    {
        tocoFondo++;
    }

    public void registrarToqueBotonIzquierda()
    {
        tocoBotonIzquierda++;
    }

    public void registrarToqueBotonDerecha()
    {
        tocoBotonDerecha++;
    }

    public void registrarToqueFondoBotonera()
    {
        tocoFondoBotonera++;
    }

    public void GuardarBD()//, string idDatoActividad)
    {
        horaYFechaTermino = DateTime.Now;
        horaFechaTermino = horaYFechaTermino.ToString();
        TimeSpan ts = horaYFechaTermino - horaYFechaInicio;
        duracion = ts.TotalSeconds.ToString();
        // Set this before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://reim-primero-basico.firebaseio.com/");
        // Get the root reference location of the database.
        DatabaseReference referenciaFirebase = FirebaseDatabase.DefaultInstance.RootReference;
        string json = JsonUtility.ToJson(this);
        //Debug.Log(json);
        //referenciaFirebase.Child("ciencias").Child(alumno).Child("registroIntentos").Child(DateTime.Now.ToString()).SetRawJsonValueAsync(json);
        referenciaFirebase.Child("ciencias").Child(idAlumno).Child("registroActividades").Child(nombreActividad).Push().SetRawJsonValueAsync(json);
    }


    public void SolicitarUsuarioYSesion()
    {
        idAlumno = PlayerPrefs.GetString("idAlumno");
        idSesion = PlayerPrefs.GetString("idSesion");
    }
}
