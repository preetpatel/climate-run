using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public PieceType type;
    private Piece currentPiece;

    // Spawns the pieces to show the segment parts.
    public void Spawn()
    {
        // Gets the correct number of pieces of the input type
        int amtObj = 0;
        switch (type)
        {
            case PieceType.jump:
                amtObj = ForestSpawnManager.Instance.jumps.Count;
                break;
            case PieceType.slide:
                amtObj = ForestSpawnManager.Instance.slides.Count;
                break;
            case PieceType.longblock:
                amtObj = ForestSpawnManager.Instance.longblocks.Count;
                break;
            case PieceType.ramp:
                amtObj = ForestSpawnManager.Instance.ramps.Count;
                break;
        }

        // Randomly selects the piece to display
        currentPiece = ForestSpawnManager.Instance.GetPiece(type, 
            Random.Range(0, amtObj));
        currentPiece.gameObject.SetActive(true);
        currentPiece.transform.SetParent(transform, false);
    }

    // Despawns the piece
    public void DeSpawn()
    {
        currentPiece.gameObject.SetActive(false);
    }
}
