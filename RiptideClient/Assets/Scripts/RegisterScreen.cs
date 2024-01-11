using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class RegisterScreen : MonoBehaviour
{
    private string dbName = "URI=file:playerData.db";

    public GameObject ErrorPosition;
    public GameObject ErrorMessage;

    public GameObject UsernameField;
    public GameObject RegisterField;

    private static TextMeshProUGUI UsernameTMP;
    private static TextMeshProUGUI RegisterTMP;

    private void Start()
    {
        UsernameTMP = UsernameField.GetComponent<TextMeshProUGUI>();
        RegisterTMP = RegisterField.GetComponent<TextMeshProUGUI>();
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT Id,Username,Password,isStayLogIn FROM Register";
                SqliteDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    int id = Convert.ToInt32(r[0]);
                    string username = (string)r[1];
                    string password = (string)r[2];
                    string isStayLogIn = (string)r[3];
                    if (isStayLogIn == "true")
                    {
                        UIManager.Singleton.Username = username;
                        UIManager.Singleton.playUI.SetActive(true);
                        UIManager.Singleton.registerUI.SetActive(false);
                        break;
                    }
                }
            }
            connection.Close();
        }
    }

    public void Registering()
    {
        if (UsernameTMP.text.Length > 3 && RegisterTMP.text.Length > 3) {
            using (var connection = new SqliteConnection(dbName))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"DELETE FROM Register";
                    command.ExecuteNonQueryAsync();
                    command.CommandText = $"INSERT INTO Register (Username, Password, isStayLogIn) VALUES('{UsernameTMP.text}', '{RegisterTMP.text}', 'true')";
                    command.ExecuteNonQueryAsync();
                }
                connection.Close();
                UIManager.Singleton.Username = UsernameTMP.text;
                UIManager.Singleton.playUI.SetActive(true);
                UIManager.Singleton.registerUI.SetActive(false);
            }
            Debug.Log("Register");
        }
    }

    public void LogIn()
    {
        bool isLogin = false;
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT Id,Username,Password,isStayLogIn FROM Register";
                SqliteDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    int id = Convert.ToInt32(r[0]);
                    string username = (string)r[1];
                    string password = (string)r[2];
                    Debug.Log($"{id} {username} == {UsernameTMP.text} && {password} == {RegisterTMP.text}");
                    if (username == UsernameTMP.text && password == RegisterTMP.text)
                    {
                        Debug.Log("Enter!");
                        UIManager.Singleton.Username = username;
                        isLogin = true;
                        break;
                    }
                }
            }
            connection.Close();
        }
        if (!isLogin)
        {
            GameObject tmp = Instantiate(ErrorMessage, ErrorPosition.transform);
            Destroy(tmp, 3f);
        }
        else
        {
            using (var connection1 = new SqliteConnection(dbName))
            {
                connection1.Open();
                using (var command1 = connection1.CreateCommand())
                {
                    command1.CommandText = $"UPDATE Register SET isStayLogIn = 'true' WHERE Username = '{UsernameTMP.text}'";
                    command1.ExecuteNonQueryAsync();
                }
                connection1.Close();
            }
            Debug.Log("Update!");
            UIManager.Singleton.playUI.SetActive(true);
            UIManager.Singleton.registerUI.SetActive(false);
        }
    }
}
