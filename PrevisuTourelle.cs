using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrevisuTourelle : MonoBehaviour
{
    public Manager manager;
    public GameObject previsuAllowed;
    public GameObject previsuLocked;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //si la tourelle est trop proche d'un autre objet, regarde si cet objet est probl�matique quant � la pose de la tourelle, et l'ajoute � la liste des objets qui g�nent si c'est le cas
    //affiche la pr�visualisation de la tourelle avec l'apparence qui interdit la pose de la tourelle
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Path" || other.gameObject.tag == "Turret")
        {
            manager.objectsEnContactAvecLaPrevisu.Remove(other.gameObject);
            manager.objectsEnContactAvecLaPrevisu.Add(other.gameObject);
            previsuAllowed.SetActive(false);
            previsuLocked.SetActive(true);
        }
    }

    //retire de la liste des objets qui obstruent l'objet dont on s'est �loign�, remet l'apparence qui autorise la pose de la tour s'il n'y a plus d'objets qui obstruent
    public void OnTriggerExit(Collider other)
    {
        manager.objectsEnContactAvecLaPrevisu.Remove(other.gameObject);
        if(manager.objectsEnContactAvecLaPrevisu.Count == 0)
        {
            previsuAllowed.SetActive(true);
            previsuLocked.SetActive(false);
        }
    }
}
