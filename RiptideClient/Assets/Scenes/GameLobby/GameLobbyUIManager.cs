using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameLobbyUIManager : MonoBehaviour
{
    private static GameLobbyUIManager _singleton;
    public static GameLobbyUIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(GameLobbyUIManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }
    private void Awake()
    {
        Singleton = this;
    }

    public void GetReady()
    {
        SendReady(NetworkManager.Singleton.Client.Id);
    }

    public GameObject[] Usernames;
    public GameObject[] IsReady;

    public void AddPlayer(int id, string username)
    {
        Usernames[id - 1].GetComponent<TextMeshProUGUI>().text = username;
    }

    public void SendReady(ushort id)
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.readyUp);
        message.AddUShort(id);
        NetworkManager.Singleton.Client.Send(message);
    }

    [MessageHandler((ushort)ServerToClientId.readyUp)]
    private static void PlayerMovement(Message message)
    {
        for (int i = 0; i < 4; i++)
        {
            if (message.GetBool())
            {
                Singleton.IsReady[i].GetComponent<TextMeshProUGUI>().text = "Ready!";
                Singleton.IsReady[i].GetComponent<TextMeshProUGUI>().color = Color.green;
            }
        }
    }

}
