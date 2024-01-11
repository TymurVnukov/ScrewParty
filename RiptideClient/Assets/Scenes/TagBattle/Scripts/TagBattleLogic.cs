using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TagBattleLogic : MonoBehaviour
{
    public GameObject[] scores;
    private static TextMeshProUGUI Points1TMP;
    private static TextMeshProUGUI Points2TMP;
    private static TextMeshProUGUI Points3TMP;
    private static TextMeshProUGUI Points4TMP;

    public GameObject Timer;
    public GameObject RoundEndUI;
    private static TextMeshProUGUI TimerTMP;
    private static GameObject RoundEndUISingle;
    private void Start()
    {
        RoundEndUISingle = RoundEndUI;
        TimerTMP = Timer.GetComponent<TextMeshProUGUI>();
        Points1TMP = scores[0].GetComponent<TextMeshProUGUI>();
        Points2TMP = scores[1].GetComponent<TextMeshProUGUI>();
        Points3TMP = scores[2].GetComponent<TextMeshProUGUI>();
        Points4TMP = scores[3].GetComponent<TextMeshProUGUI>();
    }
    [MessageHandler((ushort)ServerToClientId.tagBattle_TimerTick)]
    private static void TimerTick(Message message)
    {
        ushort timer = message.GetUShort();
        TimerTMP.text = timer.ToString();
        if (timer == 0)
        {
            RoundEndUISingle.SetActive(true);
            foreach (Player player in Player.list.Values)
            {
                player.GetComponent<PlayerCharacterSkin>().TNT.SetActive(false);
            }
        }
    }
    [MessageHandler((ushort)ServerToClientId.tagBattle_PlayerTag)]
    private static void PlayerTag(Message message)
    {
        ushort tagPlayer = message.GetUShort();
        foreach (Player player in Player.list.Values)
        {
            if(player.Id == tagPlayer)
            {
                player.GetComponent<PlayerCharacterSkin>().TNT.SetActive(true);
            }
            else
            {
                player.GetComponent<PlayerCharacterSkin>().TNT.SetActive(false);
            }
        }
    }
    [MessageHandler((ushort)ServerToClientId.tagBattle_PlayerKill)]
    private static void PlayerKill(Message message)
    {
        ushort killPlayerID = message.GetUShort();
        foreach(Player player in Player.list.Values)
        {
            if(killPlayerID == player.Id)
            {
                player.StartDestroyEffect();
            }
        }
        Points1TMP.text = message.GetInt().ToString();
        Points2TMP.text = message.GetInt().ToString();
        Points3TMP.text = message.GetInt().ToString();
        Points4TMP.text = message.GetInt().ToString();
    }
}