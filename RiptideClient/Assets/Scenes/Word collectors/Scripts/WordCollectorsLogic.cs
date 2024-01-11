using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WordCollectorsLogic : MonoBehaviour
{
    public static int WordsLeght = 0;

    public GameObject[] scores;
    public GameObject RoundEndUI;
    public GameObject Timer;
    public GameObject Word;

    private static GameObject RoundEndUISingle;
    private static TextMeshProUGUI TimerTMP;
    private static TextMeshProUGUI Points1TMP;
    private static TextMeshProUGUI Points2TMP;
    private static TextMeshProUGUI Points3TMP;
    private static TextMeshProUGUI Points4TMP;
    private static TextMeshProUGUI WordTMP;

    private void Start()
    {
        RoundEndUISingle = RoundEndUI;
        TimerTMP = Timer.GetComponent<TextMeshProUGUI>();
        Points1TMP = scores[0].GetComponent<TextMeshProUGUI>();
        Points2TMP = scores[1].GetComponent<TextMeshProUGUI>();
        Points3TMP = scores[2].GetComponent<TextMeshProUGUI>();
        Points4TMP = scores[3].GetComponent<TextMeshProUGUI>();
        WordTMP = Word.GetComponent<TextMeshProUGUI>();
        WordsLeght = 0;
        WordTMP.text = "";
        foreach (Player player in Player.list.Values)
        {
            player.UpHeadText.GetComponent<TextMeshProUGUI>().text = "";
            player.UpHeadText.GetComponent<TextMeshProUGUI>().enabled = true;
        }
    }

    [MessageHandler((ushort)ServerToClientId.wordCollectors_TimerTick)]
    private static void TimerTick(Message message)
    {
        ushort timer = message.GetUShort();
        TimerTMP.text = timer.ToString();
        Points1TMP.text = message.GetInt().ToString();
        Points2TMP.text = message.GetInt().ToString();
        Points3TMP.text = message.GetInt().ToString();
        Points4TMP.text = message.GetInt().ToString();
        if (timer == 0)
        {
            RoundEndUISingle.SetActive(true);
            foreach (Player player in Player.list.Values)
            {
                player.UpHeadText.GetComponent<TextMeshProUGUI>().text = "";
                player.UpHeadText.GetComponent<TextMeshProUGUI>().enabled = false;
            }
        }
    }

    [MessageHandler((ushort)ServerToClientId.wordCollectors_Word)]
    private static void WordGet(Message message)
    {
        string word = message.GetString();
        string result = word.Substring(WordsLeght, word.Length - WordsLeght).Split(".")[0];
        if (WordsLeght != 0) {
            if (word[WordsLeght - 1] == '.')
            {
                WordTMP.text = result;
            }
        }
        else
        {
            WordTMP.text = word.Split(".")[0];
        }
    }
}
