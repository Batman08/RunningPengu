using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public int SegId { get; set; }
    public bool Transition;

    public int Length;
    public int BeginY1, BeginY2, BeginY3;
    public int EndY1, EndY2, EndY3;

    private PieceSpawner[] _pieces;

    private void Awake()
    {
        _pieces = gameObject.GetComponentsInChildren<PieceSpawner>();

        for (int i = 0; i < _pieces.Length; i++)    //**
        {
            foreach (MeshRenderer mr in _pieces[i].GetComponentsInChildren<MeshRenderer>())
            {
                mr.enabled = LevelManager.Instance.SHOW_COLLIDER;
            }
        }  //**
    }

    public void Spawn()
    {
        gameObject.SetActive(value: true);

        for (int i = 0; i < _pieces.Length; i++)
        {
            _pieces[i].Spawn();
        }
    }

    public void Despawn()
    {
        gameObject.SetActive(value: false);

        for (int i = 0; i < _pieces.Length; i++)
        {
            _pieces[i].Despawn();
        }
    }
}
