using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType
{
    None = -1,
    Ramp = 0,
    Longblock = 1,
    Jump = 2,
    Slide = 3,

}

public class Piece : MonoBehaviour
{
    public PieceType Type;
    public int visualIndex;
}
