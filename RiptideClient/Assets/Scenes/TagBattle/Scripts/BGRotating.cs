using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGRotating : MonoBehaviour
{
    private float angel;
    void Update()
    {
        angel += Time.deltaTime * 6;
        transform.rotation = Quaternion.Euler(-89.98f, 0, angel);
    }
}
