using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    //La liste des points qui forment le chemin (au niveau des virages)
    public List<Transform> ListTurn = new List<Transform>();
    public GameObject groundPathPrefab;
    public GameObject cercleGroundPrefab;
    public List<GameObject> enemiesPrefab = new List<GameObject>();
    public TextMeshProUGUI goldTxt;
    public TextMeshProUGUI vieTxt;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI FinDePartieTxt;
    public TextMeshProUGUI meilleurScoreTxt;
    public TextMeshProUGUI meilleurNiveauTxt;

    public GameObject menu;
    public bool partieEnCours;

    public string pseudo;
    public int niveauMax;
    public int meilleurScore;
    public int vague;
    public int vagueMax;
    public int level;
    public int score;
    public float multiplierEnemies;
    public float spawnTime;
    public GameObject tourelle1Prefab, tourelle2Prefab, tourelle3Prefab, tourelle4Prefab;
    public int coutTourelle1, coutTourelle2, coutTourelle3, coutTourelle4;
    public GameObject tourelle1Previsu, tourelle2Previsu, tourelle3Previsu, tourelle4Previsu;
    public GameObject previsuTourelleEnCours;
    public Transform pointApparitionPrevisuTourelle1, pointApparitionPrevisuTourelle2, pointApparitionPrevisuTourelle3, pointApparitionPrevisuTourelle4;
    public Vector3 positionDoigtSol;
    public Vector3 decalageDoigtTourelle;
    public List<GameObject> objectsEnContactAvecLaPrevisu = new List<GameObject>();
    public int gold;
    public int vie;

    [SerializeField]
    private float ratioTurret1;
    [SerializeField]
    private float ratioTurret2;
    [SerializeField]
    private float ratioTurret3;
    [SerializeField]
    private float ratioTurret4;

    public GameObject text;
    public Ray ray;
    public RaycastHit hit;
    public LayerMask clicMaskGenerateur;
    public LayerMask clicMaskSol;

    // Start is called before the first frame update
    void Start()
    {
        CreationFloor();
    }

    // Update is called once per frame
    void Update()
    {
        goldTxt.text = gold.ToString();
        vieTxt.text = vie.ToString();
        scoreTxt.text = score.ToString();

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000f, clicMaskSol))
        {
            positionDoigtSol = hit.point;
            positionDoigtSol = new Vector3(positionDoigtSol.x, 0, positionDoigtSol.z);
        }


        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f, clicMaskGenerateur))
            {
                if (hit.collider.tag == "GenerateurTourelle")
                {
                    ClicGenerateur(hit.collider.gameObject);
                }
            }
        }

        if (previsuTourelleEnCours != null && Input.GetMouseButton(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f, clicMaskSol))
            {
                previsuTourelleEnCours.transform.position = new Vector3(hit.point.x, 0, hit.point.z) + decalageDoigtTourelle;
            }
        }

        if (previsuTourelleEnCours != null && Input.GetMouseButtonUp(0))
        {
            CreationTourelle();
        }
    }

    private void CreationFloor()
    {
        //crée un chemin entre chaque virage et un cercle de terrain sur chaque virage 
        GameObject newPath;
        for (int i = 0; i < ListTurn.Count - 1; i++)
        {
            newPath = Instantiate(cercleGroundPrefab, ListTurn[i].position, Quaternion.identity);
            newPath.transform.parent = GameObject.Find("Ground").transform.Find("Circle").transform;
            newPath = Instantiate(groundPathPrefab, ListTurn[i].position + (ListTurn[i + 1].position - ListTurn[i].position) / 2, Quaternion.identity);
            newPath.transform.LookAt(ListTurn[i]);
            newPath.transform.localScale = new Vector3(1, .1f, (ListTurn[i + 1].position - ListTurn[i].position).magnitude);
            newPath.transform.parent = GameObject.Find("Ground").transform.Find("Path").transform;
        }
        newPath = Instantiate(cercleGroundPrefab, ListTurn[ListTurn.Count - 1].position, Quaternion.identity);
        newPath.transform.parent = GameObject.Find("Ground").transform.Find("Circle").transform;
    }

    private void ApparitionEnnemis()
    {
        int nbEnemiesTotal =(int)(10 *  level * vague * multiplierEnemies);
        CalculationRateTurrets();
        List<int> listEnemies = new List<int>();
        for (int i = 0; i <= nbEnemiesTotal * ratioTurret1; i++)
        {
            listEnemies.Add(1);
        }
        for (int i = 0; i <= nbEnemiesTotal * ratioTurret2; i++)
        {
            listEnemies.Add(2);
        }
        for (int i = 0; i <= nbEnemiesTotal * ratioTurret3; i++)
        {
            listEnemies.Add(3);
        }
        for (int i = 0; i <= nbEnemiesTotal * ratioTurret4; i++)
        {
            listEnemies.Add(4);
        }
        int j = 0;
        while (listEnemies.Count > 0 && j < 10000)
        {
            int random = Random.Range(0, listEnemies.Count - 1);
            StartCoroutine(SpawnOneEnemy(j, listEnemies[random]));
            listEnemies.RemoveAt(random);
            j++;
        }
    }

    private void CalculationRateTurrets()
    {
        int nombreTotalTurret = GameObject.Find("ContainerTurrets").transform.Find("ContainerTurrets1").childCount
                                                + GameObject.Find("ContainerTurrets").transform.Find("ContainerTurrets2").childCount
                                                + GameObject.Find("ContainerTurrets").transform.Find("ContainerTurrets3").childCount
                                                + GameObject.Find("ContainerTurrets").transform.Find("ContainerTurrets4").childCount;
        if (nombreTotalTurret == 0)
        {
            ratioTurret1 = .25f;
            ratioTurret2 = .25f;
            ratioTurret3 = .25f;
            ratioTurret4 = .25f;
        }
        else
        {
            ratioTurret1 = GameObject.Find("ContainerTurrets").transform.Find("ContainerTurrets1").childCount / nombreTotalTurret;
            ratioTurret2 = GameObject.Find("ContainerTurrets").transform.Find("ContainerTurrets2").childCount / nombreTotalTurret;
            ratioTurret3 = GameObject.Find("ContainerTurrets").transform.Find("ContainerTurrets3").childCount / nombreTotalTurret;
            ratioTurret4 = GameObject.Find("ContainerTurrets").transform.Find("ContainerTurrets4").childCount / nombreTotalTurret;
        }
    }

    IEnumerator SpawnOneEnemy(int ordre, int type)
    {
        yield return new WaitForSeconds(ordre * spawnTime);
        if (partieEnCours)
        {
            GameObject newEnemy = Instantiate(enemiesPrefab[type - 1]);
            newEnemy.transform.parent = GameObject.Find("Enemies").transform;
        }
    }

    public void DeclancherProchaineVague()
    {
        StartCoroutine(DeclancherProchaineVagueEnum());
    }
    public IEnumerator DeclancherProchaineVagueEnum()
    {
        if (vague != 0)
        {
            text.GetComponent<TextMeshProUGUI>().text = "Vague " + vague + " Terminée";
            text.SetActive(true);
        }
        vague++;
        yield return new WaitForSeconds(2);
        text.GetComponent<TextMeshProUGUI>().text = "Début de la vague " + vague;
        yield return new WaitForSeconds(2);
        text.SetActive(false);
        ApparitionEnnemis();
    }

    public void ClicGenerateur(GameObject generateur)
    {
        if (generateur.name == "GenerateurTourelle1")
        {
            if (gold >= coutTourelle1)
            {
                previsuTourelleEnCours = Instantiate(tourelle1Previsu);
                previsuTourelleEnCours.transform.position = pointApparitionPrevisuTourelle1.position;
                decalageDoigtTourelle = pointApparitionPrevisuTourelle1.position - positionDoigtSol;
            }
        }
        else if (generateur.name == "GenerateurTourelle2")
        {
            if (gold >= coutTourelle2)
            {
                previsuTourelleEnCours = Instantiate(tourelle2Previsu);
                previsuTourelleEnCours.transform.position = pointApparitionPrevisuTourelle2.position;
                decalageDoigtTourelle = pointApparitionPrevisuTourelle2.position - positionDoigtSol;
            }
        }
        else if (generateur.name == "GenerateurTourelle3")
        {
            if (gold >= coutTourelle3)
            {
                previsuTourelleEnCours = Instantiate(tourelle3Previsu);
                previsuTourelleEnCours.transform.position = pointApparitionPrevisuTourelle3.position;
                decalageDoigtTourelle = pointApparitionPrevisuTourelle3.position - positionDoigtSol;
            }
        }
        else if (generateur.name == "GenerateurTourelle4")
        {
            if (gold >= coutTourelle4)
            {
                previsuTourelleEnCours = Instantiate(tourelle4Previsu);
                previsuTourelleEnCours.transform.position = pointApparitionPrevisuTourelle4.position;
                decalageDoigtTourelle = pointApparitionPrevisuTourelle4.position - positionDoigtSol;
            }
        }
    }

    private void CreationTourelle()
    {
        if (objectsEnContactAvecLaPrevisu.Count == 0)
        {
            if (previsuTourelleEnCours.name == "PrevisuTourelle1(Clone)")
            {
                GameObject newTurret = Instantiate(tourelle1Prefab, previsuTourelleEnCours.transform.position, Quaternion.identity);
                newTurret.transform.parent = GameObject.Find("ContainerTurrets").transform.Find("ContainerTurrets1").transform;
                gold -= coutTourelle1;
            }
            else if (previsuTourelleEnCours.name == "PrevisuTourelle2(Clone)")
            {
                GameObject newTurret = Instantiate(tourelle2Prefab, previsuTourelleEnCours.transform.position, Quaternion.identity);
                newTurret.transform.parent = GameObject.Find("ContainerTurrets").transform.Find("ContainerTurrets2").transform;
                gold -= coutTourelle2;
            }
            else if (previsuTourelleEnCours.name == "PrevisuTourelle3(Clone)")
            {
                GameObject newTurret = Instantiate(tourelle3Prefab, previsuTourelleEnCours.transform.position, Quaternion.identity);
                newTurret.transform.parent = GameObject.Find("ContainerTurrets").transform.Find("ContainerTurrets3").transform;
                gold -= coutTourelle3;
            }
            else if (previsuTourelleEnCours.name == "PrevisuTourelle4(Clone)")
            {
                GameObject newTurret = Instantiate(tourelle4Prefab, previsuTourelleEnCours.transform.position, Quaternion.identity);
                newTurret.transform.parent = GameObject.Find("ContainerTurrets").transform.Find("ContainerTurrets4").transform;
                gold -= coutTourelle4;
            }
        }
        Destroy(previsuTourelleEnCours);
    }

    public void LancerPartie(int niveau)
    {
        level = niveau;
        menu.SetActive(false);
        partieEnCours = true;
        DeclancherProchaineVague();
    }

    public void FinDePartie()
    {
        partieEnCours = false;
         GameObject[] newList = GameObject.FindGameObjectsWithTag("Ennemi");
        foreach(GameObject go in newList)
        {
            Destroy(go);
        }
        newList = GameObject.FindGameObjectsWithTag("Tourelle");
        foreach (GameObject go in newList)
        {
            Destroy(go);
        }
        score += vie;
        if(score > meilleurScore)
        {
            meilleurScore = score;
            meilleurScoreTxt.text = meilleurScore.ToString();
            GetComponent<DataBase>().MajScore();
        }
        if(vague > niveauMax)
        {
            niveauMax = vague;
            meilleurNiveauTxt.text = niveauMax.ToString();
            GetComponent<DataBase>().MajNiveau();
        }
        FinDePartieTxt.text = "Partie terminée, votre score est de " + score;
        FinDePartieTxt.gameObject.SetActive(true);
        menu.SetActive(true);
        gold = 50;
        vie = 30;
        vague = 0;
        score = 0;
    }
}