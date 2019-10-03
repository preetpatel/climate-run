using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private int minZ = 150;
    private int maxZ = 250;
    private int minX = -3;
    private int maxX = 3;
    public new GameObject gameObject;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = minZ; i < maxZ; i += 20)
        {
            Vector3 randomPos = new Vector3(Random.Range(minX, maxX), 0, i);
            Instantiate(gameObject, randomPos, Quaternion.identity);
        }
    }

}
