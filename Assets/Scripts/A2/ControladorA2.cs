using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum estadoA2
{
    intro,
    juego,
    acierto,
    equivocacion,
    tiempo_agotado,
    instrucciones,
    fin
}



public class ControladorA2 : MonoBehaviour {

    public static ControladorA2 instanciaCompartida;

    public estadoA2 estadoActualA2;

    int cantidadRespuestasCorrectas = 5;        //Recetas acertadas en todos los ingredientes
    int cantidadRespuestasIncorrectas = 0;      //Recetas incorrectas en al menos 1 ingrediente
    int cantidadTiempoAgotado = 0;
    int cantidadDePlatos = 3;                   //Cantidad de recetas para completar la actividad

    //***Instrucciones***
    bool mostradaIntruccionA2 = false;
    public bool estabamosEnPausa = false;
    public AudioClip musicaDeFondo;
    public AudioClip instruccionesAudio1;
    public AudioClip instruccionesAudio2;
    AudioSource fuenteAudio;

    //***Variables CANVAS***
    public Canvas instruccionesA2;
    public Canvas GameOverPanel;
    public Canvas MensajesPanel;
    public Canvas AgregarAlimentoPanel;
    public Canvas LetreroNombreAlimentoPanel;
    public Canvas InicioPanel;
    public Canvas alimentosEnBolsa;
    public Canvas ordenesChef;
    public Text textoChef;
    public Text correcto;
    public Text incorrecto;
    public Text nombreAlimento;


    public Text respuestasCorrectas;
    public Text respuestasIncorrectas;
    public Text respuestasTiempoAgotado;
    public Text AgregarAlimentoSi;
    public Text AgregarAlimentoNo;
    public Text AgregarAlimentoTexto;
    public Text comidaQuemada;
    public Text gameOverText;

    //***Variables Temporizador***
    Vector3[] nivelesIndicador = new Vector3[2] { new Vector3(-9.91f, -3.92f, 0), new Vector3(-9.91f, 2.00f, 0) };      //[0] = mínimo , [1] = máximo
    Vector3[] nivelesIndicador2 = new Vector3[2] { new Vector3(20.91f, -3.92f, 0), new Vector3(20.91f, 2.00f, 0) };
    public SpriteRenderer indicadorTimer;
    public SpriteRenderer indicadorTimer2;


    //***Variables "SENSOR"***
    public Canvas sensorDespensa;
    public Canvas sensorCocina;

    public DatosDeIntentoA2 intento = new DatosDeIntentoA2();
    public DatosDeActividadA2 actividad = new DatosDeActividadA2();


    private void Awake()
    {
        instanciaCompartida = this;
    }
    
    // Use this for initialization
    void Start () {
        actividad = new DatosDeActividadA2();
        actividad.registrarInicio();
        actividad.SolicitarUsuarioYSesion();
        fuenteAudio = GetComponent<AudioSource>();
        CambiarAudio(1);
        CambiarEstadoA2(estadoA2.intro);

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void CambiarEstadoA2(estadoA2 nuevoEstado)
    {
        InicioPanel.enabled = false;
        sensorCocina.enabled = false;
        sensorDespensa.enabled = false;
        MensajesPanel.enabled = false;
        comidaQuemada.enabled = false;
        correcto.enabled = false;
        incorrecto.enabled = false;
        GameOverPanel.enabled = false;
        ordenesChef.enabled = false;
        LetreroNombreAlimentoPanel.enabled = false;

        if (nuevoEstado == estadoA2.intro)
        {
            InicioPanel.enabled = true;
        }
        else if (nuevoEstado == estadoA2.juego)
        {
            //sensorCocina.enabled = true;
            ordenesChef.enabled = true;

            //sensorJuego.enabled = true;
            if (estabamosEnPausa)
            {
                TimerA2.instanciaCompartida.ResumeTimer();
                estabamosEnPausa = false;

            }
            else
            {
                TimerA2.instanciaCompartida.ResetTimer();
            }


        }
        else if (nuevoEstado == estadoA2.acierto)
        {
            FollowA2.instanciaCompartida.VolverCocina();
            cantidadRespuestasCorrectas++;
            if (cantidadRespuestasCorrectas + cantidadRespuestasIncorrectas + cantidadTiempoAgotado == cantidadDePlatos)
            {
                CambiarEstadoA2(estadoA2.fin);
            }
            else
            {
                MensajesPanel.enabled = true;
                correcto.text = "Muy bien! Entregaste todos los alimentos correctamente";
                correcto.enabled = true;
            }

            TimerA2.instanciaCompartida.PauseTimer();
        }
        else if (nuevoEstado == estadoA2.equivocacion)
        {
            FollowA2.instanciaCompartida.VolverCocina();
            cantidadRespuestasIncorrectas++;
            if (cantidadRespuestasCorrectas + cantidadRespuestasIncorrectas + cantidadTiempoAgotado == cantidadDePlatos)
            {
                CambiarEstadoA2(estadoA2.fin);
            }
            else
            {
                MensajesPanel.enabled = true;
                incorrecto.text = "No te quedan más intentos. Entregaste " + Bolsa.instanciaCompartida.cantidadDeAlimentos + " alimento(s) correctamente";
                incorrecto.enabled = true;
            }

            TimerA2.instanciaCompartida.PauseTimer();
        }
        else if (nuevoEstado == estadoA2.tiempo_agotado)
        {
            FollowA2.instanciaCompartida.VolverCocina();
            cantidadTiempoAgotado++;
            intento.registrarTiempoAgotado(cantidadRespuestasCorrectas + cantidadRespuestasIncorrectas + cantidadTiempoAgotado);
            MensajesPanel.enabled = true;
            comidaQuemada.text = "Se quemó la comida... Entregaste " + Bolsa.instanciaCompartida.cantidadDeAlimentos + " alimento(s) correctamente";
            comidaQuemada.enabled = true;
            if (cantidadRespuestasCorrectas + cantidadRespuestasIncorrectas + cantidadTiempoAgotado == cantidadDePlatos)
            {
                CambiarEstadoA2(estadoA2.fin);
            }

            TimerA2.instanciaCompartida.PauseTimer();
             

        }
        else if (nuevoEstado == estadoA2.instrucciones)
        {

            TimerA2.instanciaCompartida.PauseTimer();

            actividad.registrarInstruccionesLeidas();
            InstruccionesA2.instanciaCompartida.Instruccion1();
            CambiarAudio(2);
        }
        else if (nuevoEstado == estadoA2.fin)
        {
            TimerA2.instanciaCompartida.PauseTimer();

            actividad.registrarMarcador(intento.alimentosAcertadosTotal);
            GameOverPanel.enabled = true;
            
            //Se comprueba si completó la actividad o no se completó
            if (cantidadRespuestasCorrectas == 3)
            {
                gameOverText.text = "Muy bien! Entregaste todos los platos.";
            }
            else
            {
                gameOverText.text = "Terminó el juego!"; 
            }

            intento.GuardarBD(actividad.idActividad, actividad.nombreActividad, actividad.idAlumno);

            respuestasCorrectas.text = cantidadRespuestasCorrectas.ToString();
            respuestasIncorrectas.text = cantidadRespuestasIncorrectas.ToString();
            respuestasTiempoAgotado.text = cantidadTiempoAgotado.ToString();
        }
        estadoActualA2 = nuevoEstado;
    }


    public void ReiniciarPartida()
    {       
        if (cantidadRespuestasCorrectas + cantidadRespuestasIncorrectas >= 3)
        {
            //Se reinicia el marcador y comienza una nueva partida...
            cantidadRespuestasCorrectas = 0;
            cantidadRespuestasIncorrectas = 0;
            cantidadTiempoAgotado = 0;
            //BaseDeDatosA2.instanciaCompartida.ActividadIniciada();
            intento = new DatosDeIntentoA2();
            intento.registrarInicio();
            actividad.registrarIntento();

        }
        Despensa.instanciaCompartida.ReiniciarDespensa();
        textoChef.text = Bolsa.instanciaCompartida.GenerarOrdenDelChel(cantidadRespuestasCorrectas + cantidadRespuestasIncorrectas + cantidadTiempoAgotado + 1);

        CambiarEstadoA2(estadoA2.juego);
    }


    public void CambiarHabitacion()
    {
        FollowA2.instanciaCompartida.CambiarDeHabitacion();
        /*if (FollowA2.instanciaCompartida.EstamosEnLaCocina())
        {
            sensorCocina.enabled = true;
            sensorDespensa.enabled = false;
        }else if (!FollowA2.instanciaCompartida.EstamosEnLaCocina())
        {
            sensorCocina.enabled = false;
            sensorDespensa.enabled = true;
        }*/
    }


    public void CerrarSesion()
    {
        if (estadoActualA2 == estadoA2.intro)
        {
            actividad.GuardarBD();
            //BaseDeDatosA2.instanciaCompartida.SeCerroSesionA2();
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        else if (estadoActualA2 == estadoA2.fin)
        {

            if (cantidadRespuestasCorrectas + cantidadRespuestasIncorrectas + cantidadTiempoAgotado != 3)   //Se comprueba si se rindió
            {
                BaseDeDatosA2.instanciaCompartida.SeRindio();
            }
            CambiarEstadoA2(estadoA2.intro);
        }
        else if (estadoActualA2 == estadoA2.juego)
        {
            intento.registrarRetirada();
            actividad.registrarRetirada();
            CambiarEstadoA2(estadoA2.equivocacion);
        }
        else
        {
            //CambiarEstadoA2(estadoA2.intro);
            CambiarEstadoA2(estadoA2.fin);
        }
    }

    //Funcion que permite ir disminuyendo el tiempo en pantalla
    public void DisminuirTiempo(float disminucion)
    {
        if(estadoActualA2 == estadoA2.juego)
        {
            indicadorTimer.transform.position = new Vector3(indicadorTimer.transform.position.x, (-3.92f + ((nivelesIndicador[1][1] - nivelesIndicador[0][1]) * disminucion)), indicadorTimer.transform.position.z);
            indicadorTimer2.transform.position = new Vector3(indicadorTimer2.transform.position.x, (-3.92f + ((nivelesIndicador[1][1] - nivelesIndicador[0][1]) * disminucion)), indicadorTimer2.transform.position.z);
        }
    }


    /*public void ConfirmacionAgregarAlimento(Alimento alimento)
    {
        AgregarAlimentoPanel.enabled = true;
        AgregarAlimentoTexto.text = "Deseas agregar " + alimento.GetNombreMasArticulo() + "?";
        AgregarAlimentoTexto.enabled = true;
        AgregarAlimentoSi.enabled = true;
        AgregarAlimentoNo.enabled = true;
    }

    public void QuitarConfirmacionAgregarAlimento()     //Borra los mensajes sobre agregar alimento
    {
        AgregarAlimentoPanel.enabled = false;
        AgregarAlimentoTexto.enabled = false;
        AgregarAlimentoSi.enabled = false;
        AgregarAlimentoNo.enabled = false;
        //alimentosEnBolsa.enabled = false;
    }

    public void SiAgregarAlimento()
    {
        QuitarConfirmacionAgregarAlimento();
        Bolsa.instanciaCompartida.AnalizarRespuesta();
        //bool respuesta = 
        if (respuesta)
        {
            Debug.Log("Muy bien");
            CambiarEstadoA2(estadoA2.acierto);
        }
        else if (!respuesta)
        {
            Debug.Log("Naaain!");
            CambiarEstadoA2(estadoA2.equivocacion);
        }
    }*/

    public void MostrarNombreAlimento (string nombreAlimentoPresionado)
    {
        nombreAlimento.text = nombreAlimentoPresionado;
    }


    public void VerInstrucciones()
    {
        if (estadoActualA2 == estadoA2.juego)
        {
            estabamosEnPausa = true;
            intento.registrarInstruccionesLeidas();
            CambiarEstadoA2(estadoA2.instrucciones);
        }else if (estadoActualA2 == estadoA2.intro)
        {
            CambiarEstadoA2(estadoA2.instrucciones);
        }
    }


    //Registra un toque en alguno de los sensores para detectar toques particulares en la pantalla
    public void registrarToqueSensor(int opcion)
    {
        if (opcion == 1)
        {
            actividad.registrarToqueFondo();
        }
        else if (opcion == 2)
        {
            actividad.registrarToqueCambioHabitacion();
        }
        else if (opcion == 3)
        {
            actividad.registrarToqueFondoBotonera();
        }
        /*else if (opcion == 4)
        {
            actividad.registrarToqueFondoBotonera();
        }*/
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
            fuenteAudio.clip = instruccionesAudio1;
            fuenteAudio.volume = 1f;
            fuenteAudio.Play();
        }
        else if (opcion == 3)
        {
            fuenteAudio.clip = instruccionesAudio2;
            fuenteAudio.volume = 1f;
            fuenteAudio.Play();
        }
    }
}
