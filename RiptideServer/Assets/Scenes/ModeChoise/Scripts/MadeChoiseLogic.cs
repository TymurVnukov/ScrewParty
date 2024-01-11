using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MadeChoiseLogic : MonoBehaviour
{
    [SerializeField] private string[] allModes;
    [SerializeField] private string[] PlayerChoise = { "none", "none", "none", "none" };
    public string resultMode = "none";

    private static MadeChoiseLogic _singleton;
    public static MadeChoiseLogic Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(MadeChoiseLogic)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }
    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        InGameData.Singleton.MiniGamePlayed++;
        StartCoroutine(GameEnd());
        StartCoroutine(Timer());
    }
    private IEnumerator GameEnd()
    {
        SendEndGame(InGameData.Singleton.MiniGamePlayed);
        yield return new WaitForSeconds(5);
        if(InGameData.Singleton.MiniGamePlayed == 5)//5
        {
            NetworkManager.Singleton.SendSwitchScene("ResultScene");
        }
    }

    private static void SendEndGame(int MiniGamePlayed)
    {
        NetworkManager.Singleton.Server.SendToAll(SendEndGameMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.inGameData_endGame), MiniGamePlayed));
    }
    private static Message SendEndGameMessage(Message message, int MiniGamePlayed)
    {
        message.AddInt(MiniGamePlayed);
        return message;
    }

    [MessageHandler((ushort)ClientToServerId.choiseMode)]
    private static void Input(ushort fromClientId, Message message)
    {
        string name = message.GetString();
        Singleton.PlayerChoise[fromClientId - 1] = name;
        Singleton.SendChoiseMode("none");
    }

    private void SendChoiseMode(string resultMode)
    {
        NetworkManager.Singleton.Server.SendToAll(SendChoiseModeMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.choiseMode), PlayerChoise, resultMode));
    }
    private Message SendChoiseModeMessage(Message message, string[] PlayerChoise, string resultMode)
    {
        message.AddString(PlayerChoise[0]);
        message.AddString(PlayerChoise[1]);
        message.AddString(PlayerChoise[2]);
        message.AddString(PlayerChoise[3]);
        message.AddString(resultMode);
        return message;
    }
    private IEnumerator Timer()
    {
        SendPlayersScore(InGameData.Singleton.points);

        for (int i = 5; i > 0; i--)//20
        {
            SendTimerTick((ushort)i);
            yield return new WaitForSeconds(1);
        }
        resultMode = "none";
        if(PlayerChoise[0] == "none" && PlayerChoise[1] == "none" && PlayerChoise[2] == "none" && PlayerChoise[3] == "none")
        {
            PlayerChoise[0] = allModes[Random.Range(0, allModes.Length)];
        }
        if (resultMode == "none")
        {
            while (resultMode == "none")
            {
                int rand = Random.Range(0, 4);
                resultMode = PlayerChoise[rand];
            }
        }
        Singleton.SendChoiseMode(resultMode);
        for (int i = 5; i > 0; i--)//5
        {
            SendTimerTick((ushort)i);
            yield return new WaitForSeconds(1);
        }
        NetworkManager.Singleton.SendSwitchScene(resultMode);
    }

    private void SendTimerTick(ushort timeLeft)
    {
        NetworkManager.Singleton.Server.SendToAll(SendTimerTickMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.ModeChoise_TimerTick), timeLeft));
    }
    private Message SendTimerTickMessage(Message message, ushort timeLeft)
    {
        message.AddUShort(timeLeft);
        return message;
    }


    private void SendPlayersScore(int[] score)
    {
        NetworkManager.Singleton.Server.SendToAll(SendPlayersScoreMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.ModeChoise_PlayersScore), score));
    }
    private Message SendPlayersScoreMessage(Message message, int[] score)
    {
        message.AddInt(score[0]);
        message.AddInt(score[1]);
        message.AddInt(score[2]);
        message.AddInt(score[3]);
        return message;
    }


}
