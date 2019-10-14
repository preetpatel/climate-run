using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestSpawnManager : MonoBehaviour
{
    public bool SHOW_COLLIDER = false; // $$$

    public static ForestSpawnManager Instance { set; get; }

    // Level Spawning
    private const float DISTANCE_BEFORE_SPAWN = 100.0f;
    private const int INITIAL_SEGMENTS = 10;
    private const int INITIAL_TRANSITION_SEGMENTS = 2;
    private const int MAX_SEGMENTS_ON = 15;
    private Transform cameraContainer;
    private int amountOfActiveSegments;
    private int continuousSegments;
    private int currentSpawnZ;
    private int currentLevel;
    private int y1, y2, y3;

    // List of pieces
    public List<Piece> ramps = new List<Piece>();
    public List<Piece> longblocks = new List<Piece>();
    public List<Piece> jumps = new List<Piece>();
    public List<Piece> slides = new List<Piece>();
    public List<Piece> treeRow = new List<Piece>();
    public List<Piece> longRamps = new List<Piece>();
    public List<Piece> fallenTrees = new List<Piece>();

    [HideInInspector]
    public List<Piece> pieces = new List<Piece>(); // active pieces

    // List of segments
    public List<Segment> availableSegments = new List<Segment>();
    public List<Segment> availableTransitions = new List<Segment>();

    [HideInInspector]   
    public List<Segment> segments; // active segments

    // Gameplay
    private bool isMoving = false;

    private void Awake()
    {
        Instance = this;
        cameraContainer = Camera.main.transform;
        currentSpawnZ = 0;
        currentLevel = 0;
    }

    private void Update()
    {
        if(currentSpawnZ - cameraContainer.position.z < DISTANCE_BEFORE_SPAWN)
            GenerateSegment();

        if(amountOfActiveSegments >= MAX_SEGMENTS_ON)
        {
            segments[amountOfActiveSegments - 1].DeSpawn();
            amountOfActiveSegments--;
        }
    }

    private void Start()
    {
        for (int i = 0; i < INITIAL_SEGMENTS; i++)
            if (i < INITIAL_TRANSITION_SEGMENTS)
            {
                SpawnTransition();
            }
            else
            {
                GenerateSegment();
            }
        
    }

    private void GenerateSegment()
    {
        SpawnSegment();

        if (Random.Range(0f, 1f) < (continuousSegments * 0.25f))
        {
            // Spawn transition seg
            continuousSegments = 0;
            SpawnTransition();
        }
        else
        {
            continuousSegments++;
        }
    }

    private void SpawnSegment()
    {
        List<Segment> possibleSeg = availableSegments.FindAll(
            x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        int id = Random.Range(0, possibleSeg.Count);
        if (segments[0].SegId == 1 && id == 1)
        {
            bool beforeRange = Random.Range(0, 2) == 1 ? true : false;
            if (beforeRange)
                id = 0;
            else
                id = Random.Range(2, possibleSeg.Count);
        }

        Segment s = GetSegment(id, false);

        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += s.length;
        amountOfActiveSegments++;
        s.Spawn();
    }

    private void SpawnTransition()
    {
        List<Segment> possibleTransition = availableTransitions.FindAll(
            x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        int id = Random.Range(0, possibleTransition.Count);

        Segment s = GetSegment(id, true);

        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += s.length;
        amountOfActiveSegments++;
        s.Spawn();
    }

    public Segment GetSegment(int id, bool transition)
    {
        Segment s = null;

        s = segments.Find(x => x.SegId == id &&
                x.transition == transition &&
                !x.gameObject.activeSelf);

        if(s == null)
        {
            GameObject go = Instantiate((transition) ? 
                availableTransitions[id].gameObject : 
                availableSegments[id].gameObject) as GameObject;
            s = go.GetComponent<Segment>();

            s.SegId = id;
            s.transition = transition;

            segments.Insert(0, s);
        }
        else
        {
            segments.Remove(s);
            segments.Insert(0, s);
        }

        return s;
    }

    public Piece GetPiece(PieceType pt, int visualIndex)
    {
        Piece p = pieces.Find(x => x.type == pt &&
                x.visualIndex == visualIndex &&
                !x.gameObject.activeSelf);

        if (p == null)
        {
            GameObject go = null;
            if (pt == PieceType.ramp)
                go = ramps[visualIndex].gameObject;
            else if (pt == PieceType.longblock)
                go = longblocks[visualIndex].gameObject;
            else if (pt == PieceType.jump)
                go = jumps[visualIndex].gameObject;
            else if (pt == PieceType.slide)
                go = slides[visualIndex].gameObject;
            else if (pt == PieceType.treeRow)
                go = treeRow[visualIndex].gameObject;
            else if (pt == PieceType.longRamp)
                go = longRamps[visualIndex].gameObject;
            else if (pt == PieceType.fallenTree)
                go = fallenTrees[visualIndex].gameObject;

            go = Instantiate(go);
            p = go.GetComponent<Piece>();
            pieces.Add(p);
        }

        return p;
    }
}
