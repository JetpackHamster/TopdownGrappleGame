using UnityEngine;

public class RoundScript : MonoBehaviour
{
    float startTime;
    float stopTime = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // if velocity too low and enough time has passed since spawn, spawn particles and set stop time
        if (gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < 3f && (Time.time - startTime > 0.2f) && stopTime == 0) {
            stopTime = Time.time;
            // stop rotation and disable sprite
            gameObject.GetComponent<Rigidbody2D>().angularDrag = 100f;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            // play particles
            gameObject.GetComponent<ParticleSystem>().Play();
        // if enough time has passed after stop time, destroy self
        } else if (stopTime != 0 && Time.time - stopTime > 0.2f) {  
            GameObject.Destroy(gameObject);
        }
    }
}
