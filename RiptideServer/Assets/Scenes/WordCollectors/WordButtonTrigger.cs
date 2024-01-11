using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordButtonTrigger : MonoBehaviour
{
    public string Letter;
    public int pointsPerChar = 50;

    private void Start()
    {
        SendWord(WordCollectorsLogic.Singleton.MainWord);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Substring(0, 6) == "Player")
        {
            if(other.gameObject.name.Substring(7, 1) == "1" && Letter == WordCollectorsLogic.MainWordChar[WordCollectorsLogic.MainWordCountPlayer1])
            {
                WordCollectorsLogic.MainWordCountPlayer1++;
                WordCollectorsLogic.Singleton.points[0] += pointsPerChar;
            }
            if (other.gameObject.name.Substring(7, 1) == "2" && Letter == WordCollectorsLogic.MainWordChar[WordCollectorsLogic.MainWordCountPlayer2])
            {
                WordCollectorsLogic.MainWordCountPlayer2++;
                WordCollectorsLogic.Singleton.points[1] += pointsPerChar;
            }
            if (other.gameObject.name.Substring(7, 1) == "3" && Letter == WordCollectorsLogic.MainWordChar[WordCollectorsLogic.MainWordCountPlayer3])
            {
                WordCollectorsLogic.MainWordCountPlayer3++;
                WordCollectorsLogic.Singleton.points[2] += pointsPerChar;
            }
            if (other.gameObject.name.Substring(7, 1) == "4" && Letter == WordCollectorsLogic.MainWordChar[WordCollectorsLogic.MainWordCountPlayer4])
            {
                WordCollectorsLogic.MainWordCountPlayer4++;
                WordCollectorsLogic.Singleton.points[3] += pointsPerChar;
            }
            SendLetter(WordCollectorsLogic.Singleton.MainWord.Substring(0, WordCollectorsLogic.MainWordCountPlayer1 + 1),
                WordCollectorsLogic.Singleton.MainWord.Substring(0, WordCollectorsLogic.MainWordCountPlayer2 + 1),
                WordCollectorsLogic.Singleton.MainWord.Substring(0, WordCollectorsLogic.MainWordCountPlayer3 + 1),
                WordCollectorsLogic.Singleton.MainWord.Substring(0, WordCollectorsLogic.MainWordCountPlayer4 + 1),
                int.Parse(other.gameObject.name.Substring(7, 1)));
            if (WordCollectorsLogic.MainWordChar[WordCollectorsLogic.MainWordCountPlayer1] == ".")
                WordCollectorsLogic.MainWordCountPlayer1++;
            if (WordCollectorsLogic.MainWordChar[WordCollectorsLogic.MainWordCountPlayer2] == ".")
                WordCollectorsLogic.MainWordCountPlayer2++;
            if (WordCollectorsLogic.MainWordChar[WordCollectorsLogic.MainWordCountPlayer3] == ".")
                WordCollectorsLogic.MainWordCountPlayer3++;
            if (WordCollectorsLogic.MainWordChar[WordCollectorsLogic.MainWordCountPlayer4] == ".")
                WordCollectorsLogic.MainWordCountPlayer4++;
            SendWord(WordCollectorsLogic.Singleton.MainWord);
        }
    }


    private void SendLetter(string letter1, string letter2, string letter3, string letter4, int id)
    {
        NetworkManager.Singleton.Server.SendToAll(SendLetterMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.wordCollectors_Letter), letter1, letter2, letter3, letter4, id));
    }
    private Message SendLetterMessage(Message message, string letter1, string letter2, string letter3, string letter4, int id)
    {
        message.AddString(letter1);
        message.AddString(letter2);
        message.AddString(letter3);
        message.AddString(letter4);
        message.AddInt(id);
        return message;
    }

    private static void SendWord(string Word)
    {
        NetworkManager.Singleton.Server.SendToAll(SendWordMessage(Message.Create(MessageSendMode.reliable, ServerToClientId.wordCollectors_Word), Word));
    }
    private static Message SendWordMessage(Message message, string Word)
    {
        message.AddString(Word);
        return message;
    }
}
