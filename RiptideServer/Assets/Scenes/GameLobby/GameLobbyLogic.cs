using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLobbyLogic : MonoBehaviour
{
    private static GameLobbyLogic _singleton;
    public static GameLobbyLogic Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(GameLobbyLogic)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }
    private void Awake()
    {
        Singleton = this;
    }

    public static bool[] isReady = { false, false, false, false };

    [MessageHandler((ushort)ClientToServerId.readyUp)]
    private static void Input(ushort fromClientId, Message message)
    {
        isReady[fromClientId - 1] = true;
        if (isReady[0] && isReady[1] && isReady[2] && isReady[3] || isReady[0])
        {
            NetworkManager.Singleton.SendSwitchScene("ModeChoise");//Breaking floor | Labirint | ModeChoise
        }
        else
        {
            SendReady();
        }
    }

    public static void SendReady()
    {
        NetworkManager.Singleton.Server.SendToAll(SendReadyMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.readyUp)));
    }
    private static Message SendReadyMessage(Message message)
    {
        message.AddBool(isReady[0]);
        message.AddBool(isReady[1]);
        message.AddBool(isReady[2]);
        message.AddBool(isReady[3]);
        return message;
    }
}
