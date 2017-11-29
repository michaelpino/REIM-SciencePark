using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alimento : MonoBehaviour {

    public int idAlimento;
    public string nombre;
    public string articuloPrevio;               //Articulo para describir al alimento, de tipo "un" "una"
    public string descriptorDelAlimento;        //Frase que describe al alimento y que usa el chef para solicitarlo
    bool enBolsa = false;
    /*public string color;
    public string tipoDeAlimento;       //flor, fruto, semilla
    public string formaDelAlimento;     //redonda, fruto, semilla
    public string saborDelAlimento;     //dulce, acido, desconocido*/

    Vector3 ubicacion = new Vector3(0, 0, 0);

    float x;
    float y;
    float z;
    Vector3 desplazamiento = new Vector3();

    public Vector3 Ubicacion;

    // Use this for initialization
    void Start () {
		
	}


    private void Awake()
    {
        //Obtener la posicion en el eje z del GameObject
        z = Camera.main.WorldToScreenPoint(new Vector3(0, 0, transform.position.z)).z;
    }

	// Update is called once per frame
	void Update () {
        //Obtener la posicion del mouse
        x = Input.mousePosition.x;
        y = Input.mousePosition.y;
    }

    void OnMouseDown()
    {        
        if (!enBolsa && ControladorA2.instanciaCompartida.estadoActualA2!= estadoA2.instrucciones)
        {
            //Calcular el desplazamiento del mouse respecto al centro del objeto
            desplazamiento = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            ControladorA2.instanciaCompartida.MostrarNombreAlimento(this.nombre);
        }    
    }

    void OnMouseDrag()
    {
        if (!enBolsa && ControladorA2.instanciaCompartida.estadoActualA2 != estadoA2.instrucciones)
        {
            //Mover el objeto en funcion de la posicion del mouse (sin variar el eje z), sumando el desplazamiento inicial
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(x, y, z)) + desplazamiento;
        }
    }

    private void OnMouseUp()
    {
        if (!enBolsa)
        {
            //Devuelve el objeto a su posicion original
            transform.position = ubicacion;
        }
    }


    public Vector3 GetUbicacion()
    {
        return ubicacion;
    }

    public void SetUbicacion(Vector3 nuevaUbicacion)
    {
        ubicacion = nuevaUbicacion;
    }


    public string GetNombre()
    {
        return nombre;
    }


    public string GetDescriptorAlimento()
    {
        return descriptorDelAlimento;
    }


    public string GetNombreMasArticulo()
    {
        return articuloPrevio + " " + nombre;
    }

    public void SetEnBolsa()
    {
        enBolsa = true;
    }
}
