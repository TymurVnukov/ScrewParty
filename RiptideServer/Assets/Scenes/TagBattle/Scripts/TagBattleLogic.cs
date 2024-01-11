using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class TagBattleLogic : MonoBehaviour
{
    private static TagBattleLogic _singleton;
    public static TagBattleLogic Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(TagBattleLogic)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }
    private void Awake()
    {
        Singleton = this;
    }

    public int[] points = new int[] { 0, 0, 0, 0 };
    private void Start()
    {
        InGameData.Singleton.CurrentScene = "TagBattle";
        StartCoroutine(Timer());
        if (Player.list.TryGetValue(1, out Player player1))
        {
            player1.Movement.tp = new Vector3(-1.25f, 1.25f, -4.25f);
            player1.Movement.isFreeze = true;
        }
        if (Player.list.TryGetValue(2, out Player player2))
        {
            player2.Movement.tp = new Vector3(-1.25f, 1.25f, -5f);
            player2.Movement.isFreeze = true;
        }
        if (Player.list.TryGetValue(3, out Player player3))
        {
            player3.Movement.tp = new Vector3(1.25f, 1.25f, -5f);
            player3.Movement.isFreeze = true;
        }
        if (Player.list.TryGetValue(4, out Player player4))
        {
            player4.Movement.tp = new Vector3(1.25f, 1.25f, -4.25f);
            player4.Movement.isFreeze = true;
        }
    }

    private IEnumerator Timer()
    {
        for (int i = 3; i > 0; i--)
        {
            SendTimerTick((ushort)i);
            yield return new WaitForSeconds(1);
        }
        foreach (Player player in Player.list.Values)
            player.GetComponent<TagManager>().isTaged = false;

        ushort randPlayer = (ushort)Random.Range(1, Player.list.Count);
        Player.list[randPlayer].GetComponent<TagManager>().isTaged = true;
        SendPlayerTag(randPlayer);

        if (Player.list.TryGetValue(1, out Player playerUnFreez1))
            playerUnFreez1.Movement.isFreeze = false;
        if (Player.list.TryGetValue(2, out Player playerUnFreez2))
            playerUnFreez2.Movement.isFreeze = false;
        if (Player.list.TryGetValue(3, out Player playerUnFreez3))
            playerUnFreez3.Movement.isFreeze = false;
        if (Player.list.TryGetValue(4, out Player playerUnFreez4))
            playerUnFreez4.Movement.isFreeze = false;

        for (int i = 60; i >= 0; i--)
        {
            SendTimerTick((ushort)i);
            yield return new WaitForSeconds(1);
            Debug.Log(i);
            if (i == 1 || i == 20 || i == 40)
            {
                ushort getId = 0;
                foreach (Player player in Player.list.Values)
                {
                    if (player.GetComponent<TagManager>().isTaged == true)
                    {
                        if (Player.list.TryGetValue(player.Id, out Player playerKilled))
                        {
                            playerKilled.Movement.tp = new Vector3(1000, 0, 1000);
                            getId = player.Id;
                        }
                    }
                    else
                    {
                        points[player.Id - 1] += 250;
                    }
                }
                SendPlayerKill(getId, points);
                foreach (Player player in Player.list.Values)
                {
                    if (player.GetComponent<TagManager>().isTaged == false)
                    {
                        player.GetComponent<TagManager>().isTaged = true;
                        SendPlayerTag(player.Id);
                        break;
                    }
                }
            }
        }
        for (int i = 3; i > 0; i--)
        {
            SendTimerTick((ushort)i);
            yield return new WaitForSeconds(1);
        }
        for (int i = 0; i < 4; i++)
        {
            InGameData.Singleton.points[i] += points[i];
        }
        NetworkManager.Singleton.SendSwitchScene("ModeChoise");
    }

    private void SendTimerTick(ushort timeLeft)
    {
        NetworkManager.Singleton.Server.SendToAll(SendTimerTickMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.tagBattle_TimerTick), timeLeft));
    }
    private Message SendTimerTickMessage(Message message, ushort timeLeft)
    {
        message.AddUShort(timeLeft);
        return message;
    }
    public void SendPlayerTag(ushort playerId)
    {
        NetworkManager.Singleton.Server.SendToAll(SendPlayerTagMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.tagBattle_PlayerTag), playerId));
    }
    private Message SendPlayerTagMessage(Message message, ushort playerId)
    {
        message.AddUShort(playerId);
        return message;
    }

    public void SendPlayerKill(ushort playerId, int[] points)
    {
        NetworkManager.Singleton.Server.SendToAll(SendPlayerKillMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.tagBattle_PlayerKill), playerId, points));
    }
    private Message SendPlayerKillMessage(Message message, ushort playerId, int[] points)
    {
        message.AddUShort(playerId);
        for(int i = 0; i < points.Length; i++)
        {
            message.AddInt(points[i]);
        }
        return message;
    }
}