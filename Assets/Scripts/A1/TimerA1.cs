using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerA1 : MonoBehaviour {

    public static TimerA1 instanciaCompartida;

    float humedadMaxima = 60.0f;
    float humedadActual;
    float luzMaxima = 80.0f;
    float luzActual;
    //int posicionLuz;
    //float tiempoMaximo = 0.0f;
    public float tiempoActual;
    //float tiempoGuardado;       //Variable en que guardo el tiempo que va al hacer una pausa, por ej. para mostrar las instrucciones
    float humedadGuardado;
    float luzGuardado;
    int sentidoDeLaLuz = -1;        //+1 = llega luz, -1 = no llega luz

    float nivelHumedad = 0f;
    float nivelLuz = 0f;

    private void Awake()
    {
        instanciaCompartida = this;

    }
    // Use this for initialization
    void Start()
    {
        humedadActual = (6*humedadMaxima)/10;
        luzActual = luzMaxima / 2;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (ControladorA1.instanciaCompartida.estadoActualA1 == estadoA1.juego)
        {
            if (tiempoActual >= 0)
            {
                tiempoActual += Time.deltaTime;
                humedadActual -= Time.deltaTime;
                luzActual = luzActual + (Time.deltaTime * sentidoDeLaLuz);
                nivelHumedad = ((humedadMaxima - humedadActual) / humedadMaxima);
                nivelLuz = ((luzMaxima - luzActual) / luzMaxima);
                ControladorA1.instanciaCompartida.DisminuirHumedad(((humedadMaxima - humedadActual) / humedadMaxima), ((luzMaxima - luzActual) / luzMaxima));
                
                //Se indica cada cuantos segundos se debe cambiar la luz de posicion
                if (tiempoActual > 7)
                {
                    tiempoActual = 0f;
                    Luz.instanciaCompartida.CambiarPosicionLuz();
                }
                
                //Se comprueba si los niveles de umedad o luz exceden los límites
                if (nivelHumedad < 0.05 || nivelHumedad > 0.95)
                {
                    ControladorA1.instanciaCompartida.intento.registrarTiempoAgotado();
                    ControladorA1.instanciaCompartida.CambiarEstadoA1(estadoA1.fin);
                }
                if (nivelLuz < 0.02 || nivelLuz > 0.98)
                {
                    ControladorA1.instanciaCompartida.intento.registrarTiempoAgotado();
                    ControladorA1.instanciaCompartida.CambiarEstadoA1(estadoA1.fin);
                }
            }           
        }
    }

    public void ResetTimer()
    {
        humedadActual = (6 * humedadMaxima) / 10;
        luzActual = luzMaxima / 2;
        tiempoActual = 0;
    }

    public void PauseTimer()
    {
        humedadGuardado = humedadActual;
        luzGuardado = luzActual;
        tiempoActual = -10000.0f;
    }

    public void ResumeTimer()
    {
        humedadActual = humedadGuardado;
        luzActual = luzGuardado;
        tiempoActual = 0;
    }


    //Agrega agua a la planta, y comprueba si al agregar se excede de humedad
    public void Humedecer()
    {
        if ((humedadActual + (humedadMaxima / 8)) > (humedadMaxima*0.98))
        {
            humedadActual = humedadMaxima * 0.99f;
        }
        else
        {
            ControladorA1.instanciaCompartida.mensajeTexo.text = "Agregaste agua a la planta";
            humedadActual += (humedadMaxima / 8);
        }      
    }


    public void CambiarSentidoLuz(int sentido)
    {
        sentidoDeLaLuz = sentido;
    }


    //Comprueba si la planta se encuentra en sus niveles ideales de humedad, para ver si se puede agregar fertilizante
    public bool NivelCorrectoHumedad()
    {       
        if (nivelHumedad > 0.4f && nivelHumedad < 0.6f)
        {
            return true;
        }
        else {
            return false;
        }
    }


    //Comprueba si la planta se encuentra en sus niveles ideales de luz, para ver si se puede agregar fertilizante
    public bool NivelCorrectoLuz()
    {      
        if (nivelLuz > 0.40f && nivelLuz < 0.65f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
