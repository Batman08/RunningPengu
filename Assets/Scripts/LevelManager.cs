using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { set; get; }

    private const bool SHOW_COLLIDER = true;

    //Level Spawning
    private const float DISTANCE_BEFORE_SPAWN = 100f;
    private const int INITIAL_SEGMENTS = 10;
    private const int MAX_SEGMENTS_ON_SCREEN = 15;
    private Transform _cameraContainer;
    private int _amountOfActiveSegments;
    private int _continiousSegments;
    private int _currentSpawnZ;
    private int __currentLevel;
    private int _y1, _y2, _y3;


    //List of pieces
    public List<Piece> Ramps = new List<Piece>();
    public List<Piece> LongBlocks = new List<Piece>();
    public List<Piece> Jumps = new List<Piece>();
    public List<Piece> Slides = new List<Piece>();
    public List<Piece> Pieces = new List<Piece>(); //All pieces in the pool


    //List of segments
    public List<Segment> AvailableSegments = new List<Segment>();
    public List<Segment> AvailableTransitions = new List<Segment>();
    public List<Segment> Segments = new List<Segment>();


    //Gameplay
    private bool _isMoving = false;


    private void Awake()
    {
        Instance = this;
        _cameraContainer = Camera.main.transform;
        _currentSpawnZ = 0;
        __currentLevel = 0;
    }

    private void Start()
    {

    }

    private void SpawnAllSegments()
    {
        for (int i = 0; i < INITIAL_SEGMENTS; i++)
        {
            //Generate Segment
            GenerateSegment();
        }
    }

    private void GenerateSegment()
    {
        SpawnSegment();

        if (Random.Range(0f, 1f) < (_continiousSegments * 0.25f))
        {
            //Spawn transition seg
            _continiousSegments = 0;
            SpawnTransition();
        }

        else
        {
            _continiousSegments++;
        }
    }

    private void SpawnSegment()
    {

    }

    private void SpawnTransition()
    {

    }

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
        }//10.19min -pt10 
        return p;
    }
}
