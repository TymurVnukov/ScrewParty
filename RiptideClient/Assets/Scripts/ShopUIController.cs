using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIController : MonoBehaviour
{
    public GameObject MainUI;
    public void CloseUI()
    {
        MainUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
