using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolsa : MonoBehaviour {

    public static Bolsa instanciaCompartida;

    public bool mostrandoBolsa = false;

    public int cantidadDeAlimentos = 0;
    static int cantidadDeAlimentosCorrectos = 3;                                //Valor de alimentos que serán requeridos al estudiante
    public int intentos = 0;                                                           //Valor de intentos de agregar alimentos a la bolsa
    int maximoDeIntentos = cantidadDeAlimentosCorrectos * 2;
    List<Alimento> listaDeAlimentos = new List<Alimento>();                     //Lista que contiene indices randomizados para mostrar animales
    List<Alimento> respuestasCorrectas = new List<Alimento>();                  //Lista que contiene los nombres de los alimentos elegidos como correctos por el chef
    Alimento alimentoTemporal = new Alimento();                                 //Alimento en espera de que se confirme que se agregará a la bolsa
    Vector3[] posicionAlimentosEnBolsa = new Vector3[5]{ new Vector3(14.3f, -0.3f, 0f), new Vector3(14.3f + 2.5f, -0.3f, 0f), new Vector3(14.3f + 5f, -0.3f, 0f), new Vector3(14.3f + 7.5f, -0.3f, 0f),
                                         new Vector3(14.3f + 10.0f, -0.3f, 0f)};
    public int platoActual = 0;

    public int CuantosAlimentosHay()
    {
        return listaDeAlimentos.Count;
    }

    public void AgregarAlimento(Alimento nuevoAlimento)
    {
        listaDeAlimentos.Add(nuevoAlimento); 
    }

    /*public void EliminarAlimento(Alimento nuevoAlimento)
    {
        Debug.Log("Se eliminó " + nuevoAlimento.GetNombreMasArticulo() + " a la bolsa");
        listaDeAlimentos.Remove(nuevoAlimento);
    }*/


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Awake()
    {
        instanciaCompartida = this;
    }

    void OnTriggerEnter2D(Collider2D otroObjeto)
    {
        if(otroObjeto.gameObject.tag == ("AlimentoA2"))
        {
            alimentoTemporal = otroObjeto.GetComponent<Alimento>();
            Bolsa.instanciaCompartida.AnalizarRespuesta();
            //ControladorA2.instanciaCompartida.ConfirmacionAgregarAlimento(alimentoTemporal);

            //otroObjeto.gameObject.SetActive(false);       //Se deshabilita el GameObject
        }
        
    }


    //Revisa si el alimento consultado esta en la lista de alimentos correctos
    public void AnalizarRespuesta()             
    {
        intentos++;
        if (respuestasCorrectas.Contains(alimentoTemporal))
        {
            cantidadDeAlimentos++;
            ControladorA2.instanciaCompartida.intento.registrarEnvio(platoActual, alimentoTemporal.GetNombre(), true);
            //BaseDeDatosA2.instanciaCompartida.RegistrarAciertoErrorIngrediente(alimentoTemporal.GetNombre(), true);
            AgregarAlimento(alimentoTemporal);
            if (listaDeAlimentos.Count == cantidadDeAlimentosCorrectos)
            {
                ControladorA2.instanciaCompartida.CambiarEstadoA2(estadoA2.acierto);
                listaDeAlimentos.Clear();
                ReiniciarIntentos();
            }
            
            alimentoTemporal.gameObject.SetActive(false);
            alimentoTemporal.SetEnBolsa();
            alimentoTemporal.transform.position = posicionAlimentosEnBolsa[CuantosAlimentosHay()];         //Se ubica espacialmente el alimento en la bolsa

            ControladorA2.instanciaCompartida.nombreAlimento.text = "Acertaste! Quedan " + (maximoDeIntentos-intentos) + " intento(s)";;

        }
        else
        {
            ControladorA2.instanciaCompartida.intento.registrarEnvio(platoActual, alimentoTemporal.GetNombre(), false);
            //BaseDeDatosA2.instanciaCompartida.RegistrarAciertoErrorIngrediente(alimentoTemporal.GetNombre(), false);
            ControladorA2.instanciaCompartida.nombreAlimento.text = "Ingrediente equivocado. Quedan " + (maximoDeIntentos-intentos) + " intento(s)";
        }

        if(intentos >= maximoDeIntentos)        //Si se alcanzó el maximo de intentos
        {
            ControladorA2.instanciaCompartida.CambiarEstadoA2(estadoA2.equivocacion);
            listaDeAlimentos.Clear();
            ReiniciarIntentos();
        }
    }

    public void GenerarRespuestasCorrectas()        //Genera aleatoriamente "n" respuesas correctas
    {
        cantidadDeAlimentos = 0;
        respuestasCorrectas.Clear();
        ReiniciarIntentos();
        System.Random rnd = new System.Random();                        //Se crea el objeto que entregara el numero aleatorio. OJO:Se utiliza System.Random en vez de Random solo, que usa el random de Unity, el cual no tiene Next() como metodo
        int alimentoCorrecto;                                             //Variable que almacenara momentaneamente el index que se quiere agregar en desorden
        List<Alimento> listaDeAlimentosTotal = Despensa.instanciaCompartida.GetAlimentosEnDespensa();

        while (respuestasCorrectas.Count < cantidadDeAlimentosCorrectos)
        {
            alimentoCorrecto = rnd.Next(0, listaDeAlimentosTotal.Count);                          //Se guarda el alimento candidato a agregar
            if (!respuestasCorrectas.Contains(listaDeAlimentosTotal[alimentoCorrecto]))           //Se comprueba que el alimento no haya sido agregado
            {
                respuestasCorrectas.Add(listaDeAlimentosTotal[alimentoCorrecto]);
                //BaseDeDatosA2.instanciaCompartida.RegistrarIngredienteRequerido(listaDeAlimentosTotal[alimentoCorrecto].GetNombre());
                
            }
        }
        
    }       

    public Alimento GetAlimentoTemp()
    {
        return alimentoTemporal;
    }

    void OnMouseDown()
    {
        if (mostrandoBolsa)
        {
            Despensa.instanciaCompartida.MostrarAlimentos();
        }
        else
        {
            Despensa.instanciaCompartida.OcultarAlimentos();
            ControladorA2.instanciaCompartida.alimentosEnBolsa.enabled = true;
            foreach (Alimento alimento in listaDeAlimentos)
            {
               alimento.gameObject.SetActive(true);
            }
            mostrandoBolsa = true;
        }
        
    }


    public void OcultarAlimentos()               //Oculta los alimentos disponibles en la despensa
    {
        ControladorA2.instanciaCompartida.alimentosEnBolsa.enabled = false;
        foreach (Alimento alimento in listaDeAlimentos)
        {
            alimento.gameObject.SetActive(false);
        }

        mostrandoBolsa = false;
    }

    public bool EstaAlimento(Alimento alimento)
    {
        return listaDeAlimentos.Contains(alimento);
    }


    public string GenerarOrdenDelChel(int numeroDePlato)
    {
        platoActual = numeroDePlato;
        ControladorA2.instanciaCompartida.nombreAlimento.text = "Tienes " + (maximoDeIntentos - intentos) + " intentos";
        ControladorA2.instanciaCompartida.intento.alimentosCorrectos(numeroDePlato, respuestasCorrectas[0].GetNombre(), respuestasCorrectas[1].GetNombre(), respuestasCorrectas[2].GetNombre());
        string oracionCompleta = "";
        if (numeroDePlato == 1)
        {
            
            /*oracionCompleta = "En primer lugar me gustaría <color=#ff00ffff>" + respuestasCorrectas[0].GetDescriptorAlimento() + "</color>";
            oracionCompleta += ". Luego quiero agregar <color=#ff0000ff>" + respuestasCorrectas[1].GetDescriptorAlimento() + "</color>";
            oracionCompleta += ". Y como tercer ingrediente <color=#0000ffff>" + respuestasCorrectas[2].GetDescriptorAlimento() + "</color>";*/
            oracionCompleta = "Plato 1:\n<color=#ff00ffff>- " + respuestasCorrectas[0].GetDescriptorAlimento() + "</color>" + "\n";
            oracionCompleta += "<color=#ff0000ff>- " + respuestasCorrectas[1].GetDescriptorAlimento() + "</color>" + "\n";
            oracionCompleta += "<color=#0000ffff>- " + respuestasCorrectas[2].GetDescriptorAlimento() + "</color>" + "\n";
            return oracionCompleta;
        }
        else if (numeroDePlato == 2)
        {
            /*oracionCompleta = "Para el segundo plato necesito <color=#ff00ffff>" + respuestasCorrectas[0].GetDescriptorAlimento() + "</color>";
            oracionCompleta += ". Además, deberias traerme <color=#ff0000ff>" + respuestasCorrectas[1].GetDescriptorAlimento() + "</color>";
            oracionCompleta += ". Y junto a todo esto <color=#0000ffff>" + respuestasCorrectas[2].GetDescriptorAlimento() + "</color>";*/
            oracionCompleta = "Plato 2:\n<color=#ff00ffff>- " + respuestasCorrectas[0].GetDescriptorAlimento() + "</color>" + "\n";
            oracionCompleta += "<color=#ff0000ff>- " + respuestasCorrectas[1].GetDescriptorAlimento() + "</color>" + "\n";
            oracionCompleta += "<color=#0000ffff>- " + respuestasCorrectas[2].GetDescriptorAlimento() + "</color>" + "\n";
            return oracionCompleta;
        }
        else if (numeroDePlato == 3)
        {
            /*oracionCompleta = "En el tercer plato deseo <color=#ff00ffff>" + respuestasCorrectas[0].GetDescriptorAlimento() + "</color>";
            oracionCompleta += ". Como segundo ingrediente sería excelente agregar <color=#ff0000ff>" + respuestasCorrectas[1].GetDescriptorAlimento() + "</color>";
            oracionCompleta += ". Y como toque especial, agregaremos también <color=#0000ffff>" + respuestasCorrectas[2].GetDescriptorAlimento() + "</color>";*/
            oracionCompleta = "Plato 3:\n<color=#ff00ffff>- " + respuestasCorrectas[0].GetDescriptorAlimento() + "</color>" + "\n";
            oracionCompleta += "<color=#ff0000ff>- " + respuestasCorrectas[1].GetDescriptorAlimento() + "</color>" + "\n";
            oracionCompleta += "<color=#0000ffff>- " + respuestasCorrectas[2].GetDescriptorAlimento() + "</color>" + "\n";
            return oracionCompleta;
        }
        else
        {           
            return "Houston, we have a problem!";
            //ControladorA2.instanciaCompartida.CambiarEstadoA2(estadoA2.fin);
        }
        
    }


    public void ReiniciarIntentos()
    {
        intentos = 0;
    }
}
