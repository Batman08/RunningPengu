using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlacierSpawner : MonoBehaviour
{
    private const float DISTANCE_TO_SPAWN = 10f;

    public float ScrollSpeed = 2f;
    public float TotalLength;
    public bool IsScrolling { set; get; }

    private float _scrollLocation;
    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!IsScrolling)
            return;

        _scrollLocation += ScrollSpeed * Time.deltaTime;

        Vector3 newLocation = (_playerTransform.position.z + _scrollLocation) * Vector3.forward;
        transform.position = newLocation;

        bool noGlaciersInfrontOfPlayer = (transform.GetChild(0).transform.position.z < _playerTransform.position.z - DISTANCE_TO_SPAWN);
        if (noGlaciersInfrontOfPlayer)
        {
            //16 ----- 9:53 / 15:01
        }
    }
}
