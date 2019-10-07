using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class manages the spawning of all objects within the forest level
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

    [HideInInspector]
    public List<Piece> pieces = new List<Piece>(); // active pieces

    // List of segments
    public List<Segment> availableSegments = new List<Segment>();
    public List<Segment> availableTransitions = new List<Segment>();

    [HideInInspector]   
    public List<Segment> segments; // active segments

    // Gameplay
    private bool isMoving = false;

    // Initialises necessary variables
    private void Awake()
    {
        Instance = this;
        cameraContainer = Camera.main.transform;
        currentSpawnZ = 0;
        currentLevel = 0;
    }

    // Called once per frame
    private void Update()
    {
        // If the camera is close enough generate a segment
        if(currentSpawnZ - cameraContainer.position.z < DISTANCE_BEFORE_SPAWN)
            GenerateSegment();

        // If we have too many segments spawned, despawn one.
        if(amountOfActiveSegments >= MAX_SEGMENTS_ON)
        {
            segments[amountOfActiveSegments - 1].DeSpawn();
            amountOfActiveSegments--;
        }
    }

    // Called before first frame update
    private void Start()
    {
        // Spawns the initial segments before the user starts
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

    // Generates a segment, spawning it in the level
    private void GenerateSegment()
    {
        // Spawns the segment
        SpawnSegment();

        // If there have been too many segments in a row, spawn a transition
        // Gives the user a "break"
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

    // Spawns the segment into the world
    private void SpawnSegment()
    {
        // Finds a list of all possible segments to spawn
        List<Segment> possibleSeg = availableSegments.FindAll(
            x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        // Randomly selects the segment to spawn
        int id = Random.Range(0, possibleSeg.Count);
        if (segments[0].SegId == 1 && id == 1)
        {
            bool beforeRange = Random.Range(0, 2) == 1 ? true : false;
            if (beforeRange)
                id = 0;
            else
                id = Random.Range(2, possibleSeg.Count);
        }

        // Gets the segment to spawn
        Segment s = GetSegment(id, false);

        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;

        // Sets segment values
        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += s.length;
        amountOfActiveSegments++;
        // Spawns the segment
        s.Spawn();
    }

    // Spawns a transition as a "break" for the player
    private void SpawnTransition()
    {
        // Gets all possible transitions
        List<Segment> possibleTransition = availableTransitions.FindAll(
            x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        // Randomly selects a transition
        int id = Random.Range(0, possibleTransition.Count);

        Segment s = GetSegment(id, true);

        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;

        // Sets transition values and spawns the transition
        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += s.length;
        amountOfActiveSegments++;
        s.Spawn();
    }

    // Gets a segment from the pool of segments
    public Segment GetSegment(int id, bool transition)
    {
        Segment s = null;

        // Finds the segment/transition with the input id
        s = segments.Find(x => x.SegId == id &&
                x.transition == transition &&
                !x.gameObject.activeSelf);

        if(s == null)
        {
            // If null instantiate the transition or segment and add it
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

    // Gets a specific piece of a segment
    public Piece GetPiece(PieceType pt, int visualIndex)
    {
        // Gets the piece
        Piece p = pieces.Find(x => x.type == pt &&
                x.visualIndex == visualIndex &&
                !x.gameObject.activeSelf);

        // If it can't find the piece instantiate one of the correct type
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

            go = Instantiate(go);
            p = go.GetComponent<Piece>();
            pieces.Add(p);
        }

        return p;
    }
}
