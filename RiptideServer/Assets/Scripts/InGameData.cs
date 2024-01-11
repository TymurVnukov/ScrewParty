using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameData : MonoBehaviour
{
    private static InGameData _singleton;
    public static InGameData Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(InGameData)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }
    public class Player
    {
        public string username;
    }

    [SerializeField] public int MaxPlayer = 4;
    [SerializeField] public string CurrentScene = "GameLobby";
    [SerializeField] public int[] points = new int[] { 0, 0, 0, 0 };
    [SerializeField] public int MiniGamePlayed = 0;
    [SerializeField] public string[] Players_Username;
    [SerializeField] public int[] Players_HeadID;
    [SerializeField] public int[] Players_BodyID;
    [SerializeField] public int[] Players_HandsID;
    [SerializeField] public int[] Players_LegsID;

    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        Players_Username = new string[MaxPlayer];
    }
}
