using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstruccionesA3 : MonoBehaviour {

    public static InstruccionesA3 instanciaCompartida;

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
        Instrucciones1.enabled = false;
        //if (instruccionesInicialesMostradas == true)
        if (ControladorA3.instanciaCompartida.estabamosEnPausa) 
        {
            ControladorA3.instanciaCompartida.CambiarEstadoA3(estadoA3.juego);
        }
        else
        {
            instruccionesInicialesMostradas = true;
            ControladorA3.instanciaCompartida.CambiarEstadoA3(estadoA3.intro);
        }
        ControladorA3.instanciaCompartida.CambiarAudio(1);
    }

}
