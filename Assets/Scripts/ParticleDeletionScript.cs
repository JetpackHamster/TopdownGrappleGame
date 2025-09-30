using UnityEngine;

public class ParticleDeletionScript : MonoBehaviour
{
    float startTime;
    public float lifetime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > lifetime) {
            GameObject.Destroy(gameObject);
        }
    }
}
