using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstruccionesA1 : MonoBehaviour {

    public static InstruccionesA1 instanciaCompartida;

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
        /*if (!instruccionesInicialesMostradas)
        {
            //Despensa.instanciaCompartida.ReiniciarDespensaInstrucciones();
        }
        if (!FollowA2.instanciaCompartida.EstamosEnLaCocina())
        {
            //FollowA2.instanciaCompartida.CambiarDeHabitacionIntrucciones();
        }*/
        Instrucciones1.enabled = true;
    }


    /*public void Instruccion2()
    {
        Instrucciones1.enabled = false;
        Instrucciones2.enabled = true;
    }


    public void Instruccion3()
    {
        Instrucciones2.enabled = false;
        Instrucciones3.enabled = true;
    }


    public void Instruccion4()
    {
        Instrucciones3.enabled = false;
        Instrucciones4.enabled = true;
    }*/

    public void FinalizarInstrucciones()
    {
        //Instrucciones4.enabled = false;
        Instrucciones1.enabled = false;
        //if (instruccionesInicialesMostradas == true)
        if (ControladorA1.instanciaCompartida.estabamosEnPausa)
        {
            ControladorA1.instanciaCompartida.CambiarEstadoA1(estadoA1.juego);
        }
        else
        {
            //instruccionesInicialesMostradas = true;
            ControladorA1.instanciaCompartida.CambiarEstadoA1(estadoA1.intro);
        }
    }

}
