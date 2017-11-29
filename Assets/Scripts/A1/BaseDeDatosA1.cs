using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDeDatosA1 : MonoBehaviour {

    public static BaseDeDatosA1 instanciaCompartida;

    //Variables de la sesion
    private DateTime inicioSesion;
    private DateTime finSesion;
    private int countHaJugado = 0;              //Cuantas veces ha iniciado el juego
    private int countHaVistoInstrucciones = 0;  //Cuantas veces ha visto las instrucciones
    private int countRetirado = 0;              //Cuantas veces terminó el juego sin terminar todos los intentos de platos, mediante el boton salir
    private bool actividadAprobada = false;     //Se considera aprobada cuando se alcanza la fase final de la planta
    private int countAprobado = 0;              //Cuantas veces se finalizó correctamente la actividad
    private int countReprobado = 0;             //Cuantas veces se finalizó la actividad porque se excedió alguno de los niveles (luz y/o agua)

    //Variables del alumno
    private string rutAlumno;


    private void Awake()
    {
        instanciaCompartida = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SeInicioSesionA1()
    {
        inicioSesion = DateTime.Now;
    }

    public void SeCerroSesionA1()
    {
        finSesion = DateTime.Now;
    }

    //Funciones sobre contadores
    public void ActividadAprobada()
    {
        if (actividadAprobada == false)
        {
            actividadAprobada = true;
        }
        countAprobado++;
    }

    public void ActividadReprobada()
    {
        countReprobado++;
    }

    public void ActividadIniciada()
    {
        countHaJugado++;
    }

    public void VistoInstrucciones()
    {
        countHaVistoInstrucciones++;
    }


    public void SeRindio()
    {
        countRetirado++;
    }

}
