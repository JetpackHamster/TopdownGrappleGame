using UnityEngine;

public class RoundScript : MonoBehaviour
{
    float startTime;
    public GameObject roundParticleSpawnable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // if velocity too low and enough time has passed since spawn, spawn particles and set stop time
        if (gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < 6f && (Time.time - startTime > 0.2f)) {
            
            // stop rotation and disable sprite
            //gameObject.GetComponent<Rigidbody2D>().angularDrag = 100f;
            //gameObject.GetComponent<SpriteRenderer>().enabled = false;
            // play particles and destroy self
            Instantiate(roundParticleSpawnable, transform.position, transform.rotation);
            GameObject.Destroy(gameObject); 
        }
    }
}
