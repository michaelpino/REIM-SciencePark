using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerA2 : MonoBehaviour
{

    public static TimerA2 instanciaCompartida;

    float tiempoMaximo = 45.0f;
    public float tiempoRestante;
    float tiempoGuardado;       //Variable en que guardo el tiempo que va al hacer una pausa, por ej. para mostrar las instrucciones
    


    private void Awake()
    {
        instanciaCompartida = this;
        
    }
    // Use this for initialization
    void Start()
    {
        PauseTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if(tiempoRestante >= 0.00)
        {
            tiempoRestante -= Time.deltaTime;
            ControladorA2.instanciaCompartida.DisminuirTiempo(((tiempoMaximo - tiempoRestante) / tiempoMaximo));
        }
        

        if ((tiempoRestante < 0 && tiempoRestante > -0.9))
        {
            ControladorA2.instanciaCompartida.CambiarEstadoA2(estadoA2.tiempo_agotado);
        }

    }

    public void ResetTimer()
    {
        tiempoRestante = tiempoMaximo;
    }

    public void PauseTimer()
    {
        tiempoGuardado = tiempoRestante;
        tiempoRestante = -1.0f;

    }

    public void ResumeTimer()
    {
        tiempoRestante = tiempoGuardado;
    }


    /*public void MostrarTimer(bool mostrarlo)
    {
        if (mostrarlo)
        {
            timerTextUI.enabled = true;
            timerLegendTextUI.enabled = true;
        }
        else
        {
            timerTextUI.enabled = false;
            timerLegendTextUI.enabled = false;
        }
    }*/

    
}