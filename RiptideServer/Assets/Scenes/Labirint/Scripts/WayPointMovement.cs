using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointMovement : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 0, 100f), 0.005f);
    }
}
