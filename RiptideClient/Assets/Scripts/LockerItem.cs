using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerItem : MonoBehaviour
{
    public int id;
    public LockerItemType type;
    public LockerItemRare rare;

    public void SetCharacter()
    {
        PlayerLocker.SetCharacter(id, type);
    }
}
