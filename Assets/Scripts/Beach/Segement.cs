using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segement : MonoBehaviour
{
    public int SegId { set; get; }
    public bool transition;

    public int length;
    public int beginY1, beginY2, beginY3;
    public int endY1, endY2, endY3;

    private BeachPieceSpawner[] pieces;

    private void Awake()
    {
        //get the piece spawners
        pieces = gameObject.GetComponentsInChildren<BeachPieceSpawner>();
        for (int i = 0; i < pieces.Length; i++)
        {
            foreach (MeshRenderer mr in pieces[i].GetComponentsInChildren<MeshRenderer>())
            {
                //if SHOW_COLLIDER is on then show all colliders
                mr.enabled = BeachLevelManager.Instance.SHOW_COLLIDER;
            }
        }
    }

    public void Spawn()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < pieces.Length; i++)
        {   
            //spawn all pieces
            pieces[i].Spawn();
        }
    }

    public void DeSpawn()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < pieces.Length; i++)
        {   
            //despawn all pieces
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
