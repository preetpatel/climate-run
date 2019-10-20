using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BeachPieceType
{
    none = -1,
    ramp = 0,
    longblock = 1,
    jump = 2,
    slide = 3,
    shipwreck = 4,
    volleyballNet,
}

public class BeachPiece : MonoBehaviour

    
{   
    public BeachPieceType type;
    public int visualIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
