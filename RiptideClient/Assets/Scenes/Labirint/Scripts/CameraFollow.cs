using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float smoothTime;
    private Vector3 _currentVelocity = Vector3.zero;

    private void Awake()
    {
        target = GameObject.FindObjectOfType<PlayerController>().gameObject;
    }
    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y + 3f, target.transform.position.z - 5f);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
    }
}
