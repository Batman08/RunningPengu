using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public int MaxCoin = 5;
    public float ChanceToSpawn = 0.5f;
    public bool ForceSpawnAll = false;

    private GameObject[] _coins;

    private void Awake()
    {
        _coins = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            _coins[i] = transform.GetChild(i).gameObject;
        }
    }

    private void OnEnable()
    {
        Spawn();
    }

    private void OnDisable()
    {
        DisableObjects();
    }
    //pt13 9:19 / 26:17
    private void Spawn()
    {
        bool shouldNotSpawn = (Random.Range(0f, 1f) < ChanceToSpawn);
        if (shouldNotSpawn)
            return;

        if (ForceSpawnAll)
        {
            foreach (GameObject go in _coins)
            {
                go.SetActive(value: true);
            }
        }

        else
        {
            int r = Random.Range(0, MaxCoin);

            for (int i = 0; i < r; i++)
            {
                _coins[i].SetActive(value: true);
            }
        }
    }

    private void DisableObjects()
    {
        foreach (GameObject go in _coins)
        {
            go.SetActive(value: false);
        }
    }
}
