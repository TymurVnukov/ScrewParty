using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultSceneLogic : MonoBehaviour
{
    void Start()
    {
        SendScoring();
        StartCoroutine(GameEnd());
    }
    private IEnumerator GameEnd()
    {
        if (Player.list.TryGetValue(1, out Player player1))
        {
            player1.Movement.tp = new Vector3(-4f, -4.5f, 0);
            player1.Movement.isFreeze = true;
        }
        if (Player.list.TryGetValue(2, out Player player2))
        {
            player2.Movement.tp = new Vector3(-2.75f, -4.5f, 1f);
            player2.Movement.isFreeze = true;
        }
        if (Player.list.TryGetValue(3, out Player player3))
        {
            player3.Movement.tp = new Vector3(-1.5f, -4.5f, 1.75f);
            player3.Movement.isFreeze = true;
        }
        if (Player.list.TryGetValue(4, out Player player4))
        {
            player4.Movement.tp = new Vector3(2.25f, -4.5f, 1.5f);
            player4.Movement.isFreeze = true;
        }

        yield return new WaitForSeconds(5);

        if (Player.list.TryGetValue(1, out Player player11))
        {
            player11.Movement.tp = new Vector3(-4f, -4.5f, 0);
            player11.Movement.isFreeze = false;
        }
        if (Player.list.TryGetValue(2, out Player player21))
        {
            player21.Movement.tp = new Vector3(-2.75f, -4.5f, 1f);
            player21.Movement.isFreeze = false;
        }
        if (Player.list.TryGetValue(3, out Player player31))
        {
            player31.Movement.tp = new Vector3(-1.5f, -4.5f, 1.75f);
            player31.Movement.isFreeze = false;
        }
        if (Player.list.TryGetValue(4, out Player player41))
        {
            player41.Movement.tp = new Vector3(2.25f, -4.5f, 1.5f);
            player41.Movement.isFreeze = false;
        }
    }
    private void SendScoring()
    {
        NetworkManager.Singleton.Server.SendToAll(SendScoringMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.resultScene_Scoring)));
    }
    private Message SendScoringMessage(Message message)
    {
        message.AddInt(InGameData.Singleton.points[0]);
        message.AddInt(InGameData.Singleton.points[1]);
        message.AddInt(InGameData.Singleton.points[2]);
        message.AddInt(InGameData.Singleton.points[3]);
        return message;
    }
}
