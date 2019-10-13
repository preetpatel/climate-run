using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGaps : MonoBehaviour
{
    private float minZ = 0;
    private float maxZ = 100;
    public List<GameObject> gaps = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        minZ = transform.position.z - 10;
        maxZ = transform.position.z + 100;

        for (int i = (int) minZ; i < maxZ; i += 10)
        {
            GameObject gap = gaps[Random.Range(0, gaps.Count)];
            Instantiate(gap, new Vector3(0, 0, i), Quaternion.identity, gameObject.transform);
        }
    }

}