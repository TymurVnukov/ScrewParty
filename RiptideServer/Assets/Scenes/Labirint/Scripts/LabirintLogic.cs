using JetBrains.Annotations;
using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;


public class LabirintLogic : MonoBehaviour
{
    public GameObject Block;
    public Vector2[] BlocksWay;
    public int WayPointsUsing;
    public int[] points = new int[] { 0, 0, 0, 0 };
    public bool[] isPlayerWin = new bool[] { false, false, false, false };
    [SerializeField] private float cutoutSizesSpeed = 0.03f;
    [SerializeField] private float cutoutSizes = 0.075f;
    [SerializeField] private bool direction = false;
    [SerializeField] private Vector2 cutoutSizesMaxMin = new Vector2(0.025f, 0.125f);
    void Start()
    {
        InGameData.Singleton.CurrentScene = "Labirint";
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
        BlocksWay = new Vector2[150];
        GameObject prevBlock = Instantiate(Block, new Vector3(0, 0, 0), Quaternion.identity);
        PlaceBlocks();
        PlaceBlocks();
        void PlaceBlocks()
        {
            BlocksWay = new Vector2[150];
            for (int i = 0; i < BlocksWay.Length; i++)
            {
                if (prevBlock.transform.position.z <= 101)
                {
                    int rand = Random.Range(0, 3);
                    if (prevBlock.transform.position.x >= 8)
                    {
                        rand = Random.Range(0, 3);
                        if (rand == 1)
                            rand = 0;
                    }
                    if (prevBlock.transform.position.x <= -8)
                    {
                        rand = Random.Range(1, 3);
                    }
                    GameObject block = null;
                    if (rand == 0)//left
                    {
                        block = Instantiate(Block, new Vector3(prevBlock.transform.position.x - 1.5f, 0, prevBlock.transform.position.z), Quaternion.identity);
                    }
                    if (rand == 1)//right
                    {
                        block = Instantiate(Block, new Vector3(prevBlock.transform.position.x + 1.5f, 0, prevBlock.transform.position.z), Quaternion.identity);
                    }
                    if (rand == 2)//top
                    {
                        block = Instantiate(Block, new Vector3(prevBlock.transform.position.x, 0, prevBlock.transform.position.z + 1.5f), Quaternion.identity);
                    }
                    BlocksWay[i] = new Vector2(block.transform.position.x, block.transform.position.z);
                    prevBlock = block;
                }
            }
            SendBlockPosition(BlocksWay);
        }
    }
    
    private void FixedUpdate()
    {
        foreach (Player player in Player.list.Values)
        {
            if(player.transform.position.y < -10f)
            {
                player.Movement.tp = new Vector3(0, 1.25f, -4f);
            }
            if (player.transform.position.z > 100f)
            {
                isPlayerWin[player.Id - 1] = true;
            }
            if(isPlayerWin[player.Id - 1] == true)
            {
                points[player.Id - 1] = 1000;
            }
        }

        if (cutoutSizes <= cutoutSizesMaxMin.x)
        {
            direction = false;
        }
        else if(cutoutSizes >= cutoutSizesMaxMin.y)
        {
            direction = true;
        }
        if (direction)
        {
            cutoutSizes -= Time.deltaTime * cutoutSizesSpeed;
        }
        else
        {
            cutoutSizes += Time.deltaTime * cutoutSizesSpeed;
        }
        SendCutoutPosition(cutoutSizes);
    }
    private void SendBlockPosition(Vector2[] positions)
    {
        NetworkManager.Singleton.Server.SendToAll(SendBlockPositionMessage(Message.Create(MessageSendMode.unreliable, ServerToClientId.labirint_blockPosition), positions));//reliable
    }
    private Message SendBlockPositionMessage(Message message, Vector2[] positions)
    {
        message.AddUShort((ushort)positions.Length);
        for (int i = 0; i < positions.Length; i++)
        {
            message.AddVector2(positions[i]);
        }
        return message;
    }
    private void SendCutoutPosition(float cutoutSize)
    {
        NetworkManager.Singleton.Server.SendToAll(SendCutoutPositionMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.labirint_CutoutPosition), cutoutSize));
    }
    private Message SendCutoutPositionMessage(Message message, float cutoutSize)
    {
        message.AddFloat(cutoutSize);
        return message;
    }

    private IEnumerator Timer()
    {
        for (int i = 3; i > 0; i--)
        {
            SendTimerTick((ushort)i);
            yield return new WaitForSeconds(1);
        }
        if (Player.list.TryGetValue(1, out Player playerUnFreez1))
            playerUnFreez1.Movement.isFreeze = false;
        if (Player.list.TryGetValue(2, out Player playerUnFreez2))
            playerUnFreez2.Movement.isFreeze = false;
        if (Player.list.TryGetValue(3, out Player playerUnFreez3))
            playerUnFreez3.Movement.isFreeze = false;
        if (Player.list.TryGetValue(4, out Player playerUnFreez4))
            playerUnFreez4.Movement.isFreeze = false;
        for (int i = 60; i >= 0; i--)//100
        {
            SendTimerTick((ushort)i);
            yield return new WaitForSeconds(1);
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
        NetworkManager.Singleton.Server.SendToAll(SendTimerTickMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.labirint_TimerTick), timeLeft));
    }
    private Message SendTimerTickMessage(Message message, ushort timeLeft)
    {
        message.AddUShort(timeLeft);
        return message;
    }
}
