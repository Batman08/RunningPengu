using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public PieceType Type;

    private Piece _currentPiece;

    public void Spawn()
    {
        //_currentPiece = //Get me a new piece from the pool

        _currentPiece.gameObject.SetActive(value: true);
        _currentPiece.transform.SetParent(transform, false);
    }

    public void Despawn()
    {
        _currentPiece.gameObject.SetActive(value: false);
    }
}
