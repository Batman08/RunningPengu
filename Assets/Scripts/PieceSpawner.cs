using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public PieceType Type;

    private Piece _currentPiece;

    public void Spawn()
    {
        int amtObj = 0;
        switch (Type)
        {
            case PieceType.Ramp:
                amtObj = LevelManager.Instance.Ramps.Count;
                break;
            case PieceType.Longblock:
                amtObj = LevelManager.Instance.LongBlocks.Count;
                break;
            case PieceType.Jump:
                amtObj = LevelManager.Instance.Jumps.Count;
                break;
            case PieceType.Slide:
                amtObj = LevelManager.Instance.Slides.Count;
                break;
        }


        _currentPiece = LevelManager.Instance.GetPiece(Type, Random.Range(0, amtObj));
        _currentPiece.gameObject.SetActive(value: true);
        _currentPiece.transform.SetParent(transform, false);
    }

    public void Despawn()
    {
        _currentPiece.gameObject.SetActive(value: false);
    }
}
