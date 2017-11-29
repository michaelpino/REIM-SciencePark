using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonFondoA1 : MonoBehaviour {

    void OnMouseDown()
    {
        ControladorA1.instanciaCompartida.registrarToqueSensor(1);
    }
}
