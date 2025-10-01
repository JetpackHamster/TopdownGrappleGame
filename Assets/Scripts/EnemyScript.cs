using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    LayerMask rayMask_Level;
    GameObject player;
    public float terrainAvoidRange;
    public float targetSpeed;

    float frameMoveAcceleration;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // set layermask for navigation raycast
        rayMask_Level = LayerMask.GetMask("Level");
        
        // define player reference
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // find player direction
        Vector3 targetDirection = player.transform.position - transform.position;

        // find correct rotation and aim toward player
        float rotation_z = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation_z - 90);
        
        // cast ray in direction of aim
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * 2, transform.up, terrainAvoidRange, rayMask_Level);

        // move forward when ray doesn't hit obstacle
        if (!hit) {
            frameMoveAcceleration = (targetSpeed - gameObject.GetComponent<Rigidbody2D>().velocity.magnitude) * Time.deltaTime;
        } else {
            frameMoveAcceleration = 0;
        }

        if (frameMoveAcceleration > 0) {
            gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(transform.up.x, transform.up.y) * frameMoveAcceleration;
        }
    }
}
