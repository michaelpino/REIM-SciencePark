using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstruccionesA2 : MonoBehaviour {

    public static InstruccionesA2 instanciaCompartida;

    bool instruccionesInicialesMostradas = false;

    //***Variables CANVAS***
    public Canvas Instrucciones1;
    public Canvas Instrucciones2;
    public Canvas Instrucciones3;
    public Canvas Instrucciones4;

    private void Awake()
    {
        instanciaCompartida = this;
    }


    public void Instruccion1()
    {
        if (!instruccionesInicialesMostradas)
        {
            Despensa.instanciaCompartida.ReiniciarDespensaInstrucciones();
        }
        if (!FollowA2.instanciaCompartida.EstamosEnLaCocina())
        {
            FollowA2.instanciaCompartida.CambiarDeHabitacionIntrucciones();
        }
        Instrucciones1.enabled = true;
    }


    public void Instruccion2()
    {
        ControladorA2.instanciaCompartida.CambiarAudio(3);
        Instrucciones1.enabled = false;
        FollowA2.instanciaCompartida.CambiarDeHabitacionIntrucciones();
        Instrucciones2.enabled = true;
    }


    /*public void Instruccion3()
    {
        Instrucciones2.enabled = false;
        Instrucciones3.enabled = true;
    }


    public void Instruccion4()
    {
        Instrucciones3.enabled = false;
        FollowA2.instanciaCompartida.CambiarDeHabitacionIntrucciones();
        Instrucciones4.enabled = true;
    }*/

    public void FinalizarInstrucciones()
    {
        Instrucciones2.enabled = false;
        FollowA2.instanciaCompartida.CambiarDeHabitacionIntrucciones();
        //if (instruccionesInicialesMostradas == true)
        if (ControladorA2.instanciaCompartida.estabamosEnPausa)
        {
            ControladorA2.instanciaCompartida.CambiarEstadoA2(estadoA2.juego);
        }
        else
        {
            instruccionesInicialesMostradas = true;
            ControladorA2.instanciaCompartida.CambiarEstadoA2(estadoA2.intro);
        }
        ControladorA2.instanciaCompartida.CambiarAudio(1);
    }
}
