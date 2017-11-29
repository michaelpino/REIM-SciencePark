using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseDeDatosA3 : MonoBehaviour {

    public static BaseDeDatosA3 instanciaCompartida;

    //Variables de la sesion
    private DateTime inicioSesion;
    private DateTime finSesion;
    private int countHaJugado = 0;              //Cuantas veces ha iniciado el juego
    private int countHaVistoInstrucciones = 0;  //Cuantas veces ha visto las instrucciones
    private int countCompletado = 0;            //Cuantas veces terminó el juego terminando todos los intentos
    private int countRetirado = 0;              //Cuantas veces terminó el juego sin terminar todos los intentos
    private Boolean actividadAprobada = false;  //Se considera aprobada cuando se alcanza al menos un 70% de aciertos, en la dificultad configurada
    private int countAprobado = 0;              //Cuantas veces aprobó en el intento
    private int countReprobado = 0;             //Cuantas veces reprobó en el intento
    //public String horayfecha;


    //Variables de sensores "Juego"
    public int countBordeArrastreAnimal = 0;                        //Cuantas veces no ha arrastrado exitosamente al animal
    public int countBordeBtnIzq = 0;                                //Cuantas veces no ha cambiado portal izq exitosamente
    public int countBordeBtnDer = 0;                                //Cuantas veces no ha cambiado portal der exitosamente
    public int countFondo = 0;                                      //Cuantas veces ha tocado en el fondo "no util"

    //Variables del alumno
    private Boolean sexo;       //true = hombre     false = mujer
    private String rutAlumno;
    private int dificultad;     //1 = facil 2 = normal  3 = dificil



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

    public void GuardarDatosBD ()
    {

    }

    public void GuardarDatosLocal()
    {

    }

    public void SeInicioSesion()
    {
        inicioSesion = DateTime.Now;
    }

    public void SeCerroSesion()
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


    public void LoTermino()
    {
        countCompletado++;
    }

    public void SeRindio()
    {
        countRetirado++;
    }

}
