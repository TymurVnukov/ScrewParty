using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShoppingTimeLogic : MonoBehaviour
{
    public int[] points = new int[] { 0, 0, 0, 0 };
    public GameObject Milk;
    public GameObject[] SpawnMilkPoints;

    private void Start()
    {
        InGameData.Singleton.CurrentScene = "Breaking floor";
        StartCoroutine(StartGame());
        StartCoroutine(Timer());
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
        for(int i = 0; i < SpawnMilkPoints.Length; i++)
        {
            if (Random.Range(0, 10) == 0)
            {
                Instantiate(Milk, SpawnMilkPoints[i].transform.position, Quaternion.identity);
                SendMilkSpawn(SpawnMilkPoints[i].transform.position, SpawnMilkPoints[i].transform.rotation.y);
            }
        }


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

    private void SendMilkSpawn(Vector3 position, float rotate)
    {
        NetworkManager.Singleton.Server.SendToAll(SendMilkSpawnMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.shoppingTime_MilkSpawn), position, rotate));
    }
    private Message SendMilkSpawnMessage(Message message, Vector3 position, float rotate)
    {
        message.AddFloat(position.x);
        message.AddFloat(position.y);
        message.AddFloat(position.z);
        message.AddFloat(rotate);
        return message;
    }

    private void SendTimerTick(ushort timeLeft, int points1, int points2, int points3, int points4)
    {
        NetworkManager.Singleton.Server.SendToAll(SendTimerTickMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.shoppingTime_TimerTick), timeLeft, points1, points2, points3, points4));
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
