using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum estadoA1
{
    intro,
    juego,
    instrucciones,
    fin
}

public class ControladorA1 : MonoBehaviour {

    public static ControladorA1 instanciaCompartida;
    public estadoA1 estadoActualA1;

    int estadioDePlanta = 1;                   //Estadio máximo al que llegó la planta durante el juego
    public List<SpriteRenderer> listaDePlantas;

    int cantidadFertilizacionesCorrectas = 0;        
    int cantidadFertilizacionesIncorrectas = 0;      
    //int cantidadInsectosEliminados = 0;

    //***Instrucciones***
    public bool estabamosEnPausa = false;

    //***Variables Temporizador***
    public SpriteRenderer indicadorAgua;
    public SpriteRenderer luz;
    public SpriteRenderer indicadorLuz;
    public Ubicador carrilIzquierdo;
    public Ubicador carrilCentral;
    public Ubicador carrilDerecho;
    public Ubicador aguaMax;
    //public Ubicador aguaOKMax;
    public Ubicador aguaMid;
    //public Ubicador aguaOKMin;
    public Ubicador aguaMin;
    public Ubicador luzMax;
    //public Ubicador luzOKMax;
    public Ubicador luzMid;
    //public Ubicador luzOKMin;
    public Ubicador luzMin;
    
    float[] nivelesAgua = { 0, 0, 0, 0, 0 };        //[0] = mínimo , [1] = mínimo aceptable, [2] = medio, [3] = máximo aceptable, [4] = máximo
    float[] nivelesLuz = { 0, 0, 0, 0, 0 };         //[0] = mínimo , [1] = mínimo aceptable, [2] = medio, [3] = máximo aceptable, [4] = máximo

    //***Variables CANVAS***
    public Canvas InicioPanel;
    public Canvas GameOverPanel;
    public Canvas MensajesPanel;
    public Text mensajeTexo;
    public Text gameOverText;
    public Text fertilizacionesCorrectas;
    public Text fertilizacionesIncorrectas;
    public Text faseMaxPlanta;

    //***Variables DB***
    public DatosDeIntentoA1 intento = new DatosDeIntentoA1();
    public DatosDeActividadA1 actividad = new DatosDeActividadA1();


    private void Awake()
    {
        instanciaCompartida = this;       
    }


    // Use this for initialization
    void Start () {

        actividad = new DatosDeActividadA1();
        actividad.registrarInicio();
        actividad.SolicitarUsuarioYSesion();

        EstablecerNivelesAguaLuz();
        ReestablecerNiveles();
        CambiarEstadoA1(estadoA1.intro);
    }
	


    public void CambiarEstadoA1(estadoA1 nuevoEstado)
    {
        InicioPanel.enabled = false;
        GameOverPanel.enabled = false;
        MensajesPanel.enabled = false;

        if (nuevoEstado == estadoA1.intro)
        {
            InicioPanel.enabled = true;
        }
        else if (nuevoEstado == estadoA1.juego)
        {
            MensajesPanel.enabled = true;
            if (estabamosEnPausa)
            {
                TimerA1.instanciaCompartida.ResumeTimer();
                estabamosEnPausa = false;
            }
            else
            {
                mensajeTexo.text = "";
            }
        }
        else if (nuevoEstado == estadoA1.instrucciones)
        {

            TimerA1.instanciaCompartida.PauseTimer();
            //BaseDeDatosA1.instanciaCompartida.VistoInstrucciones();
            intento.registrarInstruccionesLeidas();
            actividad.registrarInstruccionesLeidas();
            InstruccionesA1.instanciaCompartida.Instruccion1();
        }
        else if (nuevoEstado == estadoA1.fin)
        {
            Macetero.instanciaCompartida.ReiniciarPosicion();
            TimerA1.instanciaCompartida.PauseTimer();
            GameOverPanel.enabled = true;
            cantidadFertilizacionesCorrectas = Planta.instanciaCompartida.GetNivelFertilizacion();
            cantidadFertilizacionesIncorrectas = Planta.instanciaCompartida.GetNivelNOFertilizacion();

            intento.GuardarBD(actividad.idActividad, actividad.nombreActividad, actividad.idAlumno);
            //intento.GuardarBD("FECHA/ID AQUI", "A1", "16385071");
            actividad.registrarMarcador(Planta.instanciaCompartida.GetNivelFertilizacion());

            //Se comprueba si completó la actividad o no se completó
            if (Planta.instanciaCompartida.GetNivelFertilizacion() == 20)
            {
                BaseDeDatosA1.instanciaCompartida.ActividadAprobada();
                gameOverText.text = "¡Muy bien! Tu planta dió muchos frutos";
            }
            else
            {
                BaseDeDatosA1.instanciaCompartida.ActividadReprobada();
                gameOverText.text = "Terminó el juego!";
            }

            fertilizacionesCorrectas.text = cantidadFertilizacionesCorrectas.ToString();
            fertilizacionesIncorrectas.text = cantidadFertilizacionesIncorrectas.ToString();
            faseMaxPlanta.text = Planta.instanciaCompartida.GetFasePlanta().ToString();
        }

        estadoActualA1 = nuevoEstado;
    }


    public void CerrarSesion()
    {
        if(estadoActualA1 != estadoA1.instrucciones)
        {
            if (estadoActualA1 == estadoA1.intro)
            {
                actividad.GuardarBD();
                //BaseDeDatosA1.instanciaCompartida.SeCerroSesionA1();
                SceneManager.LoadScene(1, LoadSceneMode.Single);
            }
            else if (estadoActualA1 == estadoA1.fin)
            {
                CambiarEstadoA1(estadoA1.intro);
            }
            else if (estadoActualA1 == estadoA1.juego)
            {
                //BaseDeDatosA1.instanciaCompartida.SeRindio();
                intento.registrarRetirada();
                actividad.registrarRetirada();
                CambiarEstadoA1(estadoA1.fin);
            }
        }       
    }


    public void ReiniciarPartida()
    {
        ReestablecerNiveles();
        intento = new DatosDeIntentoA1();
        intento.registrarInicio();
        actividad.registrarIntento();
        Planta.instanciaCompartida.ReiniciarPlanta();
        TimerA1.instanciaCompartida.ResetTimer();
        //BaseDeDatosA1.instanciaCompartida.ActividadIniciada();
        CambiarFasePlanta(0);

        CambiarEstadoA1(estadoA1.juego);
    }



    //Funcion que permite ir disminuyendo el tiempo en pantalla
    public void DisminuirHumedad(float cambioHumedad, float cambioLuz)
    {
        if (estadoActualA1 == estadoA1.juego)
        {
            indicadorAgua.transform.position = new Vector3(indicadorAgua.transform.position.x, (nivelesAgua[4] - ((nivelesAgua[4] - nivelesAgua[0]) * cambioHumedad)), indicadorAgua.transform.position.z);
            indicadorLuz.transform.position = new Vector3((nivelesLuz[4] - ((nivelesLuz[4] - nivelesLuz[0]) * cambioLuz)), indicadorLuz.transform.position.y , indicadorLuz.transform.position.z);
        }
    }


    //Captura las posiciones claves de cada indicador de las barras de luz y agua
    void EstablecerNivelesAguaLuz()
    {
        nivelesAgua[0] = aguaMin.GetUbicacion().y;
        //nivelesAgua[1] = aguaOKMin.GetUbicacion().y;
        nivelesAgua[2] = aguaMid.GetUbicacion().y;
        //nivelesAgua[3] = aguaOKMax.GetUbicacion().y;
        nivelesAgua[4] = aguaMax.GetUbicacion().y;

        nivelesLuz[0] = luzMin.GetUbicacion().x;
        //nivelesLuz[1] = luzOKMin.GetUbicacion().x;
        nivelesLuz[2] = luzMid.GetUbicacion().x;
        //nivelesLuz[3] = luzOKMax.GetUbicacion().x;
        nivelesLuz[4] = luzMax.GetUbicacion().x;
    }


    //Reestablece los indicadores a sus niveles medios
    void ReestablecerNiveles()
    {
        indicadorAgua.GetComponent<SpriteRenderer>().transform.position = new Vector3 (indicadorAgua.GetComponent<SpriteRenderer>().transform.position.x, nivelesAgua[2], indicadorAgua.GetComponent<SpriteRenderer>().transform.position.z);
        indicadorLuz.GetComponent<SpriteRenderer>().transform.position = new Vector3(nivelesLuz[2], indicadorLuz.GetComponent<SpriteRenderer>().transform.position.y, indicadorLuz.GetComponent<SpriteRenderer>().transform.position.z);
    }


    //Se mueve de posicion la imagen de la luz y se cambia el sentido de la luz
    public void MoverLuz(int posicion)
    {
        //Se mueve la imagen de luz
        if (posicion == 1)
        {
            //luz.transform.position = carrilIzquierdo.transform.position;
            //luz.transform.position.Set(carrilIzquierdo.transform.position.x, carrilIzquierdo.transform.position.y, luz.transform.position.z);
            luz.transform.position = new Vector3(carrilIzquierdo.transform.position.x, carrilIzquierdo.transform.position.y + 2f, 0f);
        }
        else if (posicion == 2)
        {
            //luz.transform.position = carrilCentral.transform.position;
            //luz.transform.position.Set(carrilCentral.transform.position.x, carrilCentral.transform.position.y, luz.transform.position.z);
            luz.transform.position = new Vector3(carrilCentral.transform.position.x, carrilCentral.transform.position.y + 2f, 0f);
        }
        else if (posicion == 3)
        {
            //luz.transform.position = carrilDerecho.transform.position;
            //luz.transform.position.Set(carrilDerecho.transform.position.x, carrilDerecho.transform.position.y, luz.transform.position.z);
            luz.transform.position = new Vector3(carrilDerecho.transform.position.x, carrilDerecho.transform.position.y + 2f, 0f);
        }

        //Se cambia el sentido de la luz
        if (Planta.instanciaCompartida.GetUbicacion() == posicion)
        {
            Macetero.instanciaCompartida.transform.position = Macetero.instanciaCompartida.ubicacion;
            TimerA1.instanciaCompartida.CambiarSentidoLuz(1);
        }
        else
        {
            TimerA1.instanciaCompartida.CambiarSentidoLuz(-1);
        }
    }

    
    //Recibe desde Planta que fase tiene la planta, y acorde a eso cambia su Sprite
    public void CambiarFasePlanta(int fase)
    {
        if (fase > 3)
        {
            Macetero.instanciaCompartida.GetComponent<SpriteRenderer>().sprite = listaDePlantas[0].sprite;
        }
        else
        {
            Macetero.instanciaCompartida.GetComponent<SpriteRenderer>().sprite = listaDePlantas[fase].sprite;
        }
    }


    public void VerInstrucciones()
    {
        if (estadoActualA1 == estadoA1.juego)
        {
            estabamosEnPausa = true;
            CambiarEstadoA1(estadoA1.instrucciones);
        }else if(estadoActualA1 == estadoA1.intro)
        {
            CambiarEstadoA1(estadoA1.instrucciones);
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
            actividad.registrarToqueBarrasDeNivel();
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

}
