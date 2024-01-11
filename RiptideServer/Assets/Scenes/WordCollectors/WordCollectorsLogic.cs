using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WordCollectorsLogic : MonoBehaviour
{
    public int[] points = new int[] { 0, 0, 0, 0 };

    public List<string> Words;

    public string MainWord = "";

    public static int MainWordCountPlayer1 = 0;
    public static int MainWordCountPlayer2 = 0;
    public static int MainWordCountPlayer3 = 0;
    public static int MainWordCountPlayer4 = 0;
    public static string[] MainWordChar;

    private static WordCollectorsLogic _singleton;
    public static WordCollectorsLogic Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(WordCollectorsLogic)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    private void Start()
    {
        MainWordCountPlayer1 = 0;
        MainWordCountPlayer2 = 0;
        MainWordCountPlayer3 = 0;
        MainWordCountPlayer4 = 0;
        for (int i = 0; i < 11; i++)
        {
            int rand = Random.Range(0, Words.Count - 1);
            MainWord += "." + Words[rand];
            Words.Remove(Words[rand]);
        }
        MainWordChar = new string[MainWord.Length];
        for (int i = 0; i < MainWord.Length; i++)
        {
            MainWordChar[i] = MainWord[i].ToString();
        }
        InGameData.Singleton.CurrentScene = "WordCollectors";
        StartCoroutine(StartGame());
        StartCoroutine(Timer());
    }
    private void Awake()
    {
        Singleton = this;
    }
    private IEnumerator Timer()
    {
        for (int i = 3; i > 0; i--)
        {
            SendTimerTick((ushort)i, points[0], points[1], points[2], points[3]);
            yield return new WaitForSeconds(1);
        }
        for (int i = 99; i >= 0; i--)//100
        {
            SendTimerTick((ushort)i, points[0], points[1], points[2], points[3]);
            yield return new WaitForSeconds(1);
        }
        for (int i = 3; i >= 0; i--)
        {
            SendTimerTick((ushort)i, points[0], points[1], points[2], points[3]);
            yield return new WaitForSeconds(1);
        }
        for (int i = 0; i < 4; i++)
        {
            InGameData.Singleton.points[i] += points[i];
        }
        NetworkManager.Singleton.SendSwitchScene("ModeChoise");
    }
    private IEnumerator StartGame()
    {
        if (Player.list.TryGetValue(1, out Player player1))
        {
            player1.Movement.tp = new Vector3(-7.5f, 1.7f, 3f);
            player1.Movement.isFreeze = true;
        }
        if (Player.list.TryGetValue(2, out Player player2))
        {
            player2.Movement.tp = new Vector3(7.5f, 1.7f, 3f);
            player2.Movement.isFreeze = true;
        }
        if (Player.list.TryGetValue(3, out Player player3))
        {
            player3.Movement.tp = new Vector3(-7.5f, 1.7f, -3f);
            player3.Movement.isFreeze = true;
        }
        if (Player.list.TryGetValue(4, out Player player4))
        {
            player4.Movement.tp = new Vector3(7.5f, 1.7f, -3f);
            player4.Movement.isFreeze = true;
        }

        yield return new WaitForSeconds(3);

        if (Player.list.TryGetValue(1, out Player playerUnFreez1))
            playerUnFreez1.Movement.isFreeze = false;
        if (Player.list.TryGetValue(2, out Player playerUnFreez2))
            playerUnFreez2.Movement.isFreeze = false;
        if (Player.list.TryGetValue(3, out Player playerUnFreez3))
            playerUnFreez3.Movement.isFreeze = false;
        if (Player.list.TryGetValue(4, out Player playerUnFreez4))
            playerUnFreez4.Movement.isFreeze = false;
    }
    private void SendTimerTick(ushort timeLeft, int points1, int points2, int points3, int points4)
    {
        NetworkManager.Singleton.Server.SendToAll(SendTimerTickMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.wordCollectors_TimerTick), timeLeft, points1, points2, points3, points4));
    }
    private Message SendTimerTickMessage(Message message, ushort timeLeft, int points1, int points2, int points3, int points4)
    {
        message.AddUShort(timeLeft);
        message.AddInt(points1);
        message.AddInt(points2);
        message.AddInt(points3);
        message.AddInt(points4);
        return message;
    }
}