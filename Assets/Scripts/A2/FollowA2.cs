using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowA2 : MonoBehaviour {

    public static FollowA2 instanciaCompartida;

    public float interpVelocity;
    public float minDistance;
    public float followDistance;
    public GameObject target;
    public Vector3 offset;
    Vector3 targetPos;

    //Posiciones posibles de la camara
    Vector3[] posicionCamara = new Vector3[2]{new Vector3(0, 0, 0), new Vector3(18f, 0f, 0)};
    bool estamosEnLaCocina = true;      //Cuando es false, se esta en la despensa, es decir en posicionCamara[1]




    public void CambiarDeHabitacion()
    {
        if (ControladorA2.instanciaCompartida.estadoActualA2 == estadoA2.juego)
        {
            if (estamosEnLaCocina)
            {
                target.transform.position = posicionCamara[1];
                estamosEnLaCocina = false;
                ControladorA2.instanciaCompartida.ordenesChef.enabled = false;
                ControladorA2.instanciaCompartida.LetreroNombreAlimentoPanel.enabled = true;
            }
            else if (!estamosEnLaCocina)
            {
                target.transform.position = posicionCamara[0];
                estamosEnLaCocina = true;
                ControladorA2.instanciaCompartida.ordenesChef.enabled = true;
                ControladorA2.instanciaCompartida.LetreroNombreAlimentoPanel.enabled = false;
            }
        }
    }


    public void CambiarDeHabitacionIntrucciones()
    {
            if (estamosEnLaCocina)
            {
                target.transform.position = posicionCamara[1];
                estamosEnLaCocina = false;
                /*ControladorA2.instanciaCompartida.ordenesChef.enabled = false;
                ControladorA2.instanciaCompartida.LetreroNombreAlimentoPanel.enabled = true;*/
            }
            else if (!estamosEnLaCocina)
            {
                target.transform.position = posicionCamara[0];
                estamosEnLaCocina = true;
                /*ControladorA2.instanciaCompartida.ordenesChef.enabled = true;
                ControladorA2.instanciaCompartida.LetreroNombreAlimentoPanel.enabled = false;*/
            }
    }


    public void VolverCocina()
    {
        target.transform.position = posicionCamara[0];
        estamosEnLaCocina = true;
        ControladorA2.instanciaCompartida.ordenesChef.enabled = false;
    }

    public bool EstamosEnLaCocina()
    {
        return estamosEnLaCocina;
    }


    // Use this for initialization
    void Start()
    {
        targetPos = transform.position;
    }

    void Awake()
    {
        instanciaCompartida = this;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

            Vector3 targetDirection = (target.transform.position - posNoZ);

            interpVelocity = targetDirection.magnitude * 40f;       //Esta variable fija la velocidad de movimiento de la camara al cambiar de posicion

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.25f);

        }
    }
}
