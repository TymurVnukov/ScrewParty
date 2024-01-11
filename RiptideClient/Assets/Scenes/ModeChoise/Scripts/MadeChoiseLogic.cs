using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MadeChoiseLogic : MonoBehaviour
{
    public GameObject GameEnd;
    public GameObject GameCount;
    public GameObject[] scores;
    public GameObject[] allMode;
    public GameObject[] allModeButtons;
    public Color defaultColor;
    public Color choiseColor;
    public Color resultModeColor;
    public GameObject Timer;


    private static TextMeshProUGUI Points1TMP;
    private static TextMeshProUGUI Points2TMP;
    private static TextMeshProUGUI Points3TMP;
    private static TextMeshProUGUI Points4TMP;
    private static TextMeshProUGUI TimerTMP;

    private static MadeChoiseLogic _singleton;
    public static MadeChoiseLogic Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(MadeChoiseLogic)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }
    private void Awake()
    {
        Singleton = this;
        TimerTMP = Timer.GetComponent<TextMeshProUGUI>();
        Points1TMP = scores[0].GetComponent<TextMeshProUGUI>();
        Points2TMP = scores[1].GetComponent<TextMeshProUGUI>();
        Points3TMP = scores[2].GetComponent<TextMeshProUGUI>();
        Points4TMP = scores[3].GetComponent<TextMeshProUGUI>();
    }

    [MessageHandler((ushort)ServerToClientId.choiseMode)]
    private static void ChoiseMode(Message message)
    {
        string PlayerChoise1 = message.GetString();
        string PlayerChoise2 = message.GetString();
        string PlayerChoise3 = message.GetString();
        string PlayerChoise4 = message.GetString();
        string resultMode = message.GetString();
        for(int i = 0; i < Singleton.allMode.Length; i++)
        {
            string choiseString = Singleton.allMode[i].GetComponent<ModeItem>().choiseString;
            if (choiseString == PlayerChoise1 || choiseString == PlayerChoise2 || choiseString == PlayerChoise3 || choiseString == PlayerChoise4)
            {
                Singleton.allMode[i].GetComponent<Image>().color = Singleton.choiseColor;
            }
            else
            {
                Singleton.allMode[i].GetComponent<Image>().color = Singleton.defaultColor;
            }
        }
        if(resultMode != "none")
        {
            for (int i = 0; i < Singleton.allMode.Length; i++)
            {
                string choiseString = Singleton.allMode[i].GetComponent<ModeItem>().choiseString;
                Singleton.allMode[i].GetComponent<Image>().color = Singleton.defaultColor;
                if(choiseString == resultMode)
                {
                    Singleton.allMode[i].GetComponent<Image>().color = Singleton.resultModeColor;
                }
                Singleton.allModeButtons[i].GetComponent<Button>().enabled = false;
            }
        }
    }

    [MessageHandler((ushort)ServerToClientId.ModeChoise_TimerTick)]
    private static void GetTimer(Message message)
    {
        int time = message.GetUShort();
        TimerTMP.text = time.ToString();
    }

    [MessageHandler((ushort)ServerToClientId.ModeChoise_PlayersScore)]
    private static void GetPlayersScore(Message message)
    {
        Points1TMP.text = message.GetInt().ToString();
        Points2TMP.text = message.GetInt().ToString();
        Points3TMP.text = message.GetInt().ToString();
        Points4TMP.text = message.GetInt().ToString();
    }

    [MessageHandler((ushort)ServerToClientId.inGameData_endGame)]
    private static void EndGame(Message message)
    {
        int gamePlayed = message.GetInt() - 1;
        Singleton.GameCount.GetComponent<TextMeshProUGUI>().text = gamePlayed.ToString();
        if (gamePlayed == 4)
        {
            Singleton.GameEnd.SetActive(true);
        }
    }
}
