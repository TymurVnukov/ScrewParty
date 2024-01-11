using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocker : MonoBehaviour
{
    private static PlayerLocker _singleton;
    public static PlayerLocker Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(PlayerLocker)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }
    private void Awake()
    {
        Singleton = this;
    }

    public static int HeadID = 0;
    public static int BodyID = 0;
    public static int HandsID = 0;
    public static int LegsID = 0;

    public GameObject[] Head;
    public GameObject[] Body;
    public GameObject[] LeftHand;
    public GameObject[] RightHand;
    public GameObject[] LeftLeg;
    public GameObject[] RightLeg;

    public static void SetCharacter(int id, LockerItemType type)
    {
        if (type == LockerItemType.Head)
        {
            for (int i = 0; i < Singleton.Head.Length; i++)
            {
                Singleton.Head[i].SetActive(false);
            }
            Singleton.Head[id - 1].SetActive(true);
            HeadID = id - 1;
        }
        if (type == LockerItemType.Body)
        {
            for(int i = 0; i < Singleton.Body.Length; i++)
            {
                Singleton.Body[i].SetActive(false);
            }
            Singleton.Body[id - 1].SetActive(true);
            BodyID = id - 1;
        }
        if (type == LockerItemType.Hands)
        {
            for (int i = 0; i < Singleton.LeftHand.Length; i++)
            {
                Singleton.LeftHand[i].SetActive(false);
                Singleton.RightHand[i].SetActive(false);
            }
            Singleton.LeftHand[id - 1].SetActive(true);
            Singleton.RightHand[id - 1].SetActive(true);
            HandsID = id - 1;
        }
        if (type == LockerItemType.Legs)
        {
            for (int i = 0; i < Singleton.LeftLeg.Length; i++)
            {
                Singleton.LeftLeg[i].SetActive(false);
                Singleton.RightLeg[i].SetActive(false);
            }
            Debug.Log(id);
            Singleton.LeftLeg[id - 1].SetActive(true);
            Singleton.RightLeg[id - 1].SetActive(true);
            LegsID = id - 1;
        }
    }
}
