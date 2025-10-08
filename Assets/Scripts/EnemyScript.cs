using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    LayerMask rayMask_Level;
    GameObject player;

    public GameObject particleSpawnable;
    public GameObject[] lootSpawnable;

    public float terrainAvoidRange;
    public float terrainStrafeRange;
    public float targetSpeed;
    float modifiedTargetSpeed;
    public float strafeForce;

    float frameMoveAcceleration;
    bool isMoveLeft = false;
    

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
        
        // cast side ray
        RaycastHit2D sideHit = Physics2D.Raycast(transform.position + transform.right * 2, transform.right * 1, terrainStrafeRange, rayMask_Level);
        if (isMoveLeft) {
            sideHit = Physics2D.Raycast(transform.position + transform.right * -2, transform.right * -1, terrainStrafeRange, rayMask_Level);
        }
        
        // toggle strafe direction when sideHit
        if (sideHit) {
            isMoveLeft = !isMoveLeft;
        }


        // move forward when ray doesn't hit obstacle
        if (!hit) {
            frameMoveAcceleration = (modifiedTargetSpeed - gameObject.GetComponent<Rigidbody2D>().velocity.magnitude) * Time.deltaTime;
        } else {
            frameMoveAcceleration = -1 * Time.deltaTime;
            // strafe away from obstacle
            if (isMoveLeft) {
                gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(transform.right.x, transform.right.y) * -1f * strafeForce * Time.deltaTime;
            } else {
                gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(transform.right.x, transform.right.y) * 1f * strafeForce * Time.deltaTime;
            }
        }

        if (frameMoveAcceleration != 0) {
            gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(transform.up.x, transform.up.y) * frameMoveAcceleration;
        }
        
        // reset modified target speed
        modifiedTargetSpeed = targetSpeed;
    }

    void OnCollisionStay2D(Collision2D collision) {
        // detect round impact and destroy self
        if (collision.transform.name.Equals("Round(Clone)")) {
            Instantiate(particleSpawnable, transform.position, transform.rotation);
            // spawn loot if rng says yes
            if (Random.Range(0F, 10F) > 7f) {
                Instantiate(lootSpawnable[Random.Range(0, lootSpawnable.Length)], transform.position, transform.rotation);
            }
            // report death to total iso count
            GameObject.Find("Main Camera").GetComponent<CameraScript>().worldIsoCount--;
            GameObject.Destroy(gameObject);

        } else if (collision.transform.name.Equals("Player")) {
            // detect player impact and steal resources
            if (Random.Range(0F, 10F) > 9.6f && player.GetComponent<PlayerScript>().grappleCount > 0) {
                player.GetComponent<PlayerScript>().grappleCount--;
            }   
            if (Random.Range(0F, 10F) > 9.6f && player.GetComponent<PlayerScript>().roundCount > 0) {
                player.GetComponent<PlayerScript>().roundCount--;
            } 

            // explode player when player is robbed of all resources
            if(player.GetComponent<PlayerScript>().grappleCount <= 0 && player.GetComponent<PlayerScript>().roundCount <= 0 && Random.Range(0F, 10F) > 9.9f) {
                player.GetComponent<PlayerScript>().isExplodified = true;
            }
        
        // make groups of Iso move faster
        } else if (collision.transform.name.Equals("Iso(Clone)")) {
            modifiedTargetSpeed *= 1.5f;
        }
        //Debug.Log(collision.transform.name);

    }
}
