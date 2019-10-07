using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    // class used to add names and sentences wanted from unity
    public string name;

    [TextArea(3, 10)]
    public string[] sentences;
}
