using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matabichos : MonoBehaviour {

    //***Variables para permitir drag and drop***
    Vector3 ubicacion = new Vector3(0, 0, 0);
    Vector3 desplazamiento = new Vector3();
    float x;
    float y;
    float z;



    private void Awake()
    {
        //Obtener la posicion en el eje z del GameObject
        z = Camera.main.WorldToScreenPoint(new Vector3(0, 0, transform.position.z)).z;
    }

    // Use this for initialization
    void Start()
    {
        //Fijo la ubicacion inicial del objeto segun colocacion en Unity
        ubicacion.Set(transform.position.x, transform.position.y, transform.position.z);
    }


    // Update is called once per frame
    void Update()
    {
        //Obtener la posicion del mouse
        x = Input.mousePosition.x;
        y = Input.mousePosition.y;
    }

    void OnMouseDown()
    {
        if (ControladorA1.instanciaCompartida.estadoActualA1 == estadoA1.juego)
        {
            //Calcular el desplazamiento del mouse respecto al centro del objeto
            desplazamiento = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            //ControladorA2.instanciaCompartida.MostrarNombreAlimento(this.nombre);
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
        if (true)
        {
            //Devuelve el objeto a su posicion original
            transform.position = ubicacion;
        }
    }


    //Devuelve el objeto a su posicion original
    public void VolverAlOrigen()
    {
        transform.position = ubicacion;
    }
}
