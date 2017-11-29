using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ubicador : MonoBehaviour {

    Vector3 ubicacion = new Vector3(0, 0, 0);
    public string posicion;
    public int numeroPosicion;      //1 = Izquierda, 2 = centro, 3 = derecha


    // Use this for initialization
    void Start () {
        ubicacion.Set(transform.position.x, transform.position.y, transform.position.z);
        //Debug.Log("Posicion de Inicial de ubicador de " + posicion + ": " + ubicacion);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3 GetUbicacion()
    {
        return ubicacion;
    }


    public string GetPosicion()
    {
        return posicion;
    }


    public int GetNumeroPosicion()
    {
        return numeroPosicion;
    }
}
