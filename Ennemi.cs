using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemi : MonoBehaviour
{
    public float moveSpeed;

    public Manager manager;
    public int pv;
    public int touches;
    public int valeurGold;

    [SerializeField]
    private int step = 1;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        transform.position = manager.ListTurn[0].position;
        transform.LookAt(manager.ListTurn[1]);
    }

    // Update is called once per frame
    void Update()
    {
        if(moveSpeed*Time.deltaTime > (manager.ListTurn[step].position - transform.position).magnitude)
        {
            transform.position = manager.ListTurn[step].position;
            if (step == manager.ListTurn.Count - 1)
            {
                manager.vie--;
                if (manager.vie <= 0)
                {
                    manager.FinDePartie();
                }
                else if (GameObject.Find("Enemies").transform.childCount == 1)
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
                Destroy(gameObject);
            }
            else
            {
                step++;
                transform.LookAt(manager.ListTurn[step].position);
            }
        }
        else
        {
            transform.Translate((manager.ListTurn[step].position - transform.position).normalized * moveSpeed * Time.deltaTime,Space.World);
        }
    }
}
