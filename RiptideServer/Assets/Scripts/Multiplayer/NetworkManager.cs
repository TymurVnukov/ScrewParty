using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine.SceneManagement;

public enum ServerToClientId : ushort
{
    sync = 1,
    playerSpawned,
    playerMovement,
    readyUp,
    playersSkins,
    switchScene,
    breakingFloor_breakBox,
    breakingFloor_TimerTick,
    labirint_blockPosition,
    labirint_CutoutPosition,
    choiseMode,
    ModeChoise_TimerTick,
    labirint_TimerTick,
    tagBattle_TimerTick,
    tagBattle_PlayerTag,
    tagBattle_PlayerKill,
    ModeChoise_PlayersScore,
    shoppingTime_TimerTick,
    shoppingTime_MilkSpawn,
    wordCollectors_TimerTick,
    wordCollectors_Letter,
    wordCollectors_Word,
    inGameData_endGame,
    resultScene_Scoring,
}
public enum ClientToServerId : ushort
{
    name = 1,
    input,
    readyUp,
    choiseMode,
}
public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;
    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public Server Server { get; private set; }
    //public ushort CurrentTick { get; private set; } = 0;
    [SerializeField] private ushort port;
    [SerializeField] private ushort maxClientCount;

    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Server = new Server();
        Server.Start(port, maxClientCount);
        Server.ClientDisconnected += PlayerLeft;
    }
    private void FixedUpdate()
    {
        Server.Tick();

        //if(CurrentTick % 200 == 0 )
        //{
        //    SendSync();
        //}

        //CurrentTick++;
    }
    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        Destroy(Player.list[e.Id].gameObject);
    }

    public void SendSwitchScene(string newScene)
    {
        NetworkManager.Singleton.Server.SendToAll(SendSwitchSceneMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.switchScene), newScene));
        SceneManager.LoadScene(newScene);
    }
    public Message SendSwitchSceneMessage(Message message, string newScene)
    {
        message.AddString(newScene);
        return message;
    }

    //private void SendSync()
    //{
    //    Message message = Message.Create(MessageSendMode.unreliable, (ushort)ServerToClientId.sync);
    //    message.Add(CurrentTick);

    //    Server.SendToAll(message);
    //}
}
