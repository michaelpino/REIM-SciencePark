using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum estadoA3
{
    intro,
    juego,
    acierto,
    equivocacion,
    tiempo_agotado,
    instruccioes,
    fin
}


public class ControladorA3 : MonoBehaviour {

    public static ControladorA3 instanciaCompartida;

    public estadoREIM estadoActualREIM;
    public estadoREIM ultimaVista;
    public estadoA3 estadoActualA3;
    int cantidadRespuestasCorrectas = 0;
    int cantidadRespuestasIncorrectas = 0;
    int cantidadTiempoAgotado = 0;
    DatosDeIntentoA3 intento = new DatosDeIntentoA3();
    DatosDeActividadA3 actividad = new DatosDeActividadA3();
    string idActividad = "";

    //***PERFIL USUARIO DESDE BD***
    string idAlumno = "16385071";
    public bool alumno_sexo = true;                 //true = hombre ; false = mujer
    int dificultad = 10;                             //Facil = 10; Intermedio = 18 ; Dificil = 25

    //***Instrucciones***

    bool mostradaIntruccionA3 = false;
    public bool estabamosEnPausa = false;
    public AudioClip musicaDeFondo;
    public AudioClip instruccionesAudio;
    AudioSource fuenteAudio;


    //***Variables CANVAS***
    public Canvas botonera;
    public Canvas instruccionesA3;
    public Canvas GameOverPanel;
    public Canvas MensajesPanel;
    public Canvas InicioPanel;
    public Text respuestasCorrectas;
    public Text respuestasIncorrectas;
    public Text respuestasTiempoAgotado;
    public Text correcto;
    public Text incorrecto;
    public Text animalEscapado;
    //public Image paneltactilAnimal;

    //***Variables "SENSOR"***
    public Canvas sensorJuego;

    //***Variables GameObjects***


    private void Awake()
    {
        instanciaCompartida = this;
        botonera.enabled = true;   
    }


    // Use this for initialization
    void Start()
    {
        actividad.registrarInicio();
        actividad.SolicitarUsuarioYSesion();
        //idActividad = DateTime.Now.ToString();
        fuenteAudio = GetComponent<AudioSource>();
        CambiarAudio(1);
        CambiarEstadoA3(estadoA3.intro);
    }

    public void CerrarSesion()
    {
        if(estadoActualA3 == estadoA3.intro)
        {
            actividad.GuardarBD();
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        else if(estadoActualA3 == estadoA3.fin)
        {
            CambiarEstadoA3(estadoA3.intro);
        }
        else if (estadoActualA3 == estadoA3.juego)
        {
            intento.registrarRetirada();
            actividad.registrarRetirada();
            CambiarEstadoA3(estadoA3.fin);
        }
    }


    public void VerInstrucciones()
    {
        if(estadoActualA3 == estadoA3.juego)
        {
            estabamosEnPausa = true;
            ControladorDeAnimales.instanciaCompartida.OcultarAnimal();
            CambiarEstadoA3(estadoA3.instruccioes);
        }
        else if (estadoActualA3 == estadoA3.intro)
        {
            InicioPanel.enabled = false;
            CambiarEstadoA3(estadoA3.instruccioes);
        }  
    }


    public void CambiarEstadoA3(estadoA3 nuevoEstado)
    {
        
        animalEscapado.enabled = false;
        correcto.enabled = false;
        incorrecto.enabled = false;
        GameOverPanel.enabled = false;
        MensajesPanel.enabled = false;
        InicioPanel.enabled = false;
        //paneltactilAnimal.enabled = false;
        sensorJuego.enabled = false;
        Timer.instanciaCompartida.MostrarTimer(false);

        if (nuevoEstado == estadoA3.intro)
        {
            InicioPanel.enabled = true;
            Timer.instanciaCompartida.PauseTimer();
        }
        else if (nuevoEstado == estadoA3.juego)
        {
            //paneltactilAnimal.enabled = true;
            sensorJuego.enabled = true;
            if (estabamosEnPausa)
            {
                ControladorDeAnimales.instanciaCompartida.MostrarAnimal();
                Timer.instanciaCompartida.ResumeTimer();
                estabamosEnPausa = false;
               
            }
            else
            {
                Timer.instanciaCompartida.ResetTimer();
                Timer.instanciaCompartida.MostrarTimer(true);
                ControladorDeAnimales.instanciaCompartida.AgregarAnimal();
            }
            

        }
        else if (nuevoEstado == estadoA3.acierto)
        {
            intento.registrarAcierto();
            actividad.registrarCorrecto();
            MensajesPanel.enabled = true;
            correcto.enabled = true;
            cantidadRespuestasCorrectas++;
            ControladorDeAnimales.instanciaCompartida.RemoverAnimal();
            Timer.instanciaCompartida.PauseTimer();
        }
        else if (nuevoEstado == estadoA3.equivocacion)
        {
            intento.registrarEquivocacion();
            actividad.registrarIncorrecto();
            MensajesPanel.enabled = true;
            incorrecto.enabled = true;
            cantidadRespuestasIncorrectas++;
            ControladorDeAnimales.instanciaCompartida.RemoverAnimal();
            Timer.instanciaCompartida.PauseTimer();
        }
        else if (nuevoEstado == estadoA3.tiempo_agotado)
        {
            intento.registrarTiempoAgotado();
            MensajesPanel.enabled = true;
            animalEscapado.enabled = true;
            cantidadTiempoAgotado++;
            Timer.instanciaCompartida.PauseTimer();
            ControladorDeAnimales.instanciaCompartida.RemoverAnimal();

        }
        else if (nuevoEstado == estadoA3.instruccioes)
        {
            intento.registrarInstruccionesLeidas();
            actividad.registrarInstruccionesLeidas();
            Timer.instanciaCompartida.PauseTimer();
            //paneltactilAnimal.enabled = false;
            InstruccionesA3.instanciaCompartida.Instruccion1();
            CambiarAudio(2);
        }
        else if (nuevoEstado == estadoA3.fin)
        {
            actividad.registrarMarcador(cantidadRespuestasCorrectas);
            GameOverPanel.enabled = true;
            if (ControladorDeAnimales.instanciaCompartida.animalActual != null)
            {
                ControladorDeAnimales.instanciaCompartida.RemoverAnimal();
            }
            
            /*
            //Se comprueba si completó la actividad o se rindió
            if (ControladorDeAnimales.instanciaCompartida.QuedanMasAnimales() == true)
            {
                BaseDeDatosA3.instanciaCompartida.SeRindio();
            }else if (ControladorDeAnimales.instanciaCompartida.QuedanMasAnimales() == false)
            {
                BaseDeDatosA3.instanciaCompartida.LoTermino();
            }
            
            //Se comprueba si aprobó o reprobó la actividad
            if (((Double)cantidadRespuestasCorrectas / (Double)dificultad) > 0.7){
                BaseDeDatosA3.instanciaCompartida.ActividadAprobada();
            }
            else
            {
                BaseDeDatosA3.instanciaCompartida.ActividadReprobada();
            }
            */

            intento.GuardarBD(actividad.idActividad, actividad.nombreActividad,idAlumno);
            

            respuestasCorrectas.text = cantidadRespuestasCorrectas.ToString();
            respuestasIncorrectas.text = cantidadRespuestasIncorrectas.ToString();
            respuestasTiempoAgotado.text = cantidadTiempoAgotado.ToString();
        }

        
        estadoActualA3 = nuevoEstado;

    }
   

    public void EsCorrecto()
    {
        if (ControladorDeAnimales.instanciaCompartida.animalActual.id_habitat == ControladorDeAnimales.instanciaCompartida.portalActual)
        {
            intento.registrarEnvio(ControladorDeAnimales.instanciaCompartida.animalActual.nombre, ControladorDeAnimales.instanciaCompartida.animalActual.habitat, true);
            CambiarEstadoA3(estadoA3.acierto);
        }
        else if(ControladorDeAnimales.instanciaCompartida.animalActual.id_habitat != ControladorDeAnimales.instanciaCompartida.portalActual)
        {
            intento.registrarEnvio(ControladorDeAnimales.instanciaCompartida.animalActual.nombre, ControladorDeAnimales.instanciaCompartida.VerPortalActual(), false);
            CambiarEstadoA3(estadoA3.equivocacion);
        }  
    }

    public void VueltaAlJuego()
    {
        if (ControladorDeAnimales.instanciaCompartida.QuedanMasAnimales())
        {
            CambiarEstadoA3(estadoA3.juego);
        }
        else
        {
            CambiarEstadoA3(estadoA3.fin);
        }
        
    }

    public void ReiniciarPartida()
    {
        ControladorDeAnimales.instanciaCompartida.RandomizarListaDeAnimales(dificultad);
        //Destroy(intento);
        intento = new DatosDeIntentoA3();
        intento.registrarInicio();
        actividad.registrarIntento();


        //Se reinicia el marcador...
        cantidadRespuestasCorrectas = 0;
        cantidadRespuestasIncorrectas = 0;
        cantidadTiempoAgotado = 0;
        Timer.instanciaCompartida.PauseTimer();
        CambiarEstadoA3(estadoA3.juego);
    }

    public void CambiarDificultad(int nivel)        //Directamente accesible desde uLearnet
    {
        if (nivel == 1)
        {
            dificultad = 10;
        }
        else if (nivel == 2)
        {
            dificultad = 18;
        }
        else if(nivel == 3)
        {
            dificultad = 25;
        }
    }

    //Registra un toque en alguno de los sensores para detectar toques particulares en la pantalla
    public void registrarToqueSensor(int opcion)
    {
        if(opcion == 1)
        {
            actividad.registrarToqueFondo();
        }
        else if (opcion == 2)
        {
            actividad.registrarToqueBotonIzquierda();
        }
        else if (opcion == 3)
        {
            actividad.registrarToqueBotonDerecha();
        }
        else if (opcion == 4)
        {
            actividad.registrarToqueFondoBotonera();
        }
    }

    public void CambiarAudio(int opcion)
    {
        if (opcion == 1)
        {
            fuenteAudio.clip = musicaDeFondo;
            fuenteAudio.volume = 0.8f;
            fuenteAudio.Play();
        }
        else if (opcion == 2)
        {
            fuenteAudio.clip = instruccionesAudio;
            fuenteAudio.volume = 1f;
            fuenteAudio.Play();
        }
    }

}
