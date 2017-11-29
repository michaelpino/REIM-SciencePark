using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorSicomotrizA3 : MonoBehaviour {

    //Funciones Juego
    public void SensorBordeAnimal()
    {
        BaseDeDatosA3.instanciaCompartida.countBordeArrastreAnimal++;
    }

    public void SensorBordeBtnIzq()
    {
        BaseDeDatosA3.instanciaCompartida.countBordeBtnIzq++;
    }

    public void SensorBordeBtnDer()
    {
        BaseDeDatosA3.instanciaCompartida.countBordeBtnDer++;
    }

    public void SensorFondo()
    {
        BaseDeDatosA3.instanciaCompartida.countFondo++;
    }




    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
