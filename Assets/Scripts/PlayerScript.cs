using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject grappleSpawnable;
    public GameObject roundSpawnable;

    GameObject activeGrapple;
    GameObject activeRound;
    
    public int roundCount;
    public int grappleCount;

    public Color baseColor;
    public float grappleRange;
    LayerMask rayMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Disable VSync to use targetFrameRate
        QualitySettings.vSyncCount = 0;

        // Set target frame rate to 120 FPS
        Application.targetFrameRate = 120;

        rayMask = LayerMask.GetMask("Level");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPosition = Camera.main.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        // find target direction for aim triangle
        Vector3 targetDirection = camPosition - transform.position;
        
        // Calculate a rotation a step closer to the target and applies rotation to this object
        float rotation_z = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation_z - 90);
        
        // cast ray in direction of aim
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * 2, transform.up, grappleRange, rayMask);
        
        // change aim arrow color by whether can grapple
        if (!hit) {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        } else {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = baseColor;
        }

        // when LMB first pressed, fire round if round is had
        if (Input.GetMouseButtonDown(0) && roundCount > 0 && Time.time > 2) {
            // create fired round and add velocity
            activeRound = Instantiate(roundSpawnable, transform.position + transform.up, transform.rotation);
            activeRound.GetComponent<Rigidbody2D>().velocity += new Vector2(activeRound.transform.up.x, activeRound.transform.up.y) * 30f;
            // add recoil velocity to player and decrease count
            gameObject.GetComponent<Rigidbody2D>().velocity -= new Vector2(transform.up.x, transform.up.y) * 3f;
            roundCount--;
        }

        // when RMB first pressed, create grapple point
        if (Input.GetMouseButtonDown(1)) {
            if (hit && grappleCount > 0) {
                activeGrapple = Instantiate(grappleSpawnable, hit.point, transform.rotation);
                grappleCount--;
            }
        }
        // move towards activeGrapple when holding RMB
        if (Input.GetMouseButton(1) && activeGrapple != null) {
            Vector2 fVector = new Vector2(activeGrapple.transform.position.x - transform.position.x, activeGrapple.transform.position.y - transform.position.y);
            fVector.Normalize();
            gameObject.GetComponent<Rigidbody2D>().velocity += (fVector * 0.2f);
        }

        // disable grapple when RMB released
        if (Input.GetMouseButtonUp(1)) {
            activeGrapple = null;
        }

        
    }

    void OnCollisionStay2D(Collision2D collision) {
        // dash off of surfaces
        if (Input.GetKey(KeyCode.Space)) {

            // dash away from contact surface
            Vector2 fVector = new Vector2(collision.transform.position.x - transform.position.x, collision.transform.position.y - transform.position.y);
            fVector.Normalize();
            
            float dashX = 0;
            float dashY = 0;
            // change dash direction by keys pressed
            if (Input.GetKey(KeyCode.W)) {
                dashY++;
            }
            if (Input.GetKey(KeyCode.S)) {
                dashY--;
            }
            if (Input.GetKey(KeyCode.A)) {
                dashX--;
            }
            if (Input.GetKey(KeyCode.D)) {
                dashX++;
            }
            // apply dash force
            fVector.x -= dashX;
            fVector.y -= dashY;
            gameObject.GetComponent<Rigidbody2D>().velocity -= fVector;
        }

        // collect items
        //Debug.Log(collision.transform.name);
        if (collision.transform.name.Equals("TriBundle(Clone)")) {
            GameObject.Destroy(collision.gameObject);
            grappleCount += Random.Range(3,6);
        } else if (collision.transform.name.Equals("RoundBundle(Clone)")) {
            GameObject.Destroy(collision.gameObject);
            roundCount += Random.Range(3,6);
        }
    }
}
