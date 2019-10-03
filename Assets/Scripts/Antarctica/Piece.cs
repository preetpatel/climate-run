using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType
{
    none = -1,
    hole = 0
}

public class Piece : MonoBehaviour
{
    public PieceType type;
    public int visualIndex;
}
