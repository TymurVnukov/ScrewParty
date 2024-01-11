using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabirintLogic : MonoBehaviour
{
    private static LabirintLogic _singleton;
    public static LabirintLogic Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(LabirintLogic)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }
    private void Awake()
    {
        Singleton = this;
    }


    public GameObject Block1;
    public GameObject Block2;
    public GameObject Block3;
    public GameObject Block4;
    public GameObject Block5;
    public GameObject Block6;
    public GameObject Block7;
    public GameObject Block8;
    public GameObject Block9;
    public GameObject Block10;

    public static float cutoutSize;


    [MessageHandler((ushort)ServerToClientId.labirint_blockPosition)]
    private static void PlayerMovement(Message message)
    {
        ushort count = message.GetUShort();
        for (int i = 0; i < count; i++)
        {
            Vector2 position = message.GetVector2();
            Debug.Log(position);
            if (position.x >= 0f && position.y < 20)
            {
                Instantiate(Singleton.Block1, new Vector3(position.x, 0, position.y), Quaternion.identity);
            }
            if(position.x < 0f && position.y < 20)
            {
                Instantiate(Singleton.Block2, new Vector3(position.x, 0, position.y), Quaternion.identity);
            }

            if (position.x >= 0f && position.y > 20 && position.y <= 40)
            {
                Instantiate(Singleton.Block3, new Vector3(position.x, 0, position.y), Quaternion.identity);
            }
            if (position.x < 0f && position.y > 20 && position.y <= 40)
            {
                Instantiate(Singleton.Block4, new Vector3(position.x, 0, position.y), Quaternion.identity);
            }

            if (position.x >= 0f && position.y > 40 && position.y <= 60)
            {
                Instantiate(Singleton.Block5, new Vector3(position.x, 0, position.y), Quaternion.identity);
            }
            if (position.x < 0f && position.y > 40 && position.y <= 60)
            {
                Instantiate(Singleton.Block6, new Vector3(position.x, 0, position.y), Quaternion.identity);
            }

            if (position.x >= 0f && position.y > 60 && position.y <= 80)
            {
                Instantiate(Singleton.Block7, new Vector3(position.x, 0, position.y), Quaternion.identity);
            }
            if (position.x < 0f && position.y > 60 && position.y <= 80)
            {
                Instantiate(Singleton.Block8, new Vector3(position.x, 0, position.y), Quaternion.identity);
            }

            if (position.x >= 0f && position.y > 80 && position.y <= 101)
            {
                Instantiate(Singleton.Block9, new Vector3(position.x, 0, position.y), Quaternion.identity);
            }
            if (position.x < 0f && position.y > 80 && position.y <= 101)
            {
                Instantiate(Singleton.Block10, new Vector3(position.x, 0, position.y), Quaternion.identity);
            }
        }
    }


    [MessageHandler((ushort)ServerToClientId.labirint_CutoutPosition)]
    private static void CutoutPosition(Message message)
    {
        cutoutSize = message.GetFloat();
    }
}
