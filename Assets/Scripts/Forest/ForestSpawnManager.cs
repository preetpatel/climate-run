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
    private int segsSinceFireTruck;

    // List of pieces
    public List<Piece> ramps = new List<Piece>();
    public List<Piece> longblocks = new List<Piece>();
    public List<Piece> jumps = new List<Piece>();
    public List<Piece> slides = new List<Piece>();
    public List<Piece> treeRows = new List<Piece>();
    public List<Piece> longRamps = new List<Piece>();
    public List<Piece> fallenTrees = new List<Piece>();

    [HideInInspector]
    public List<Piece> pieces = new List<Piece>(); // active pieces

    // List of segments
    public List<Segment> availableSegments = new List<Segment>();
    public List<Segment> availableTransitions = new List<Segment>();
    public List<Segment> availableFireTrucks = new List<Segment>();

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
        segsSinceFireTruck = 0;

        int i = 0;
        foreach (Segment seg in availableSegments)
            seg.SegId = i++;

        foreach (Segment tSeg in availableTransitions)
            tSeg.SegId = i++;

        foreach (Segment fSeg in availableFireTrucks)
            fSeg.SegId = i++;
    }

    private void Update()
    {
        if(currentSpawnZ - cameraContainer.position.z < DISTANCE_BEFORE_SPAWN)
            GenerateSegment();

        if(amountOfActiveSegments >= MAX_SEGMENTS_ON)
        {
            Segment seg = segments[amountOfActiveSegments - 1];
            seg.DeSpawn();
            
            if (seg.fireTruck)
                segments.Remove(seg);

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
        bool spawnedTransition = SpawnSegment();

        if (Random.Range(0f, 1f) < (continuousSegments * 0.25f) && !spawnedTransition)
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

    private bool SpawnSegment()
    {
        List<Segment> possibleSeg;
        bool getFireTruck = false;
        if (segsSinceFireTruck > 20)
        {
            segsSinceFireTruck = 0;
            getFireTruck = true;
            possibleSeg = availableFireTrucks.FindAll(
            x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        }
        else
        {
            segsSinceFireTruck++;
            possibleSeg = availableSegments.FindAll(
            x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        }

        //if(possibleSeg.Count == 0)
        //{
        //    continuousSegments = 0;
        //    SpawnTransition();
        //    return true;
        //}

        int i = Random.Range(0, possibleSeg.Count);
        int id = possibleSeg[i].SegId;

        if (segments[0].SegId == 1 && id == 1)
        {
            bool beforeRange = Random.Range(0, 5) <= 1 ? true : false;
            if (beforeRange)
                i = 0;
            else
                i = Random.Range(2, possibleSeg.Count);

            id = possibleSeg[i].SegId;
        }

        Segment s = GetSegment(id, false, getFireTruck);

        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += s.length;
        amountOfActiveSegments++;
        s.Spawn();
        return false;
    }

    private void SpawnTransition()
    {
        List<Segment> possibleTransition = availableTransitions.FindAll(
            x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        int i = Random.Range(0, possibleTransition.Count);

        int id = possibleTransition[i].SegId;

        Segment s = GetSegment(id, true, false);

        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += s.length;
        amountOfActiveSegments++;
        segsSinceFireTruck++;
        s.Spawn();
    }

    public Segment GetSegment(int id, bool transition, bool fireTruck)
    {
        Segment s = null;

        if (!fireTruck)
        {
            s = segments.Find(x => x.SegId == id &&
                x.transition == transition &&
                x.fireTruck == fireTruck &&
                !x.gameObject.activeSelf);
        }

        if(s == null)
        {
            GameObject template;
            if (transition)
                template = availableTransitions.Find(x => x.SegId == id).gameObject;
            else if (fireTruck)
                template = availableFireTrucks.Find(x => x.SegId == id).gameObject;
            else
                template = availableSegments.Find(x => x.SegId == id).gameObject;

            GameObject go = Instantiate(template) as GameObject;
            s = go.GetComponent<Segment>();

            s.SegId = id;
            s.transition = transition;
            s.fireTruck = fireTruck;

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
                go = treeRows[visualIndex].gameObject;
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
