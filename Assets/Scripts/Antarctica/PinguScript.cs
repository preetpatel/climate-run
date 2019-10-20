using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinguScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerMotor playerScript = player.GetComponent<PlayerMotor>();
        bool isEndless = playerScript.isEndless;
        
        gameObject.SetActive(!isEndless);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
