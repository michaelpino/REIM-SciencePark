using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public static Timer instanciaCompartida;

    float tiempoMaximo;
    float tiempoGuardado;       //Variable en que guardo el tiempo que va al hacer una pausa, por ej. para mostrar las instrucciones
    public Text timerTextUI;
    public Text timerLegendTextUI;
    public Image botonDerecho;
    public Image botonIzquierdo;


    private void Awake()
    {
        instanciaCompartida = this;
        ResetTimer();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        tiempoMaximo -= Time.deltaTime;
        timerTextUI.text = "" + tiempoMaximo.ToString("f0");
        if (timerTextUI.text.Equals("0"))
        {
            ControladorA3.instanciaCompartida.CambiarEstadoA3(estadoA3.tiempo_agotado);
        }

    }

    public void ResetTimer()
    {
        tiempoMaximo = 20.0f;
    }

    public void PauseTimer()
    {
        tiempoGuardado = tiempoMaximo;
        tiempoMaximo = -1.0f;
        MostrarTimer(false);
    }


    public void ResumeTimer()
    {
        tiempoMaximo = tiempoGuardado;
        MostrarTimer(true);
    }


    public void MostrarTimer(bool mostrarlo)
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
    }
}
