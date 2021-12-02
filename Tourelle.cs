using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tourelle : MonoBehaviour
{
    public List<GameObject> ennemisAPortee = new List<GameObject>();
    public GameObject ennemiSelectionne;
    public GameObject projectile;
    public float cadence;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Tir());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent != null && other.gameObject.transform.parent.tag == "Ennemi")
        {
            ennemisAPortee.Remove(other.gameObject.transform.parent.gameObject);
            ennemisAPortee.Add(other.gameObject.transform.parent.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
            ennemisAPortee.Remove(other.gameObject.transform.parent.gameObject);
    }

    public IEnumerator Tir()
    {
        yield return new WaitForSeconds(cadence);
        StartCoroutine(Tir());
        for (int i = 0; i < ennemisAPortee.Count; i++)
        {
            if (ennemisAPortee[i] == null)
            {
                ennemisAPortee.RemoveAt(i);
                i--;
            }
        }
        if (ennemiSelectionne != null && !ennemisAPortee.Contains(ennemiSelectionne))
        {
            ennemiSelectionne = null;
        }
        if (ennemisAPortee.Count > 0)
        {
            ennemiSelectionne = ennemisAPortee[0];
            float distanceEnnemi = (ennemiSelectionne.transform.position - transform.position).magnitude;
            if (ennemisAPortee.Count > 1)
            {
                for (int i = 1; i < ennemisAPortee.Count; i++)
                {
                    if ((ennemisAPortee[i].transform.position - transform.position).magnitude < distanceEnnemi)
                    {
                        ennemiSelectionne = ennemisAPortee[i];
                        distanceEnnemi = (ennemiSelectionne.transform.position - transform.position).magnitude;
                    }
                }
            }
        }
        if (ennemiSelectionne != null)
        {
            GameObject newProjectile;
            newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            newProjectile.GetComponent<Projectile>().cible = ennemiSelectionne;
        }
    }
}
