using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BreakingFloorLogic : MonoBehaviour
{
    public int Time = 0;
    public GameObject Box;
    public GameObject[,] Boxes = new GameObject[7, 13];

    public const int BreakBloksCount = 30;

    public int[] points = new int[] { 0, 0, 0, 0 };

    private void Start()
    {
        InGameData.Singleton.CurrentScene = "Breaking floor";
        int X = 0, Y = 0;
        for (float y = -9f; y <= 9f; y += 1.5f)
        {
            for (float x = -4.5f; x <= 4.5f; x += 1.5f)
            {
                GameObject box = Instantiate(Box, new Vector3(y, 0, x), Box.transform.rotation, this.gameObject.transform);
                Boxes[X, Y] = box;
                X++;
            }
            Y++;
            X = 0;
        }
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
        for (int i = Time; i >= 0; i--)//100
        {
            foreach (Player player in Player.list.Values)
            {
                if (player.transform.position.y > -1f)
                {
                    points[player.Id - 1] += 10;
                }
            }

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

    private void SendTimerTick(ushort timeLeft, int points1, int points2, int points3, int points4)
    {
        NetworkManager.Singleton.Server.SendToAll(SendTimerTickMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.breakingFloor_TimerTick), timeLeft, points1, points2, points3, points4));
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

        Vector2[] breakPosition = new Vector2[BreakBloksCount];

        yield return new WaitForSeconds(1);
        for (int j = 0; j < breakPosition.Length; j++)
        {
            yield return new WaitForSeconds(1);
            for (int i = 0; i < breakPosition.Length; i++)
            {
                breakPosition[i] = new Vector2(Random.Range(0, 6), Random.Range(0, 12));
            }
            SendBreakBox(breakPosition);
            yield return new WaitForSeconds(1);
            for (int i = 0; i < breakPosition.Length; i++)
            {
                Boxes[(int)breakPosition[i].x, (int)breakPosition[i].y].active = false;
            }
            yield return new WaitForSeconds(5);
            for (int i = 0; i < breakPosition.Length; i++)
            {
                Boxes[(int)breakPosition[i].x, (int)breakPosition[i].y].active = true;
            }
        }
    }
    private void SendBreakBox(Vector2[] blockPosition)
    {
        NetworkManager.Singleton.Server.SendToAll(SendBreakBoxMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.breakingFloor_breakBox), blockPosition));
    }
    private Message SendBreakBoxMessage(Message message, Vector2[] blockPosition)
    {
        for (int i = 0; i < BreakBloksCount; i++)
        {
            message.AddVector2(blockPosition[i]);
        }
        return message;
    }
}
