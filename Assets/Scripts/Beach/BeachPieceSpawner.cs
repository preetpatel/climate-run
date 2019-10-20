using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachPieceSpawner : MonoBehaviour
{
    public BeachPieceType type;
    private BeachPiece currentPiece;

    public void Spawn()
    {
        int amtObj = 0;
        switch (type)
        {
            case BeachPieceType.jump:
                amtObj = BeachSpawnManager.Instance.jumps.Count;
                break;
            case BeachPieceType.slide:
                amtObj = BeachSpawnManager.Instance.slides.Count;
                break;
            case BeachPieceType.longblock:
                amtObj = BeachSpawnManager.Instance.longblocks.Count;
                break;
            case BeachPieceType.ramp:
                amtObj = BeachSpawnManager.Instance.ramps.Count;
                break;
            case BeachPieceType.shipwreck:
                amtObj = BeachSpawnManager.Instance.shipwrecks.Count;
                break;
        }

        currentPiece = BeachSpawnManager.Instance.GetPiece(type, Random.Range(0, amtObj));
        currentPiece.gameObject.SetActive(true);
        currentPiece.transform.SetParent(transform, false);
    }
    public void DeSpawn()
    {
        currentPiece.gameObject.SetActive(false);
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
