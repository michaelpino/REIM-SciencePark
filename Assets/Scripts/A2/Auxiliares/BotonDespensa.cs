using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonDespensa : MonoBehaviour {

    void OnMouseDown()
    {
        ControladorA2.instanciaCompartida.registrarToqueSensor(2);
        ControladorA2.instanciaCompartida.intento.registrarCambioHabitacion(Bolsa.instanciaCompartida.platoActual, "Hacia la despensa");
        ControladorA2.instanciaCompartida.CambiarHabitacion();
    }
}
