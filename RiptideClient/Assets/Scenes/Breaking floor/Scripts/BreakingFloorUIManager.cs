using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BreakingFloorUIManager : MonoBehaviour
{
    public GameObject[] scores;
    public GameObject RoundEndUI;
    public GameObject Timer;

    private static GameObject RoundEndUISingle;
    private static TextMeshProUGUI TimerTMP;
    private static TextMeshProUGUI Points1TMP;
    private static TextMeshProUGUI Points2TMP;
    private static TextMeshProUGUI Points3TMP;
    private static TextMeshProUGUI Points4TMP;

    private void Start()
    {
        RoundEndUISingle = RoundEndUI;
        TimerTMP = Timer.GetComponent<TextMeshProUGUI>();
        Points1TMP = scores[0].GetComponent<TextMeshProUGUI>();
        Points2TMP = scores[1].GetComponent<TextMeshProUGUI>();
        Points3TMP = scores[2].GetComponent<TextMeshProUGUI>();
        Points4TMP = scores[3].GetComponent<TextMeshProUGUI>();
    }

    [MessageHandler((ushort)ServerToClientId.breakingFloor_TimerTick)]
    private static void TimerTick(Message message)
    {
        ushort timer = message.GetUShort();
        TimerTMP.text = timer.ToString();
        Points1TMP.text = message.GetInt().ToString();
        Points2TMP.text = message.GetInt().ToString();
        Points3TMP.text = message.GetInt().ToString();
        Points4TMP.text = message.GetInt().ToString();
        if(timer == 0)
        {
            RoundEndUISingle.SetActive(true);
        }
    }
}
