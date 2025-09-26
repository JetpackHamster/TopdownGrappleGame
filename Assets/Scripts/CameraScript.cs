using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject playerObj;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // update position to player object position
        gameObject.transform.position = new Vector3 (playerObj.transform.position.x, playerObj.transform.position.y, -10F);

    }
}
