using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a segment
public class Segment : MonoBehaviour
{
    public int SegId { set; get; }
    public bool transition;

    public int length;
    public int beginY1, beginY2, beginY3;
    public int endY1, endY2, endY3;

    private PieceSpawner[] pieces;

    // Gets the segments pieces.
    private void Awake()
    {
        pieces = gameObject.GetComponentsInChildren<PieceSpawner>();
        // $$
        for (int i = 0; i < pieces.Length; i++)
            foreach (MeshRenderer mr in pieces[i].GetComponentsInChildren<MeshRenderer>())
                mr.enabled = ForestSpawnManager.Instance.SHOW_COLLIDER;
    }

    // Spawns the segment pieces
    public void Spawn()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < pieces.Length; i++)
            pieces[i].Spawn();
    }

    // Despawns the segment pieces
    public void DeSpawn()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < pieces.Length; i++)
            pieces[i].DeSpawn();
    }
}
