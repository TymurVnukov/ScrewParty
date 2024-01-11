using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using Unity.VisualScripting;
using TMPro;
using System.Linq;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();



    public ushort Id { get; private set; }
    public bool IsLocal { get; private set; }

    [SerializeField] private Transform camTransform;
    [SerializeField] public GameObject CharacterModel;
    [SerializeField] public GameObject destroyEffect;

    public GameObject UpHeadText;


    private Animator anim;

    public PlayerCharacterSkin CharacterSkin => characterSkin;

    [SerializeField] private PlayerCharacterSkin characterSkin;

    private string username;

    private void OnDestroy()
    {
        list.Remove(Id);
    }
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        anim = CharacterModel.GetComponent<Animator>();
    }
    private void Move(Vector3 newPosition, Vector3 forward, string animationName, int playerRotation)
    {
        transform.position = newPosition;

        if(!IsLocal)
        {
            camTransform.forward = forward;
        }
        CharacterModel.transform.rotation = Quaternion.Euler(0, playerRotation, 0);
        if (animationName == "idle")
        {
            anim.SetBool("isRunning", false);
        }
        else if (animationName == "run")
        {
            anim.SetBool("isRunning", true);
        }
    }

    public static void Spawn(ushort id, string username, Vector3 position)
    {
        Player player;
        if(id == NetworkManager.Singleton.Client.Id)
        {
            player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = true;
        }
        else
        {
            player = Instantiate(GameLogic.Singleton.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = false;
        }
        player.name = $"Player {id} {(string.IsNullOrEmpty(username) ? "Guest" : username)}";
        player.Id = id;
        player.username = username;

        GameLobbyUIManager.Singleton.AddPlayer(id, username);

        list.Add(id, player);
    }
    public void StartDestroyEffect()
    {
        GameObject explosion = Instantiate(destroyEffect, transform.position, transform.rotation);
        Destroy(explosion, 0.75f);
    }

    [MessageHandler((ushort)ServerToClientId.playerSpawned)]
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetVector3());
    }

    [MessageHandler((ushort)ServerToClientId.playerMovement)]
    private static void PlayerMovement(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Player player))
            player.Move(message.GetVector3(), message.GetVector3(), message.GetString(), message.GetInt());
    }

    [MessageHandler((ushort)ServerToClientId.playersSkins)]
    private static void PlayersSkins(Message message)
    {
        int leight = message.GetInt();
        for(int i = 1; i < leight + 1; i++)
        {
            if (list.TryGetValue((ushort)i, out Player playerList))
            {
                int HeadID = message.GetInt();
                int BodyID = message.GetInt();
                int HandsID = message.GetInt();
                int LegsID = message.GetInt();
                Debug.Log("Add Head (Local Player, Player, SkinsPreview)");
                //for (int j = 0; j < playerList.characterSkin.Head.Length; j++)
                //{
                //    playerList.characterSkin.Head[j].SetActive(false);
                //}
                //playerList.characterSkin.Head[HeadID].SetActive(true);

                for (int j = 0; j < playerList.characterSkin.Body.Length; j++)
                {
                    playerList.characterSkin.Body[j].SetActive(false);
                }
                playerList.characterSkin.Body[BodyID].SetActive(true);

                for (int j = 0; j < playerList.characterSkin.LeftHand.Length; j++)
                {
                    playerList.characterSkin.LeftHand[j].SetActive(false);
                }
                playerList.characterSkin.LeftHand[HandsID].SetActive(true);

                for (int j = 0; j < playerList.characterSkin.RightHand.Length; j++)
                {
                    playerList.characterSkin.RightHand[j].SetActive(false);
                }
                playerList.characterSkin.RightHand[HandsID].SetActive(true);

                for (int j = 0; j < playerList.characterSkin.LeftLeg.Length; j++)
                {
                    playerList.characterSkin.LeftLeg[j].SetActive(false);
                }
                playerList.characterSkin.LeftLeg[LegsID].SetActive(true);

                for (int j = 0; j < playerList.characterSkin.RightLeg.Length; j++)
                {
                    playerList.characterSkin.RightLeg[j].SetActive(false);
                }
                playerList.characterSkin.RightLeg[LegsID].SetActive(true);
            }
        }
    }

    [MessageHandler((ushort)ServerToClientId.wordCollectors_Letter)]
    private static void LetterGet(Message message)
    {
        int i = 0;
        string[] letters = { message.GetString(), message.GetString(), message.GetString(), message.GetString() };

        WordCollectorsLogic.WordsLeght = letters[NetworkManager.Singleton.Client.Id - 1].Length;

        foreach (Player player in list.Values)
        {
            string tmpLetters = letters[i].Split(".")[letters[i].Split(".").Count() - 1];
            if (tmpLetters.Length > 0)
            {
                player.UpHeadText.GetComponent<TextMeshProUGUI>().text = tmpLetters.Substring(0, tmpLetters.Length - 1);
            }
            if(letters[i].Substring(letters[i].Length - 1, 1) == ".")
            {
                player.UpHeadText.GetComponent<TextMeshProUGUI>().text = "";
            }
            i++;
        }
    }
}
