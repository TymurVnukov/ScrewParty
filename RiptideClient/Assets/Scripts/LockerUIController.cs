using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LockerItemRare
{
    Blue = 1,
    Purple = 2,
    Orange = 3,
}
public enum LockerItemType
{
    Head = 1,
    Body = 2,
    Hands = 3,
    Legs = 4,
}

public class LockerUIController : MonoBehaviour
{
    public GameObject MainUI;
    public GameObject Content;
    public GameObject[] LockerItems;
    private GameObject[] AllowedLockerItems = new GameObject[100];
    private int AllowedLockerItemsIndex = 0;
    private string dbName = "URI=file:playerData.db";

    private void Start()
    {
        UpdateLocker(1);
    }

    public void CloseUI()
    {
        MainUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void UpdateLocker(int lockerType)
    {
        for(int i = 0; i < AllowedLockerItemsIndex; i++)
        {
            Destroy(AllowedLockerItems[i]);
        }
        AllowedLockerItems = new GameObject[100];
        AllowedLockerItemsIndex = 0;
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT locker.id, locker.Name, locker.Type, locker.Rare, locker.LocalTypeId FROM PlayerLocker pl INNER JOIN Locker locker ON pl.ItemID = locker.id Where locker.Type = {lockerType} Order by locker.Rare DESC";
                SqliteDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    int id = Convert.ToInt32(r[0]);
                    string name = (string)r[1];
                    int type = Convert.ToInt32(r[2]);
                    int rare = Convert.ToInt32(r[3]);
                    int localTypeId = Convert.ToInt32(r[4]);
                    AllowedLockerItems[AllowedLockerItemsIndex] = Instantiate(LockerItems[id - 1], Content.transform);
                    AllowedLockerItemsIndex++;
                }
            }
            connection.Close();
        }
    }
}
