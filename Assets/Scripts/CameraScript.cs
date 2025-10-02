using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject playerObj;
    public GameObject[] encounterPrefabs;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // update position to player object position
        gameObject.transform.position = new Vector3 (playerObj.transform.position.x, playerObj.transform.position.y, -10F);

        // spawn random encounters
        while (Random.Range(0F, 10F) > (10F - (0.5f * Time.deltaTime))) {
            int xDir = 0;
            while(xDir == 0) {
                xDir = Random.Range(-1, 1);
            }
            int yDir = 0;
            while(yDir == 0) {
                yDir = Random.Range(-1, 1);
            }
            Debug.Log("xDir: " + xDir + " yDir: " + yDir);
            Instantiate(encounterPrefabs[0], new Vector3(transform.position.x + Random.Range(50F, 60F) * xDir, transform.position.y + Random.Range(50F, 60F) * yDir, 0), new Quaternion(0F, 0F, Random.Range(-1F, 1F), 1F));
        }
        //Debug.Log("Time.deltaTime: " + Time.deltaTime);
        // 0.07ish

    }
}
