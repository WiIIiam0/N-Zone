using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Connexion : MonoBehaviour
{
    public TMP_InputField fieldNom;
    public TMP_InputField fieldPrenom;
    public TMP_InputField fieldMail;
    public TMP_InputField fieldPseudo;
    public TextMeshProUGUI alerteInscription;
    public TextMeshProUGUI alerteConnexion;
    public TMP_InputField fieldConnexion;
    public void Inscription()
    {
        if(fieldNom.text != "" && fieldPrenom.text != "" && fieldMail.text != "" && fieldPseudo.text != "")
        {
            Debug.Log(fieldPrenom.text);
            GetComponent<DataBase>().Register(fieldNom.text, fieldPrenom.text, fieldMail.text,  fieldPseudo.text);
        }
        else
        {
            alerteInscription.text = "Champ vide";
        }
    }

    public void NouvelleConnexion()
    {
        GetComponent<DataBase>().Connexion(fieldConnexion.text);
    }
}
