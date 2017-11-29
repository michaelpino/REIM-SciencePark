using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planta : MonoBehaviour {

    public static Planta instanciaCompartida;

    int ubicacionPlanta = 2;                //1 = Izquierda, 2 = centro, 3 = derecha
    public int nivelFertilizacion = 0;      //Desde 0 a 15
    public int nivelNOFertilizacion = 0;
    int faseDeCrecimiento = 0;              //0 = Inicial, 1 = Crecimiento, 2 = Floracion, 3 = completado

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

    // Indica el carril en que se encuentra el macetero, y se comprueba si esta recibe luz
    public void SetUbicacion(int nuevaUbicacion)
    {
        ubicacionPlanta = nuevaUbicacion;
        if(ubicacionPlanta == Luz.instanciaCompartida.GetUbicacionActual())
        {
            TimerA1.instanciaCompartida.CambiarSentidoLuz(1);
        }
        else
        {
            TimerA1.instanciaCompartida.CambiarSentidoLuz(-1);
        }
    }


    public int GetUbicacion()
    {
        return ubicacionPlanta;
    }


    public int GetNivelFertilizacion()
    {
        return nivelFertilizacion;
    }

    public int GetNivelNOFertilizacion()
    {
        return nivelNOFertilizacion;
    }


    public int GetFasePlanta()
    {
        return faseDeCrecimiento;
    }


    public void AgregarFertilizante()
    {
        if (TimerA1.instanciaCompartida.NivelCorrectoHumedad())
        {
            if(TimerA1.instanciaCompartida.NivelCorrectoLuz())
            {
                nivelFertilizacion++;
                ControladorA1.instanciaCompartida.intento.registrarFertilizanteAgregado(true);
                ControladorA1.instanciaCompartida.mensajeTexo.text = "¡Muy bien! Faltan " + (20-nivelFertilizacion) + " más para que tu planta dé fruto";
                ControladorA1.instanciaCompartida.CambiarFasePlanta(faseDeCrecimiento);                   
                faseDeCrecimiento = nivelFertilizacion / 5;
                //Debug.Log(faseDeCrecimiento);
                if (faseDeCrecimiento > 3)
                {
                    ControladorA1.instanciaCompartida.CambiarEstadoA1(estadoA1.fin);
                }
            }
            else
            {
                ControladorA1.instanciaCompartida.mensajeTexo.text = "Revisa el nivel de luz de tu planta";
                ControladorA1.instanciaCompartida.intento.registrarFertilizanteAgregado(false);
                nivelNOFertilizacion++;
            }
        }
        else
        {
            ControladorA1.instanciaCompartida.mensajeTexo.text = "Revisa el nivel de agua de tu planta";
            ControladorA1.instanciaCompartida.intento.registrarFertilizanteAgregado(false);
            nivelNOFertilizacion++;
        }
    }


    public void ReiniciarPlanta()
    {

        ubicacionPlanta = 2;
        
        nivelFertilizacion = 0;
        nivelNOFertilizacion = 0;
        faseDeCrecimiento = 0;
    }



}
