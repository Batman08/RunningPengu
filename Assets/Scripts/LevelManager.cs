using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { set; get; }

    //Level Spawning

    //List of pieces
    public List<Piece> Ramps = new List<Piece>();
    public List<Piece> LongBlocks = new List<Piece>();
    public List<Piece> Jumps = new List<Piece>();
    public List<Piece> Slides = new List<Piece>();
    public List<Piece> Pieces = new List<Piece>(); //All pieces in the pool

    public Piece GetPiece(PieceType pt, int VisualIndex)
    {
        Piece p = Pieces.Find(x => x.Type == pt && x.visualIndex == VisualIndex && !x.gameObject.activeSelf);

        bool hasNotFoundPiece = (p == null);
        if (hasNotFoundPiece)
        {
            GameObject go = null;
            bool hasChosenRamp = (pt == PieceType.Ramp);
            bool hasChosenLongblock = (pt == PieceType.Longblock);
            bool hasChosenJump = (pt == PieceType.Jump);
            bool hasChosenSlide = (pt == PieceType.Slide);
            if (hasChosenRamp)
                go = Ramps[VisualIndex].gameObject;
            else if (hasChosenLongblock)
                go = LongBlocks[VisualIndex].gameObject;
            else if (hasChosenJump)
                go = Jumps[VisualIndex].gameObject;
            else if (hasChosenSlide)
                go = Slides[VisualIndex].gameObject;

            go = Instantiate(go);
            p = go.GetComponent<Piece>();
            Pieces.Add(item: p);
        }//4min -pt9
        return p;
    }
}
