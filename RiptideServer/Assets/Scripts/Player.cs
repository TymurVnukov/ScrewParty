using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using RiptideNetworking.Utils;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public string Username { get; private set; }
    public int HeadID { get; private set; }
    public int BodyID { get; private set; }
    public int HandsID { get; private set; }
    public int LegsID { get; private set; }

    public PlayerMovement Movement => movement;

    [SerializeField] private PlayerMovement movement;

    private void OnDestroy()
    {
        list.Remove(Id);
    }
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public static void Spawn(ushort id, string username, int HeadID, int BodyID, int HandsID, int LegsID)
    {
        foreach (Player otherPlayer in list.Values)
            otherPlayer.SendSpawned(id);

        Player player = Instantiate(GameLogic.Singleton.PlayerPrefab, new Vector3(0f, 1f, 0f), Quaternion.identity).GetComponent<Player>();
        player.name = $"Player {id} {(string.IsNullOrEmpty(username) ? "Guest" : username)}";
        player.Id = id;
        player.Username = string.IsNullOrEmpty(username) ? $"Guest {id}" : username;
        player.HeadID = HeadID;
        player.BodyID = BodyID;
        player.HandsID = HandsID;
        player.LegsID = LegsID;

        player.SendSpawned();
        list.Add(id, player);
        foreach (Player _player in list.Values)
        {
            InGameData.Singleton.Players_Username[_player.Id - 1] = _player.Username;
            InGameData.Singleton.Players_HeadID[_player.Id - 1] = _player.HeadID;
            InGameData.Singleton.Players_BodyID[_player.Id - 1] = _player.BodyID;
            InGameData.Singleton.Players_HandsID[_player.Id - 1] = _player.HandsID;
            InGameData.Singleton.Players_LegsID[_player.Id - 1] = _player.LegsID;
        }
        SendPlayersSkins();
    }

    #region Messages

    public static void SendPlayersSkins()
    {
        NetworkManager.Singleton.Server.SendToAll(SendPlayersSkinsMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.playersSkins)));
    }
    private static Message SendPlayersSkinsMessage(Message message)
    {
        message.AddInt(list.Count);
        foreach (Player _player in list.Values)
        {
            message.AddInt(_player.HeadID);
            message.AddInt(_player.BodyID);
            message.AddInt(_player.HandsID);
            message.AddInt(_player.LegsID);
        }
        return message;
    }

    private void SendSpawned()
    {
        NetworkManager.Singleton.Server.SendToAll(AddSpawnData(Message.Create(MessageSendMode.reliable, ServerToClientId.playerSpawned)));
    }

    private void SendSpawned(ushort toClientId)
    {
        NetworkManager.Singleton.Server.Send(AddSpawnData(Message.Create(MessageSendMode.reliable, ServerToClientId.playerSpawned)), toClientId);
    }
    private Message AddSpawnData(Message message)
    {
        message.AddUShort(Id);
        message.AddString(Username);
        message.AddVector3(transform.position);
        return message;
    }

    [MessageHandler((ushort)ClientToServerId.name)]
    private static void Name(ushort fromClientId, Message message)
    {
        Spawn(fromClientId, message.GetString(), message.GetInt(), message.GetInt(), message.GetInt(), message.GetInt());
    }

    [MessageHandler((ushort)ClientToServerId.input)]
    private static void Input(ushort fromClientId, Message message)
    {
        if (list.TryGetValue(fromClientId, out Player player))
            player.Movement.SetInput(message.GetBools(6), message.GetVector3());
    }
    #endregion
}
