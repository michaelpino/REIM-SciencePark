using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDeDatosA2 : MonoBehaviour {

    public static BaseDeDatosA2 instanciaCompartida;

    //Variables de la sesion
    private DateTime inicioSesion;
    private DateTime finSesion;
    private int countHaJugado = 0;              //Cuantas veces ha iniciado el juego
    private int countHaVistoInstrucciones = 0;  //Cuantas veces ha visto las instrucciones
    private int countRetirado = 0;              //Cuantas veces terminó el juego sin terminar todos los intentos de platos
    private bool actividadAprobada = false;     //Se considera aprobada cuando se alcanza al menos un 70% de aciertos, en la dificultad configurada
    private int countAprobado = 0;              //Cuantas veces aprobó en el intento
    private int countReprobado = 0;             //Cuantas veces reprobó en el intento
    //public String horayfecha;


    //Variables de sensores "Juego"
    public int countBordeArrastreAnimal = 0;                        //Cuantas veces no ha arrastrado exitosamente al animal
    public int countBordeBtnIzq = 0;                                //Cuantas veces no ha cambiado portal izq exitosamente
    public int countBordeBtnDer = 0;                                //Cuantas veces no ha cambiado portal der exitosamente
    public int countFondo = 0;                                      //Cuantas veces ha tocado en el fondo "no util"

    //Variables del alumno
    private bool sexo;              //true = hombre     false = mujer
    private string rutAlumno;
    //private int dificultad;       //1 = facil 2 = normal  3 = dificil

    //Variables para indicar acierto o confusion con respecto a un ingrediente
    private List<string> listaDeIngredientes = new List<string>();
    
    private List<int> listaDeIngredientesAcertados = new List<int>();       //
    private List<int> listaDeIngredientesConfundidos = new List<int>();     //
    private List<int> listaDeIngredientesPosibles = new List<int>();        //Cantidad de veces que un ingrediente estuvo disponible para agregarse


    private void Awake()
    {
        instanciaCompartida = this;
    }
    // Use this for initialization
    void Start () {
        Despensa.instanciaCompartida.EntregarListaDeAlimentos(listaDeIngredientes);
        for(int i = 0; i < listaDeIngredientes.Count;i++)        //Inicializando las equivocaciones y aciertos de cada ingrediente
        {
            listaDeIngredientesAcertados.Add(0);
            listaDeIngredientesConfundidos.Add(0);
            listaDeIngredientesPosibles.Add(0);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SeInicioSesionA2()
    {
        inicioSesion = DateTime.Now;
        //Debug.Log(inicioSesion.ToString());
    }

    public void SeCerroSesionA2()
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

    public void RegistrarAciertoErrorIngrediente (string ingrediente, bool aciertoError)        //Registra si un ingrediente fue correctamente agregado o si fue confundido. true=acertado, false=confundido
    {
        int index = listaDeIngredientes.IndexOf(ingrediente);
        if (aciertoError)
        {
            listaDeIngredientesAcertados[index]++;
        }
        else
        {
            listaDeIngredientesConfundidos[index]++;
        }
    }


    public void RegistrarIngredienteRequerido(string ingrediente)        //Registra que un ingrediente esta dentro de los ingredientes requeridos por el chef
    {
        int index = listaDeIngredientes.IndexOf(ingrediente);
        listaDeIngredientesPosibles[index]++;
    }
}
