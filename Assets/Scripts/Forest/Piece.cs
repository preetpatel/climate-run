using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Types of obstacles
public enum PieceType
{
    none = -1,
    ramp = 0,
    longblock = 1,
    jump = 2,
    slide = 3
}

public class Piece : MonoBehaviour
{
    public PieceType type;
    public int visualIndex;
}
