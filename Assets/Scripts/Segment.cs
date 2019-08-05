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

    private Piece[] _pieces;

    private void Awake()
    {
        _pieces = gameObject.GetComponentsInChildren<Piece>();
    }

    public void Spawn()
    {
        gameObject.SetActive(value: true);
    }

    public void Despawn()
    {
        gameObject.SetActive(value: false);
    }
}
