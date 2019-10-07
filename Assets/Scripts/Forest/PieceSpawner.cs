using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public PieceType type;
    private Piece currentPiece;

    public void Spawn()
    {
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

        currentPiece = ForestSpawnManager.Instance.GetPiece(type, 
            Random.Range(0, amtObj));
        currentPiece.gameObject.SetActive(true);
        currentPiece.transform.SetParent(transform, false);
    }

    public void DeSpawn()
    {
        currentPiece.gameObject.SetActive(false);
    }
}
