using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Projectile : MonoBehaviour
{
    public Manager manager;

    public GameObject cible;
    public float speed;
    public int degatsCritiques;
    public int degats;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();   
    }

    // Update is called once per frame
    void Update()
    {
        if(cible == null)
        {
            Destroy(gameObject);
        }
        else
        {
            if ((cible.transform.position - transform.position).magnitude < speed * Time.deltaTime)
            {
                InfligerDegats();
            }
            else
            {
                transform.Translate((cible.transform.position - transform.position).normalized * speed * Time.deltaTime, Space.World);
            }
        }
    }

    public void InfligerDegats()
    {
        cible.GetComponent<Ennemi>().touches++;

        if (name == "Projectile1(Clone)" && cible.name == "Ennemi1(Clone)"
       || name == "Projectile2(Clone)" && cible.name == "Ennemi2(Clone)"
       || name == "Projectile3(Clone)" && cible.name == "Ennemi3(Clone)"
       || name == "Projectile4(Clone)" && cible.name == "Ennemi4(Clone)")
        {
            cible.GetComponent<Ennemi>().pv -= degats;
        }
        else
        {
            cible.GetComponent<Ennemi>().pv -= degatsCritiques;
        }
        if(cible.GetComponent<Ennemi>().pv <= 0)
        {
            manager.gold += cible.GetComponent<Ennemi>().valeurGold / cible.GetComponent<Ennemi>().touches;
            manager.score++; 
            if (GameObject.Find("Enemies").transform.childCount == 1)
            {
                if (manager.vague == manager.vagueMax)
                {
                    manager.FinDePartie();
                }
                else
                {
                    manager.DeclancherProchaineVague();
                }
            }
            Destroy(cible);
        }
        Destroy(gameObject);
    }
}
