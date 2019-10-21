using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachSpawnManager : MonoBehaviour
{
    public bool SHOW_COLLIDER = false; //$$

    public static BeachSpawnManager Instance {set;get;}

    //level spawning
    private const float DISTANCE_BEFORE_SPAWN = 100.0f;
    private const int INITIAL_SEGMENTS = 10;
    private const int MAX_SEGMENTS_ON_SCREEN = 15;
    private Transform cameraContainer;
    private int amountOfActiveSegments;
    private int continiousSegments;
    private int currentSpawnZ;
    private int currentLevel;
    private int y1, y2, y3;

    //list of pieces
    public List<BeachPiece> ramps = new List<BeachPiece>();
    public List<BeachPiece> longblocks = new List<BeachPiece>();
    public List<BeachPiece> jumps = new List<BeachPiece>();
    public List<BeachPiece> slides = new List<BeachPiece>();
    public List<BeachPiece> shipwrecks = new List<BeachPiece>();
    public List<BeachPiece> volleyballNets = new List<BeachPiece>();
    [HideInInspector]
    public List<BeachPiece> pieces = new List<BeachPiece>();//all the pieces in the pool

    //list of segments
    public List<BeachSegment> availableSegments = new List<BeachSegment>();
    public List<BeachSegment> availableTransitions = new List<BeachSegment>();
    [HideInInspector]
    public List<BeachSegment> segments = new List<BeachSegment>();

    //GamePlay
    private bool isMoving = false;

    private void Awake()
    {
        Instance = this;
        cameraContainer = Camera.main.transform;
        currentSpawnZ = 15;
        currentLevel = 0;
 
        //FindObjectOfType<CameraMotor>().isFollowing = true;
    }

    public BeachSegment GetSegment(int id, bool transition)
    {
        BeachSegment s = null;
        s = segments.Find(x => x.SegId == id && x.transition == transition && !x.gameObject.activeSelf);

        if( s == null)
        {
            GameObject go = Instantiate((transition)?availableTransitions[id].gameObject : availableSegments[id].gameObject) as GameObject;
            s = go.GetComponent<BeachSegment>();

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
    

    public BeachPiece GetPiece(BeachPieceType pt, int visualIndex)
    {
        BeachPiece p = pieces.Find( x => x.type == pt && x.visualIndex == visualIndex && !x.gameObject.activeSelf);
        if(p == null)
        {
            GameObject go = null;
            if(pt == BeachPieceType.ramp)
            {
                go = ramps[visualIndex].gameObject;
            }
            else if (pt == BeachPieceType.longblock)
            {
                go = longblocks[visualIndex].gameObject;
            }else if (pt == BeachPieceType.jump)
            {
                go = jumps[visualIndex].gameObject;
            }else if (pt == BeachPieceType.slide)
            {
                go = slides[visualIndex].gameObject;
            }else if (pt == BeachPieceType.shipwreck)
            {
                go = shipwrecks[visualIndex].gameObject;
            }else if (pt == BeachPieceType.volleyballNet)
            {
                go = volleyballNets[visualIndex].gameObject;
            }
            go = Instantiate(go);
            p = go.GetComponent<BeachPiece>();
        }

        return p;
    }


    // Start is called before the first frame update\
    void Start()
    {
        for (int i = 0; i < INITIAL_SEGMENTS; i++)
        {
            //generate a segment
            GenerateSegment();
        }
    }

    private void GenerateSegment()
    {
        SpawnSegment();
        if (Random.Range(0f, 1f) < (continiousSegments * 0.25f))
        {
            //spawn transition seg
            continiousSegments = 0;
            SpawnTransition();
        }
        else
        {
            continiousSegments++;
        }
        //birds animations
        GameObject[] birds = new GameObject[100];
        birds = GameObject.FindGameObjectsWithTag("Bird");
        for (int i = 0; i < birds.Length; i++)
        {
            if(birds[i] == null)
            {
                continue;
            }
            Animation anim = birds[i].GetComponent<Animation>();
            if (Random.Range(0.0f, 1.0f) > 0.5f)
            {
                anim.Play("Flap");
                continue;
            }
            anim = birds[i].GetComponent<Animation>();
            anim.Play("Glide");
        }

    }

    private void SpawnSegment()
    {
        List<BeachSegment> possibleSeg = availableSegments.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        int id = Random.Range(0, possibleSeg.Count);

        BeachSegment s = GetSegment(id, false);

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
        List<BeachSegment> possibleTransition = availableTransitions.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        int id = Random.Range(0, possibleTransition.Count);

        BeachSegment s = GetSegment(id, true);

        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += s.length;
        amountOfActiveSegments++;
        s.Spawn(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(currentSpawnZ - cameraContainer.position.z < DISTANCE_BEFORE_SPAWN)
        {
            GenerateSegment();
        }

        if(amountOfActiveSegments >= MAX_SEGMENTS_ON_SCREEN)
        {
            segments[amountOfActiveSegments - 1].DeSpawn();
            amountOfActiveSegments--;
        }
    }
}
