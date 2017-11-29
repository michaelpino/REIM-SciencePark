using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum estadoREIM
{
    inicio,
    vista1,
    vista2,
    intro_a1,
    intro_a2,
    intro_a3,
    a1,
    a2,
    a3
}


public class ControladorREIM : MonoBehaviour {

    public static ControladorREIM instanciaCompartida;

    public estadoREIM estadoActualREIM = estadoREIM.vista1;
    public estadoREIM ultimaVista;
    DatosSesion sesion = new DatosSesion();
  

    //***PERFIL USUARUI DESDE BD***
    public bool alumno_sexo = true;                //true = hombre ; false = mujer
    
    //***Instrucciones***
    
    bool mostradaIntruccionVista1yVista2 = false;
    bool aceptadaA1 = false;
    bool aceptadaA2 = false;
    bool aceptadaA3 = false;
    public AudioClip musicaDeFondo;
    public AudioClip instruccionesAudio;
    AudioSource fuenteAudio;


    //***Variables CANVAS***
    public Canvas botonera;
    public Canvas bienvenida;
    public Canvas instruccionesVista1yVista2;
    public Canvas vista1;
    public Canvas vista2;
    public Canvas intro_a1;
    //public Canvas a1;
    public Canvas intro_a2;
    //public Canvas a2;
    public Canvas intro_a3;
    //public Canvas a3;

    //***Variables GameObjects***
    public GameObject btnCambiarVista;
    public GameObject intro_a1_hombre;
    public GameObject intro_a1_mujer;
    public GameObject intro_a2_hombre;
    public GameObject intro_a2_mujer;
    public GameObject intro_a3_hombre;
    public GameObject intro_a3_mujer;

    private void Awake()
    {
        instanciaCompartida = this;
        VerOcultarInstrucciones();

    }




    public void CerrarSesion()
    {
        if (estadoActualREIM == estadoREIM.vista1 || estadoActualREIM == estadoREIM.vista2)
        {
            sesion.GuardarBD();

            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
        else
        {
            CambiarEstadoREIM(ultimaVista);
        }
    }


    public void CambiarVista()
    {
        if (estadoActualREIM == estadoREIM.vista1)
        {
            sesion.registrarCambioDeVista();
            CambiarEstadoREIM(estadoREIM.vista2);
        }
        else if (estadoActualREIM == estadoREIM.vista2)
        {
            sesion.registrarCambioDeVista();
            CambiarEstadoREIM(estadoREIM.vista1);
        }
    }


    public void IniciarActividad(int actividadAIniciar)
    {
        if(actividadAIniciar == 1)
        {
            if (aceptadaA1 == true)
            {
                CambiarEstadoREIM(estadoREIM.a1);
            }
            else
            {
                CambiarEstadoREIM(estadoREIM.intro_a1);
            }
        }
        else if (actividadAIniciar == 2)
        {
            if (aceptadaA2 == true)
            {
                CambiarEstadoREIM(estadoREIM.a2);
            }
            else
            {
                CambiarEstadoREIM(estadoREIM.intro_a2);
            }
        }
        else if (actividadAIniciar == 3)
        {
            if (aceptadaA3 == true)
            {
                CambiarEstadoREIM(estadoREIM.a3);
            }
            else
            {
                CambiarEstadoREIM(estadoREIM.intro_a3);
            }

        }
    }



    public void VerOcultarInstrucciones()
    {
        if (estadoActualREIM == estadoREIM.vista1 || estadoActualREIM == estadoREIM.vista2)
        {
            sesion.registrarInstruccionesLeidas();
            if (instruccionesVista1yVista2.enabled == false)
            {
                CambiarAudio(2);
                instruccionesVista1yVista2.enabled = true;
            }
            else
            {
                CambiarAudio(1);
                instruccionesVista1yVista2.enabled = false;
            }      
        }
    }


    public void AceptarInstrucciones(int actividadAceptada)
    {
        if(actividadAceptada == 1)
        {
            aceptadaA1 = true;
            IniciarActividad(1);
        }
        else if (actividadAceptada == 2)
        {
            aceptadaA2 = true;
            IniciarActividad(2);
        }
        else if (actividadAceptada == 3)
        {
            aceptadaA3 = true;
            IniciarActividad(3);
        }
    }

    public void CambiarEstadoREIM(estadoREIM nuevoEstado)
    {

        DeshabilitarVista();

        if (nuevoEstado == estadoREIM.vista1)
        { 
            vista1.enabled = true;          
            ultimaVista = estadoREIM.vista1;                //Se guarda la ultima vista utilizada, para volver a ella despues de una actividad
            if (mostradaIntruccionVista1yVista2 == false)   //Consulta si ya se han mostrado las instrucciones por primera vez
            {
                VerOcultarInstrucciones();
                mostradaIntruccionVista1yVista2 = true;
            }
        }
        else if (nuevoEstado == estadoREIM.vista2)
        {
            vista2.enabled = true;
            ultimaVista = estadoREIM.vista2;
        }
        else if (nuevoEstado == estadoREIM.intro_a1)
        {
            sesion.registrarIngresoIntroA1();
            intro_a1.enabled = true;
            if (alumno_sexo == true)
            {
                intro_a1_hombre.GetComponent<RawImage>().enabled = true;   //Aparece el avatar masculino
                intro_a1_mujer.GetComponent<RawImage>().enabled = false;
            }
            else
            {
                intro_a1_mujer.GetComponent<RawImage>().enabled = true;   //Aparece el avatar femenino
                intro_a1_hombre.GetComponent<RawImage>().enabled = false;
            }
            
        }
        else if (nuevoEstado == estadoREIM.a1)
        {
            sesion.registrarInicioA1();
            SceneManager.LoadScene(4, LoadSceneMode.Single);
        }
        else if (nuevoEstado == estadoREIM.intro_a2)
        {
            sesion.registrarIngresoIntroA2();
            intro_a2.enabled = true;
            if (alumno_sexo == true)
            {
                intro_a2_hombre.GetComponent<RawImage>().enabled = true;   //Aparece el avatar masculino
                intro_a2_mujer.GetComponent<RawImage>().enabled = false;
            }
            else
            {
                intro_a2_mujer.GetComponent<RawImage>().enabled = true;   //Aparece el avatar femenino
                intro_a2_hombre.GetComponent<RawImage>().enabled = false;
            }
        }
        else if (nuevoEstado == estadoREIM.a2)
        {
            sesion.registrarInicioA2();
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
        else if (nuevoEstado == estadoREIM.intro_a3)
        {
            sesion.registrarIngresoIntroA3();
            intro_a3.enabled = true;
            if (alumno_sexo == true)
            {
                intro_a3_hombre.GetComponent<RawImage>().enabled = true;   //Aparece el avatar masculino
                intro_a3_mujer.GetComponent<RawImage>().enabled = false;
            }
            else
            {
                intro_a3_mujer.GetComponent<RawImage>().enabled = true;   //Aparece el avatar femenino
                intro_a3_hombre.GetComponent<RawImage>().enabled = false;
            }
        }
        else if (nuevoEstado == estadoREIM.a3)
        {
            sesion.registrarInicioA3();
            SceneManager.LoadScene(3, LoadSceneMode.Single);
        }
        /*else if (nuevoEstado == estadoREIM.inicio)
        {          
            bienvenida.enabled = true;           
        }*/

        estadoActualREIM = nuevoEstado;
        VerOcultarBotonera();
    }
   

    void DeshabilitarVista ()
    {
        if(estadoActualREIM == estadoREIM.vista1)
        {
            vista1.enabled = false;
        }
        else if(estadoActualREIM == estadoREIM.vista2)
        {
            vista2.enabled = false;
        }
        else if (estadoActualREIM == estadoREIM.intro_a1)
        {
            intro_a1.enabled = false;
        }
        else if (estadoActualREIM == estadoREIM.intro_a2)
        {
            intro_a2.enabled = false;
        }
        else if (estadoActualREIM == estadoREIM.intro_a3)
        {
            intro_a3.enabled = false;
        }
    }


    void VerOcultarBotonera()
    {
        if (estadoActualREIM == estadoREIM.vista1)
        {
            botonera.enabled = true;
            if(ultimaVista != estadoREIM.vista2)
            {
                btnCambiarVista.GetComponent<RawImage>().enabled = true;   //Aparece el boton de Cambiar Vista
            }
        }
        else if (estadoActualREIM == estadoREIM.vista2)
        {
            botonera.enabled = true;
            if (ultimaVista != estadoREIM.vista1)
            {
                btnCambiarVista.GetComponent<RawImage>().enabled = true;   //Aparece el boton de Cambiar Vista
            }
        }
        else if (estadoActualREIM == estadoREIM.intro_a1)
        {
            botonera.enabled = false; 
        }
        else if (estadoActualREIM == estadoREIM.intro_a2)
        {
            botonera.enabled = false;
        }
        else if (estadoActualREIM == estadoREIM.intro_a3)
        {
            botonera.enabled = false;
        }
    }


    // Use this for initialization
	void Start () {
        //GameObject.DontDestroyOnLoad(this.gameObject);
        sesion = new DatosSesion();
        sesion.IniciaReestableceSesion();
        if (PlayerPrefs.GetString("sexoAlumno").Equals("true"))
        {
            alumno_sexo = true;
        }
        else
        {
            alumno_sexo = false;
        }
        fuenteAudio = GetComponent<AudioSource>();
        CambiarAudio(1);

        CambiarEstadoREIM(estadoREIM.vista1);
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
