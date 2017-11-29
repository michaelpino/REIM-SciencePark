using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControladorDeAnimales : MonoBehaviour {

    public List<Animal> todosLosAnimales = new List<Animal>(); //Lista que contiene todos los animales disponibles
    List<int> indiceDeAnimales = new List<int>();               //Lista que contiene indices randomizados para mostrar animales
    


    public Transform posicionActualAnimal; // Punto inicial donde aparecerán los animales

    public Animal animalActual;    //Variable para mantener al animal actual en pantalla
    public int portalActual = 2;  //Variable que maneja el portal acual en que está el animal. Comienza e el portal central

    Vector3[] posicionPortales = new Vector3[5]{new Vector3(2 * -16f, -2.8f, 0),
    new Vector3(-16f, -2.8f, 0),
    new Vector3(0, -2.8f, 0),
    new Vector3(15.6f, -2.8f, 0),
    new Vector3(2 * 15.6f, -2.8f, 0)};


    public static ControladorDeAnimales instanciaCompartida;  // instancia compartida para solo tener un generador de animales


    void Awake()
    {
        instanciaCompartida = this;  
    }


    public void AgregarAnimal()
    {
        Animal animal = (Animal)Instantiate(todosLosAnimales[indiceDeAnimales[0]]);     //Se selecciona y crea el primer animal de la lista randomizada
        animal.transform.SetParent(this.transform, false);

        //Posición del animal
        animal.transform.position = posicionPortales[portalActual];

        indiceDeAnimales.RemoveAt(0);       //Se remueve el primer animal de los utilizables, para evitar repeticiones
        animalActual = animal;
    }


    public void RemoverAnimal()
    {
        Animal animal = animalActual;
        Destroy(animal.gameObject);
    }

    public void OcultarAnimal()
    {
        animalActual.transform.position = new Vector3(-16f, -40.8f, 0);
    }

    public void MostrarAnimal()
    {
        animalActual.transform.position = posicionPortales[portalActual];
    }

    public void MoverAnimal(string seleccion)
    {
        if (ControladorA3.instanciaCompartida.estadoActualA3 == estadoA3.juego)
        {
            if (seleccion == "derecha")
            {
                if (portalActual == 4)
                {
                    posicionActualAnimal.transform.position = posicionPortales[0];
                    animalActual.transform.position = posicionPortales[0];
                    portalActual = 0;
                }
                else
                {
                    posicionActualAnimal.transform.position = posicionPortales[portalActual + 1];
                    animalActual.transform.position = posicionPortales[portalActual + 1];
                    portalActual = portalActual + 1;
                }
            
            }else if (seleccion == "izquierda")
            {
                if (portalActual == 0)
                {
                    posicionActualAnimal.transform.position = posicionPortales[4];
                    animalActual.transform.position = posicionPortales[4];
                    portalActual = 4;
                }
                else
                {
                    posicionActualAnimal.transform.position = posicionPortales[portalActual - 1];
                    animalActual.transform.position = posicionPortales[portalActual - 1];
                    portalActual = portalActual - 1;
                }
            }
        }            
    }


    public void RandomizarListaDeAnimales(int maxDeAnimales)
    {
        System.Random rnd = new System.Random();    //Se crea el objeto que entregara el numero aleatorio. OJO:Se utiliza System.Random en vez de Random solo, que usa el random de Unity, el cual no tiene Next() como metodo
        int animalCandidato;
    
        while (indiceDeAnimales.Count != maxDeAnimales)
        {
            animalCandidato = rnd.Next(0, todosLosAnimales.Count);  //Se guarda el animal candidato a agregar
            if (!indiceDeAnimales.Contains(animalCandidato))            //Se comprueba que el animal no haya sido agregado
            {
                indiceDeAnimales.Add(animalCandidato);
            } 
        }
    }

    
    public bool QuedanMasAnimales()
    {
        if(indiceDeAnimales.Count > 0)
        {
            return true;
        }
        else{
            return false;
        }
    }
    
    public string VerPortalActual()
    {
        if(portalActual == 0)
        {
            return "Aire";
        }
        else if(portalActual == 1)
        {
            return "Desierto";
        }
        else if (portalActual == 2)
        {
            return "Bosque";
        }
        else if (portalActual == 3)
        {
            return "Hielo";
        }
        else if (portalActual == 4)
        {
            return "Oceano";
        }
        else
        {
            return "Error!";
        }
    }
    
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
