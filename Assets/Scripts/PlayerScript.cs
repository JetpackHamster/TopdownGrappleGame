/****************************************************************************
 *  File Name: PlayerScript.cs
 *  Author: Skye Drury
 *  Digipen Email: skye.drury@digipen.edu
 *  Course: VGP
 *  Description: gets player inputs and manages player behavior
****************************************************************************/

using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    // prefabs to spawn gameobjects
    public GameObject grappleSpawnable;
    public GameObject roundSpawnable;
    public GameObject explodeParticlesSpawnable;

    // gameobject references for other game objects being referred to
    GameObject activeGrapple;
    GameObject activeRound;
    
    public int roundCount;
    public int grappleCount;
    public int coinCount;
    bool isGrapplePull = true;
    public float grappleRange;

    public bool isExplodified;
    bool isDead;

    // base color for aim arrow
    public Color baseColor;
    
    // layermask for raycast
    LayerMask rayMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Disable VSync to use targetFrameRate
        QualitySettings.vSyncCount = 0;

        // Set target frame rate to 120 FPS
        Application.targetFrameRate = 120;

        // set layermask for grapple check raycast
        rayMask = LayerMask.GetMask("Level", "Iso", "Item");
    }

    // Update is called once per frame
    void Update()
    {
        // explode if exploded
        if (isExplodified & !isDead) {
            Instantiate(explodeParticlesSpawnable, transform.position, transform.rotation);
            // disable player render and drag
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().drag = 0f;
            // display game lose
            GameObject.Find("MainCanvas").transform.GetChild(3).gameObject.GetComponent<TMP_Text>().color = Color.red;
            GameObject.Find("MainCanvas").transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = ("Game Lose! " + Mathf.FloorToInt(Time.time / 60) + ":" + Time.time % 60);
            isDead = true;
        }

        Vector3 camPosition = Camera.main.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        
        // find target direction for aim triangle
        Vector3 targetDirection = camPosition - transform.position;
        
        // find correct rotation and aim toward cursor
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

        // when LMB first pressed, or MMB held, fire round if round is had
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(2)) && roundCount > 0 && Time.time > 2 && (Input.mousePosition.y > 20f)) {
            // create fired round and add velocity
            activeRound = Instantiate(roundSpawnable, transform.position + transform.up, transform.rotation);
            activeRound.GetComponent<Rigidbody2D>().velocity += new Vector2(activeRound.transform.up.x, activeRound.transform.up.y) * 30f;
            // add recoil velocity to player and decrease count
            gameObject.GetComponent<Rigidbody2D>().velocity -= new Vector2(transform.up.x, transform.up.y) * 3f;
            roundCount--;
        }

        // disable grapple pull when scroll up, reenable with scroll down
        if (Input.mouseScrollDelta.y > 1f * Time.deltaTime) {
            isGrapplePull = false;
        } else if (Input.mouseScrollDelta.y < -1f * Time.deltaTime) {
            isGrapplePull = true;
        }

        // when RMB first pressed, create grapple point
        if (Input.GetMouseButtonDown(1)) {
            if (hit && grappleCount > 0) {
                activeGrapple = Instantiate(grappleSpawnable, hit.point, transform.rotation, hit.transform);
                // scale grapple down to correct size
                activeGrapple.transform.localScale /= activeGrapple.transform.parent.transform.localScale.x;
                grappleCount--;
                isGrapplePull = true;
            }
        }
        // when holding RMB
        if (Input.GetMouseButton(1) && activeGrapple != null) {
            // move towards activeGrapple if grapple set to pull
            if (isGrapplePull) {
                Vector2 fVector = new Vector2(activeGrapple.transform.position.x - transform.position.x, activeGrapple.transform.position.y - transform.position.y);
                fVector.Normalize();
                // if other object has a rigidbody, pull it with an equal and opposite force
                activeGrapple.transform.parent.TryGetComponent<Rigidbody2D>(out Rigidbody2D RB);
                if (RB != null) {
                    gameObject.GetComponent<Rigidbody2D>().velocity += (fVector * 0.1f);
                    RB.velocity -= (fVector * 0.1f);
                } else {
                    gameObject.GetComponent<Rigidbody2D>().velocity += (fVector * 0.2f);
                }
            }
            // set rope render position
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<LineRenderer>().enabled = true;
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(0, gameObject.transform.GetChild(0).transform.position);
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(1, activeGrapple.transform.position);
        }

        // disable grapple and rope render when RMB released
        if (Input.GetMouseButtonUp(1) && activeGrapple != null) {
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<LineRenderer>().enabled = false;
            //gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(0, activeGrapple.transform.position);
            //gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(1, activeGrapple.transform.position);
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

        // collect items by destroying them and applying their effects
        //Debug.Log(collision.transform.name);
        if (collision.transform.name.Equals("TriBundle(Clone)")) {
            DestroyItem(collision.gameObject);
            grappleCount += Random.Range(3,6);
        } else if (collision.transform.name.Equals("RoundBundle(Clone)")) {
            DestroyItem(collision.gameObject);
            roundCount += Random.Range(3,8);
        } else if (collision.transform.name.Equals("Coin(Clone)")) {
            DestroyItem(collision.gameObject);
            coinCount++;
            // win game if enough coins collected
            if (coinCount >= 4) {
                Debug.Log("Game Win! " + Mathf.FloorToInt(Time.time / 60) + ":" + Time.time % 60);
                GameObject.Find("MainCanvas").transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = ("Game Win! " + Mathf.FloorToInt(Time.time / 60) + ":" + Time.time % 60);
                grappleCount = 99999;
                roundCount = 99999;
            }
        }
    }

    // ensure grapple disabled if attached to destroyed item
    void DestroyItem(GameObject obj) {
        //Debug.Log("byebye to " + obj + "AGParent: " + activeGrapple.transform.parent);
        if (activeGrapple != null && obj == activeGrapple.transform.parent.gameObject) {
            //Debug.Log("destroying rope");
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<LineRenderer>().enabled = false;
        }
        GameObject.Destroy(obj);
    }
}
