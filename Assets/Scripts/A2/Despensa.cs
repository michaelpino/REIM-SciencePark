using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despensa : MonoBehaviour {

    public static Despensa instanciaCompartida;
    int cantidadDeAlimentos = 12;
    public List<Alimento> listaDeAlimentos = new List<Alimento>();
    public List<Alimento> alimentosEnDespensa = new List<Alimento>();
    List<int> indiceDeAlimentos = new List<int>();               //Lista que contiene indices randomizados para mostrar animales

    Vector3[] posicionAlimentos = new Vector3[12]{new Vector3(12.3f, 2.5f, 0f), new Vector3(12.3f + 3.5f, 2.5f, 0f), new Vector3(12.3f + 7.0f, 2.5f, 0f), new Vector3(12.3f + 10.5f, 2.5f, 0f),
                                                    new Vector3(12.3f, -0.3f, 0f), new Vector3(12.3f + 3.5f, -0.3f, 0f), new Vector3(12.3f + 7.0f, -0.3f, 0f), new Vector3(12.3f + 10.5f, -0.3f, 0f),
                                                    new Vector3(12.3f, -3.3f, 0f), new Vector3(12.3f + 3.0f, -3.3f, 0f), new Vector3(12.3f + 6.0f, -3.3f, 0f), new Vector3(12.3f + 9.0f, -3.3f, 0f)};

    // Use this for initialization
    void Start () {
        
    }



    private void Update()
    {
        
    }


    private void Awake()
    {
        instanciaCompartida = this;
    }

    public void ReiniciarDespensa()
    {
        indiceDeAlimentos.Clear();
        EliminarAlimentosEnDespensa();
        RandomizarListaDeAlimentos(cantidadDeAlimentos);
        PosicionarAlimentos();
        Bolsa.instanciaCompartida.GenerarRespuestasCorrectas();
        Bolsa.instanciaCompartida.intentos = 0;
    }


    public void ReiniciarDespensaInstrucciones()            //Metodo exclusivo para mostrar alimentos en despensa al explicar por primera vez las instrucciones
    {
        indiceDeAlimentos.Clear();
        EliminarAlimentosEnDespensa();
        RandomizarListaDeAlimentos(cantidadDeAlimentos);
        PosicionarAlimentos();
    }


    public void RandomizarListaDeAlimentos(int maxDeAnimales)
    {
        System.Random rnd = new System.Random();    //Se crea el objeto que entregara el numero aleatorio. OJO:Se utiliza System.Random en vez de Random solo, que usa el random de Unity, el cual no tiene Next() como metodo
        int animalCandidato;                        //Variable que almacenara momentaneamente el index que se quiere agregar en desorden

        while (indiceDeAlimentos.Count != maxDeAnimales)
        {
            animalCandidato = rnd.Next(0, listaDeAlimentos.Count);       //Se guarda el alimento candidato a agregar
            if (!indiceDeAlimentos.Contains(animalCandidato))            //Se comprueba que el alimento no haya sido agregado
            {
                indiceDeAlimentos.Add(animalCandidato);
            }
        }
    }


    public Alimento CrearAlimento(int indice)
    {
        Alimento alimento = (Alimento)Instantiate(listaDeAlimentos[indice]);     //Se selecciona y crea el alimento de la lista randomizada
        alimento.transform.SetParent(this.transform, false);
        alimento.transform.position = new Vector3 (0,0,0);
        return alimento;
    }

    public void PosicionarAlimentos()
    {
        int i = 0;
        foreach (int alimento in indiceDeAlimentos)
        {
            Alimento nuevoAlimento = CrearAlimento(alimento);
            nuevoAlimento.transform.position = posicionAlimentos[i];
            nuevoAlimento.SetUbicacion(posicionAlimentos[i]);     //Se crea el alimento y se le da una nueva ubicacion
            alimentosEnDespensa.Add(nuevoAlimento);
            i++;
        }
    }

    public void EliminarAlimentosEnDespensa()
    {
        foreach (Alimento alimento in alimentosEnDespensa)
        {
            Destroy(alimento.gameObject);
        }
        alimentosEnDespensa.Clear();
    }


    public List<Alimento> GetAlimentosEnDespensa()
    {
        return alimentosEnDespensa;
    }


    public void OcultarAlimentos()
    {
        foreach (Alimento alimento in alimentosEnDespensa)
        {
            if (!Bolsa.instanciaCompartida.EstaAlimento(alimento))
            {
                alimento.gameObject.SetActive(false);
            }
        }
    }


    public void MostrarAlimentos()
    {
        Bolsa.instanciaCompartida.OcultarAlimentos();
        foreach (Alimento alimento in alimentosEnDespensa)
        {
            if (!Bolsa.instanciaCompartida.EstaAlimento(alimento))
            {
                alimento.gameObject.SetActive(true);
            }
        }
    }

    //Entrega una lista con los nombres de todos los ingredientes existentes
    public List<string> EntregarListaDeAlimentos(List<string> nuevaLista)
    {
        //List<string> nuevaLista = new List<string>();
        foreach (Alimento alimento in listaDeAlimentos)
        {
            nuevaLista.Add(alimento.GetNombre());
        }
        return nuevaLista;
    }
}
