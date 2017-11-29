using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luz : MonoBehaviour {

    public int ubicacionActual;

    public static Luz instanciaCompartida;

    private void Awake()
    {
        instanciaCompartida = this;
    }

    // Use this for initialization
    void Start () {
        ControladorA1.instanciaCompartida.MoverLuz(2);
        ubicacionActual = 2;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetUbicacionActual(int nuevaUbicacionActual)
    {
        ubicacionActual = nuevaUbicacionActual;
    }

    public int GetUbicacionActual()
    {
        return ubicacionActual;
    }

    public void CambiarPosicionLuz()
    {
        System.Random rnd = new System.Random();
        int nuevaPosicion = rnd.Next(1, 4);
        ubicacionActual = nuevaPosicion;
        ControladorA1.instanciaCompartida.MoverLuz(nuevaPosicion);
    }
}
