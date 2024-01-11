using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LabirintUIManager : MonoBehaviour
{
    public GameObject Timer;
    public GameObject RoundEndUI;
    private static TextMeshProUGUI TimerTMP;
    private static GameObject RoundEndUISingle;
    private void Start()
    {
        RoundEndUISingle = RoundEndUI;
        TimerTMP = Timer.GetComponent<TextMeshProUGUI>();
    }
    [MessageHandler((ushort)ServerToClientId.labirint_TimerTick)]
    private static void TimerTick(Message message)
    {
        ushort timer = message.GetUShort();
        TimerTMP.text = timer.ToString();
        if (timer == 0)
        {
            RoundEndUISingle.SetActive(true);
        }
    }
}
