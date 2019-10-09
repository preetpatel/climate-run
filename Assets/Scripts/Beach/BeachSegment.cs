using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachSegment : MonoBehaviour
{
    public int SegId { set; get; }
    public bool transition;

    public int length;
    public int beginY1, beginY2, beginY3;
    public int endY1, endY2, endY3;

    private BeachPieceSpawner[] pieces;

    private void Awake()
    {
        pieces = gameObject.GetComponentsInChildren<BeachPieceSpawner>();
        for (int i = 0; i < pieces.Length; i++)
        {
            foreach (MeshRenderer mr in pieces[i].GetComponentsInChildren<MeshRenderer>())
            {
                mr.enabled = BeachSpawnManager.Instance.SHOW_COLLIDER;
            }
        }
    }

    public void Spawn()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].Spawn();
        }
    }

    public void DeSpawn()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].DeSpawn();
        }
    }
    // Start is called before the first frame update
    void Start()
    {


        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
