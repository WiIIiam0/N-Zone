using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.IO;

public class DataBase : MonoBehaviour
{
    private string host = "mysql-nzonetest.alwaysdata.net";
    private string database = "nzonetest_nzonetest";
    private string username = "nzonetest";
    private string password = "Test123456789!";
    MySqlConnection con;
    void ConnectBDD()
    {
        string constr = "Server=" + host + ";DATABASE=" + database + ";User ID=" + username + ";Password=" + password + ";Pooling=true;Charset=utf8;";
        try
        {
            con = new MySqlConnection(constr);
            con.Open();
        }
        catch (IOException Ex)
        {
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Shutdown Connexion");

        if (con != null && con.State.ToString() != "Closed")
        {
            con.Close();
        }
    }

    public void Register(string nom, string prenom, string mail, string pseudo)
    {
        ConnectBDD();

        bool Exist = false;

        MySqlCommand commandsql = new MySqlCommand("SELECT pseudo FROM users WHERE pseudo = '" + pseudo + "'", con);
        MySqlDataReader MyReader = commandsql.ExecuteReader();

        while (MyReader.Read())
        {
            if (MyReader["pseudo"].ToString() != "")
            {
                GetComponent<Connexion>().alerteInscription.text = "Ce pseudo est déjà utilisé";
                Exist = true;
            }
        }
        MyReader.Close();

        if (!Exist)
        {
            string command = "INSERT INTO users VALUES (default,'" + nom + "','" + prenom +"','" + mail +"','" + pseudo + "', '0', '0')";
            MySqlCommand cmd = new MySqlCommand(command, con);

            try
            {
                cmd.ExecuteReader();
                GetComponent<Connexion>().alerteInscription.text = "Inscription réussie";
                GetComponent<Manager>().pseudo = GetComponent<Connexion>().fieldPseudo.text;
            }
            catch (IOException Ex)
            {
                Debug.Log(Ex.ToString());
            }

            cmd.Dispose();
            con.Close();
        }
    }

    public void Connexion(string pseudo)
    {
        ConnectBDD();
        MySqlCommand commandsql = new MySqlCommand("SELECT * FROM users WHERE pseudo = '" + pseudo + "'", con);
        MySqlDataReader MyReader = commandsql.ExecuteReader();
        while (MyReader.Read())
        {
            if(MyReader["pseudo"].ToString() != "")
            {
                GetComponent<Manager>().pseudo = MyReader["pseudo"].ToString();
                GetComponent<Manager>().niveauMax = (int)MyReader["niveau"];
                GetComponent<Manager>().meilleurNiveauTxt.text = GetComponent<Manager>().niveauMax.ToString();
                GetComponent<Manager>().meilleurScore = (int)MyReader["score"];
                GetComponent<Manager>().meilleurScoreTxt.text = GetComponent<Manager>().meilleurScore.ToString();
                GameObject.Find("Inscription").SetActive(false);
                GetComponent<Manager>().menu.SetActive(true);
            }
        }
        con.Close();
        if (GetComponent<Manager>().pseudo == "")
        {
            GetComponent<Connexion>().alerteConnexion.text = "Pseudo inexistant";
        }
    }

    public void MajNiveau()
    {
        ConnectBDD();
        string command = "UPDATE users SET niveau = '" +GetComponent<Manager>().niveauMax + "' WHERE pseudo = '" + GetComponent<Manager>().pseudo + "';";
        MySqlCommand cmd = new MySqlCommand(command, con);

        try
        {
            cmd.ExecuteReader();
        }
        catch (IOException Ex)
        {
        }

        cmd.Dispose();
        con.Close();
    }

    public void MajScore()
    {
        ConnectBDD();
        string command = "UPDATE users SET score = '" + GetComponent<Manager>().meilleurScore + "' WHERE pseudo = '" + GetComponent<Manager>().pseudo + "';";
        MySqlCommand cmd = new MySqlCommand(command, con);

        try
        {
            cmd.ExecuteReader();
        }
        catch (IOException Ex)
        {
        }

        cmd.Dispose();
        con.Close();

    }
}