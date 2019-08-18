using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { set; get; }

    public bool SHOW_COLLIDER = true; //**

    //Level Spawning
    private const float DISTANCE_BEFORE_SPAWN = 100f;
    private const int INITIAL_SEGMENTS = 10;
    private const int INITIAL_TRANSITION_SEGMENTS = 2;
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

    [HideInInspector]
    public List<Piece> Pieces = new List<Piece>(); //All pieces in the pool


    //List of segments
    public List<Segment> AvailableSegments = new List<Segment>();
    public List<Segment> AvailableTransitions = new List<Segment>();

    [HideInInspector]
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
        SpawnAllSegments();
    }

    private void Update()
    {
        //Spawn Segment when none left
        bool noneInfrontOfPlayer = (_currentSpawnZ - _cameraContainer.position.z < DISTANCE_BEFORE_SPAWN);
        if (noneInfrontOfPlayer)
        {
            GenerateSegment();
        }

        //Despawn Segment
        bool segmentsBehindCamera = (_amountOfActiveSegments >= MAX_SEGMENTS_ON_SCREEN);
        if (segmentsBehindCamera)
        {
            Segments[_amountOfActiveSegments - 1].Despawn();
            _amountOfActiveSegments--;
        }
    }

    private void SpawnAllSegments()
    {
        for (int i = 0; i < INITIAL_SEGMENTS; i++)
        {
            bool startOfGame = ((i < INITIAL_TRANSITION_SEGMENTS));
            if (startOfGame)
                SpawnTransition();
            else
                GenerateSegment();
        }
    }

    private void GenerateSegment()
    {
        SpawnSegment();

        if (Random.Range(0f, 1f) < (_continiousSegments * 0.25f))
        {
            //Spawn transition segment
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
        List<Segment> possibleSeg = AvailableSegments.FindAll(x => x.BeginY1 == _y1 || x.BeginY2 == _y2 || x.BeginY3 == _y3);
        int id = Random.Range(0, possibleSeg.Count);

        Segment s = GetSegment(id, false);

        _y1 = s.EndY1;
        _y2 = s.EndY2;
        _y3 = s.EndY3;

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * _currentSpawnZ;

        _currentSpawnZ += s.Length;
        _amountOfActiveSegments++;
        s.Spawn();
    }

    private void SpawnTransition()
    {
        List<Segment> possibleTransition = AvailableTransitions.FindAll(x => x.BeginY1 == _y1 || x.BeginY2 == _y2 || x.BeginY3 == _y3);
        int id = Random.Range(0, possibleTransition.Count);

        Segment s = GetSegment(id, true);

        _y1 = s.EndY1;
        _y2 = s.EndY2;
        _y3 = s.EndY3;

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * _currentSpawnZ;

        _currentSpawnZ += s.Length;
        _amountOfActiveSegments++;
        s.Spawn();
    }

    public Segment GetSegment(int id, bool transition)
    {
        Segment s = null;
        s = Segments.Find(x => x.SegId == id && x.Transition == transition && !x.gameObject.activeSelf);

        bool noSegmentsAvailable = (s == null);
        if (noSegmentsAvailable)
        {
            GameObject go = Instantiate((transition) ? AvailableTransitions[id].gameObject :
                            AvailableSegments[id].gameObject) as GameObject;
            s = go.GetComponent<Segment>();

            s.SegId = id;
            s.Transition = transition;

            Segments.Insert(0, s);
        }

        else
        {
            Segments.Remove(s);
            Segments.Insert(0, s);
        }

        return s;
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
        }
        return p;
    }
}
