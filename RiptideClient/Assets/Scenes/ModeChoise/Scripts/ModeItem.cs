using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeItem : MonoBehaviour
{
    public string choiseString;
    public void SendChoiseString(string name)
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.choiseMode);
        message.AddString(name);
        NetworkManager.Singleton.Client.Send(message);
    }
}
