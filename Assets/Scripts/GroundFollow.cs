using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFollow : MonoBehaviour
{
    private Transform _playerTransform;

    private void Start()
    {
        FindPlayer();
    }

    private void Update()
    {
        FollowPlayer();
    }

    private void FindPlayer()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FollowPlayer()
    {
        transform.position = Vector3.forward * _playerTransform.position.z;
    }
}
