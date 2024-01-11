using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultSceneLogic : MonoBehaviour
{
    private static string[] Places = { "1st", "2nd", "3th", "4th" };
    private void Start()
    {
        StartCoroutine(GameEnd());
    }

    private IEnumerator GameEnd()
    {
        yield return new WaitForSeconds(5);

        foreach (Player player in Player.list.Values)
        {
            player.StartDestroyEffect();
        }
        if (Player.list.TryGetValue(1, out Player player1))
        {
            player1.UpHeadText.GetComponent<TextMeshProUGUI>().enabled = true;
        }
        if (Player.list.TryGetValue(2, out Player player2))
        {
            player2.UpHeadText.GetComponent<TextMeshProUGUI>().enabled = true;
        }
        if (Player.list.TryGetValue(3, out Player player3))
        {
            player3.UpHeadText.GetComponent<TextMeshProUGUI>().enabled = true;
        }
        if (Player.list.TryGetValue(4, out Player player4))
        {
            player4.UpHeadText.GetComponent<TextMeshProUGUI>().enabled = true;
        }
    }
    [MessageHandler((ushort)ServerToClientId.resultScene_Scoring)]
    private static void Scoring(Message message)
    {
        int player1Score = message.GetInt();
        int player2Score = message.GetInt();
        int player3Score = message.GetInt();
        int player4Score = message.GetInt();
        int[] scores = { player1Score, player2Score, player3Score, player4Score };
        for (int j = 0; j < scores.Length; j++)
        {
            for (int i = 0; i < scores.Length - 1; i++)
            {
                if (scores[i] < scores[i + 1])
                {
                    int tmp = scores[i];
                    scores[i] = scores[i + 1];
                    scores[i + 1] = tmp;
                }
            }
        }
        for(int i = 0; i < scores.Length; i++)
        {
            if(scores[i] == player1Score)
            {
                if (Player.list.TryGetValue(1, out Player player1))
                {
                    player1.UpHeadText.GetComponent<TextMeshProUGUI>().text = Places[i];
                }
            }
            if (scores[i] == player2Score)
            {
                if (Player.list.TryGetValue(2, out Player player2))
                {
                    player2.UpHeadText.GetComponent<TextMeshProUGUI>().text = Places[i];
                }
            }
            if (scores[i] == player3Score)
            {
                if (Player.list.TryGetValue(3, out Player player3))
                {
                    player3.UpHeadText.GetComponent<TextMeshProUGUI>().text = Places[i];
                }
            }
            if (scores[i] == player4Score)
            {
                if (Player.list.TryGetValue(4, out Player player4))
                {
                    player4.UpHeadText.GetComponent<TextMeshProUGUI>().text = Places[i];
                }
            }
        }
    }
}
