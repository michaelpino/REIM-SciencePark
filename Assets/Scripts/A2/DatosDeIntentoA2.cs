using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatosDeIntentoA2 : MonoBehaviour
{

    public DateTime horaYFechaInicio = new DateTime();
    public DateTime horaYFechaTermino = new DateTime();
    public string horaFechaInicio = "";
    public string horaFechaTermino = "";
    public string idActividad = "";
    public string nombreActividad = "";
    public string alumno = "";
    public string duracion = "";
    public List<int> alimentosAcertados = new List<int> {0, 0, 0};
    public List<int> alimentosEquivocados = new List<int> { 0, 0, 0 };
    public List<int> tiempoAgotado = new List<int> { 0, 0, 0 };
    public int alimentosAcertadosTotal = 0;
    public int alimentosEquivocadosTotal = 0;
    public int tiempoAgotadoTotal = 0;
    public int instruccionesLeidas = 0;
    public int seRetiro = 0;
    //public ArrayList registroDeEnvios = new ArrayList();
    public string alimentosCorrectosP1 = "";
    public string alimentosCorrectosP2 = "";
    public string alimentosCorrectosP3 = "";
    public List<string> registroDeEnviosP1 = new List<string>();
    public List<string> registroDeEnviosP2 = new List<string>();
    public List<string> registroDeEnviosP3 = new List<string>();

    //funcion que registra cada envio de un animal hacia un portal
    public void registrarEnvio(int plato, string alimento, bool resultado)
    {
        string registro = "|" + DateTime.Now.ToLongTimeString() + "|" + alimento + "|" + resultado.ToString() + "|";
        if (plato == 1)
        {
            registroDeEnviosP1.Add(registro);
        }
        else if (plato == 2)
        {
            registroDeEnviosP2.Add(registro);
        }
        else if (plato == 3)
        {
            registroDeEnviosP3.Add(registro);
        }

        if (resultado)
        {
            registrarAcierto(plato);
        }else
        {
            registrarEquivocacion(plato);
        }
    }

    public void registrarCambioHabitacion(int plato, string destino)
    {
        string registro = "|" + DateTime.Now.ToLongTimeString() + "|" + destino + "|";
        if (plato == 1)
        {
            registroDeEnviosP1.Add(registro);
        }
        else if (plato == 2)
        {
            registroDeEnviosP2.Add(registro);
        }
        else if (plato == 3)
        {
            registroDeEnviosP3.Add(registro);
        }

    }

    public void alimentosCorrectos(int numeroPlato, string alimento1, string alimento2, string alimento3)
    {
        string registro = "|" + alimento1 + "|" + alimento2 + "|" + alimento3 + "|";
        if (numeroPlato == 1)
        {
            alimentosCorrectosP1 = registro;
        }
        else if (numeroPlato == 2)
        {
            alimentosCorrectosP2 = registro;
        }
        else if (numeroPlato == 3)
        {
            alimentosCorrectosP3 = registro;
        }
    }


    //funcion que registra el inicio del intento
    public void registrarInicio()
    {
        horaYFechaInicio = DateTime.Now;
        horaFechaInicio = horaYFechaInicio.ToString();
    }

    public void registrarAcierto(int plato)
    {
        alimentosAcertados[plato - 1]++;
        alimentosAcertadosTotal++;
    }

    public void registrarEquivocacion(int plato)
    {
        alimentosEquivocados[plato - 1]++;
        alimentosEquivocadosTotal++;
    }

    public void registrarTiempoAgotado(int plato)
    {
        tiempoAgotado[plato - 1]++;
        tiempoAgotadoTotal++;
    }

    public void registrarInstruccionesLeidas()
    {
        instruccionesLeidas++;
    }

    public void registrarRetirada()
    {
        seRetiro++;
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
        //Debug.Log(json);
        //referenciaFirebase.Child("ciencias").Child(alumno).Child("registroIntentos").Child(DateTime.Now.ToString()).SetRawJsonValueAsync(json);
        referenciaFirebase.Child("ciencias").Child(alumno).Child("registroIntentos").Child(nombreActividad).Push().SetRawJsonValueAsync(json);
    }

}

