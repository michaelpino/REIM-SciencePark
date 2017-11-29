using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonFondo : MonoBehaviour {

    void OnMouseDown()
    {
        ControladorA2.instanciaCompartida.registrarToqueSensor(1);
    }
}
