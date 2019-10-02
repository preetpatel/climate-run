using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType
{
    squareHole = -3,
    longHorizontalHole = -2,
    longVerticalHole = -1, // more of in second stage
    squareTile = 0, // show in second stage
    longHorizontalTile = 1,
    longVerticalTile = 2
}

public class Piece : MonoBehaviour
{

    public PieceType piece;
    public int visualIndex;
}
