using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxsManager : MonoBehaviour
{
    public GameObject[] Boxs;
    public static GameObject[,] InstantiateBoxs = new GameObject[7,13];

    private void Start()
    {
        int X = 0, Y = 0;
        for (float y = -9f; y <= 9f; y += 1.5f)
        {
            for (float x = -4.5f; x <= 4.5f; x += 1.5f)
            {
                int rand = Random.Range(0, Boxs.Length);
                GameObject box = Instantiate(Boxs[rand], new Vector3(y, 0, x), Boxs[rand].transform.rotation, this.gameObject.transform);
                InstantiateBoxs[X, Y] = box;
                X++;
            }
            Y++;
            X = 0;
        }
    }
    [MessageHandler((ushort)ServerToClientId.breakingFloor_breakBox)]
    private static void breakBox(Message message)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector2 pos = message.GetVector2();
            InstantiateBoxs[(int)pos.x, (int)pos.y].GetComponent<BoxScript>().StartAnim();
        }
    }
}
