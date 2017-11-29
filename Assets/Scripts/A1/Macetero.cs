using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Macetero : MonoBehaviour {

    public static Macetero instanciaCompartida;

    //***Variables para permitir drag and drop***
    public Vector3 ubicacion = new Vector3(0, 0, 0);
    Vector3 desplazamiento = new Vector3(0, 0, 0);
    float x;
    float y;
    float z;
    public Ubicador carrilIzquierdo;
    public Ubicador carrilCentral;
    public Ubicador carrilDerecho;
    //public List<Ubicador> listaDeCarriles = new List<Ubicador>();
    public List<Vector3> listaDeCarriles = new List<Vector3>();



    public int faseDePlanta;
    
    public string faseDePlantaTexto = "";



    private void Awake()
    {
        instanciaCompartida = this;
        //Obtener la posicion en el eje z del GameObject
        z = Camera.main.WorldToScreenPoint(new Vector3(0, 0, transform.position.z)).z;
    }

    // Use this for initialization
    void Start () {
        ubicacion.Set(transform.position.x, transform.position.y, transform.position.z);
        //Debug.Log("Posicion inicial de Macetero: " + ubicacion);
        /*listaDeCarriles.Add(carrilIzquierdo);
        listaDeCarriles.Add(carrilCentral);
        listaDeCarriles.Add(carrilDerecho);*/
        /*listaDeCarriles.Add(new Vector3(-3.7f, 0.6f, 0));
        listaDeCarriles.Add(new Vector3(0.5f, 0.6f, 0));
        listaDeCarriles.Add(new Vector3(4.5f, 0.6f, 0));*/
        listaDeCarriles.Add(carrilIzquierdo.GetUbicacion());
        listaDeCarriles.Add(carrilCentral.GetUbicacion());
        listaDeCarriles.Add(carrilDerecho.GetUbicacion());
    }
	

	// Update is called once per frame
	void Update () {
        //Obtener la posicion del mouse
        x = Input.mousePosition.x;
        y = Input.mousePosition.y;
    }

    void OnMouseDown()
    {
        if (ControladorA1.instanciaCompartida.estadoActualA1 == estadoA1.juego)
        {
            //ControladorA1.instanciaCompartida.mensajeTexo.text = "";
            //Calcular el desplazamiento del mouse respecto al centro del objeto
            desplazamiento = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            //Macetero.instanciaCompartida.Simulado();
        }
    }

    void OnMouseDrag()
    {
        if (ControladorA1.instanciaCompartida.estadoActualA1 == estadoA1.juego)
        {
            //Mover el objeto en funcion de la posicion del mouse (sin variar el eje z), sumando el desplazamiento inicial
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(x, y, z)) + desplazamiento;
        }
    }

    private void OnMouseUp()
    {
        if (ControladorA1.instanciaCompartida.estadoActualA1 == estadoA1.juego)
        {
            //Devuelve el objeto a su posicion original
            transform.position = ubicacion;
            Debug.Log("Retorno de posicion de Macetero: " + ubicacion);
            Macetero.instanciaCompartida.Simulado();
        }
    }

    void OnTriggerEnter2D(Collider2D otroObjeto)
    {
        /*if(ControladorA1.instanciaCompartida.estadoActualA1 == estadoA1.juego)
        {*/
            if (otroObjeto.gameObject.tag.Equals("Ubicacion"))
            {
                /*ubicacion.Set(otroObjeto.GetComponent<Ubicador>().GetUbicacion().x, otroObjeto.GetComponent<Ubicador>().GetUbicacion().y + 2, otroObjeto.GetComponent<Ubicador>().GetUbicacion().z);
                transform.position = ubicacion;*/
                
                ubicacion = listaDeCarriles[otroObjeto.GetComponent<Ubicador>().numeroPosicion - 1];
                //Debug.Log("Nueva posicion de Macetero: " + ubicacion);
                //transform.position = listaDeCarriles[otroObjeto.GetComponent<Ubicador>().numeroPosicion - 1];
                //Simulado();
                Planta.instanciaCompartida.SetUbicacion(otroObjeto.GetComponent<Ubicador>().GetNumeroPosicion());
                ControladorA1.instanciaCompartida.intento.registrarMovimientoMacetero();
            }

            if (otroObjeto.gameObject.tag.Equals("Vaso"))
            {
                TimerA1.instanciaCompartida.Humedecer();
                ControladorA1.instanciaCompartida.intento.registrarAguaAgregada();
                //transform.position = ubicacion;
            }

            if (otroObjeto.gameObject.tag.Equals("Fertilizante"))
            {
            
                Planta.instanciaCompartida.AgregarFertilizante();
                //transform.position = ubicacion;
            }  

        //}
             
    }

    public void ReiniciarPosicion()
    {
        transform.position = listaDeCarriles[1];
    }

    public void Simulado()
    {
        GetComponent<Rigidbody2D>().simulated = true;
    }
}
