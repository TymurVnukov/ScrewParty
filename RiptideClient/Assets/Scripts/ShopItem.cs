using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    private string dbName = "URI=file:playerData.db";

    public int id;

    public void Buy()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO PlayerLocker(ItemID) VALUES (" + id + ")";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
}
