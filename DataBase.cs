using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.IO;

public class Database : MonoBehaviour
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
            if (MyReader["name"].ToString() != "")
            {
                GetComponent<Connexion>().alerteInscription.text = "Ce pseudo est déjà utilisé";
                Exist = true;
            }
        }
        MyReader.Close();

        if (!Exist)
        {
            string command = "INSERT INTO users VALUES (default,'" + nom + "','" + prenom +"','" + mail + "','" + mail +"','" + pseudo + "', '0', '0')";
            MySqlCommand cmd = new MySqlCommand(command, con);

            try
            {
                cmd.ExecuteReader();
                Debug.Log("Register succesfull");
            }
            catch (IOException Ex)
            {
                Debug.Log(Ex.ToString());
            }

            cmd.Dispose();
            con.Close();
        }
    }
}