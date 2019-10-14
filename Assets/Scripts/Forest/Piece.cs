using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType
{
    none = -1,
    ramp = 0,
    longblock = 1,
    jump = 2,
    slide = 3,
    treeRow = 4,
    longRamp = 5,
    fallenTree = 6
}

public class Piece : MonoBehaviour
{
    public PieceType type;
    public int visualIndex;
}
