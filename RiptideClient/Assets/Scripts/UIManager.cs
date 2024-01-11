using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;


public class UIManager : MonoBehaviour
{
    private string dbName = "URI=file:playerData.db";

    private static UIManager _singleton;
    public static UIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }
    public string Username = "";
    [Header("Connect")]
    [SerializeField] public GameObject registerUI;
    [SerializeField] public GameObject playUI;
    [SerializeField] public GameObject settingsUI;
    [SerializeField] public GameObject connectUI;
    [SerializeField] public GameObject lockerUI;
    [SerializeField] public GameObject shopUI;

    private void Awake()
    {
        Singleton = this;
    }
    public void LogOut()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"UPDATE Register SET isStayLogIn = 'false' WHERE Username = '{Username}'";
                command.ExecuteNonQueryAsync();
            }
            connection.Close();
        }

        registerUI.SetActive(true);
        playUI.SetActive(false);
    }
    public void OpenLocker()
    {
        playUI.SetActive(false);
        lockerUI.SetActive(true);
    }
    public void OpenSettings()
    {
        playUI.SetActive(false);
        settingsUI.SetActive(true);
    }
    public void CloseSettings()
    {
        settingsUI.SetActive(false);
        playUI.SetActive(true);
    }
    public void OpenShop()
    {
        connectUI.SetActive(false);
        shopUI.SetActive(true);
    }

    public void OpenConnect()
    {
        playUI.SetActive(false);
        connectUI.SetActive(true);
    }
    public void CloseConnectUI()
    {
        connectUI.SetActive(false);
        playUI.SetActive(true);
    }

    public void ConnectClicked()
    {
        connectUI.SetActive(false);

        NetworkManager.Singleton.Connect();
    }
    public void ConnectLocalServer()
    {
        NetworkManager.Singleton.ip = "127.0.0.1";
        NetworkManager.Singleton.Connect();
        SceneManager.LoadScene("GameLobby");
    }
    public void ConnectDedicatedServer()
    {
        NetworkManager.Singleton.ip = "18.192.38.161";
        NetworkManager.Singleton.Connect();
        SceneManager.LoadScene("GameLobby");
    }
    public void ConnectMyPCServer()
    {
        NetworkManager.Singleton.ip = "159.224.231.148";
        NetworkManager.Singleton.Connect();
        SceneManager.LoadScene("GameLobby");
    }

    public void BackToMain()
    {
        playUI.SetActive(true);
        registerUI.SetActive(false);
        settingsUI.SetActive(false);
        connectUI.SetActive(false);
        shopUI.SetActive(false);
        lockerUI.SetActive(false);
    }
    public void SendName()
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.name);
        message.AddString(Username);
        message.AddInt(PlayerLocker.HeadID);
        message.AddInt(PlayerLocker.BodyID);
        message.AddInt(PlayerLocker.HandsID);
        message.AddInt(PlayerLocker.LegsID);
        NetworkManager.Singleton.Client.Send(message);
    }
}
