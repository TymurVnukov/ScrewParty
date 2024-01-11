using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using RiptideNetworking.Utils;
using System;
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
    public Client Client { get; private set; }

    //private ushort _serverTick;
    //public ushort ServerTick
    //{
    //    get => _serverTick;
    //    private set
    //    {
    //        _serverTick = value;
    //        InterpolationTick = (ushort)(value - TicksBetweenPositionUpdates);
    //    }
    //}
    //private ushort _ticksBetweenPositionUpdates = 2;
    //public ushort TicksBetweenPositionUpdates
    //{
    //    get => _ticksBetweenPositionUpdates;
    //    private set
    //    {
    //        _ticksBetweenPositionUpdates = value;
    //        InterpolationTick = (ushort)(ServerTick - value);
    //    }
    //}
    //public ushort InterpolationTick { get; private set; }

    [SerializeField] public string ip;
    [SerializeField] public ushort port;
    //[Space(10)]
    //[SerializeField] private ushort tickDivergenceTolerance = 1;

    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        Client = new Client();
        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailedToConnect;
        Client.ClientDisconnected += PlayerLeft;
        Client.Disconnected += DidDisconnect;

        //ServerTick = 2;
    }
    private void FixedUpdate()
    {
        Client.Tick();
        //ServerTick++;
    }
    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }
    public void Connect()
    {
        Client.Connect($"{ip}:{port}");
    }
    private void DidConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.SendName();
    }
    private void FailedToConnect(object sender, EventArgs e)
    {
        SceneManager.LoadScene("Lobby");
    }
    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        Destroy(Player.list[e.Id].gameObject);
        if (SceneManager.GetActiveScene().name == "GameLobby")
        {
            GameLobbyUIManager.Singleton.AddPlayer(e.Id, "none");
        }
    }
    private void DidDisconnect(object sender, EventArgs e)
    {
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Player").Length; i++)
        {
            Destroy(GameObject.FindGameObjectsWithTag("Player")[i]);
        }
        SceneManager.LoadScene("Lobby");
        //UIManager.Singleton.BackToMain();
    }

    [MessageHandler((ushort)ServerToClientId.switchScene)]
    private static void SwitchScene(Message message)
    {
        SceneManager.LoadScene(message.GetString());
    }


    //private void SetTick(ushort serverTick)
    //{
    //    if (Mathf.Abs(ServerTick - serverTick) > tickDivergenceTolerance)
    //    {
    //        Debug.Log($"Client tick: {ServerTick} -> {serverTick}");
    //        ServerTick = serverTick;
    //    }
    //}
    //[MessageHandler((ushort)ServerToClientId.sync)]
    //public static void Sync(Message message)
    //{
    //    Singleton.SetTick(message.GetUShort());
    //}
}
