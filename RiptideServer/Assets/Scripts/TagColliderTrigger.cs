using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagColliderTrigger : MonoBehaviour
{
    public GameObject player;
    public bool isEPressed = false;
    private void OnTriggerStay(Collider other)
    {
        if (InGameData.Singleton.CurrentScene == "TagBattle")
        {
            if (other.gameObject.name.Substring(0, 6) == "Player" && isEPressed && player.GetComponent<TagManager>().isTaged == true)
            {
                TagBattleLogic.Singleton.SendPlayerTag(other.gameObject.GetComponent<PlayerMovement>().player.Id);
                other.gameObject.GetComponent<TagManager>().isTaged = true;
                player.GetComponent<TagManager>().isTaged = false;
            }
        }
        if (other.gameObject.name.Substring(0, 4) == "Milk" && isEPressed)
        {
            Destroy(other.gameObject);
        }
    }
}
