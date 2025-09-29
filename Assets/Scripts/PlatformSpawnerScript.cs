using UnityEngine;

public class PlatformSpawnerScript : MonoBehaviour
{
    public Vector2 moveDirection;
    Vector2 tempMoveDirection;
    public float moveSpeed;
    public float generationTime;
    

    public GameObject[] platformsSpawnable;
    public GameObject[] itemsSpawnable;

    public GameObject[] objectiveItemsSpawnable;
    public int objectiveItemCount;
    public int objectiveItemsSpawned = 0;

    GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // limit how long platforms will be spawned for
        if (Time.time < generationTime) {
            // spawn platforms
            while (Random.Range(0F, 10F) > 8F) {
                Instantiate(platformsSpawnable[Random.Range(0, platformsSpawnable.Length)], new Vector3(transform.position.x + Random.Range(0F, 30F), transform.position.y + Random.Range(0F, 30F), 0), new Quaternion(0F, 0F, Random.Range(-1F, 1F), 1F));
            }

            // spawn items
            while (Random.Range(0F, 10F) > 9.6F) {
                Instantiate(itemsSpawnable[Random.Range(0, itemsSpawnable.Length)], new Vector3(transform.position.x + Random.Range(0F, 30F), transform.position.y + Random.Range(0F, 30F), 0), new Quaternion(0F, 0F, Random.Range(-1F, 1F), 1F));
            }

            // spawn objective items spread throughout generation time
            if ((generationTime / objectiveItemCount) < (Time.time / (objectiveItemsSpawned))) {
                Instantiate(objectiveItemsSpawnable[Random.Range(0, objectiveItemsSpawnable.Length)], new Vector3(transform.position.x + Random.Range(0F, 30F), transform.position.y + Random.Range(0F, 30F), 0), new Quaternion(0F, 0F, Random.Range(-1F, 1F), 1F));
                objectiveItemsSpawned++;
            }


            // randomly adjust movement
            moveDirection.x += Random.Range(-10f, 10f);
            moveDirection.y += Random.Range(-10f, 10f);

            // divide to move back toward 0
            moveDirection /= (1 + 1 * Time.deltaTime);

            tempMoveDirection = moveDirection;
            tempMoveDirection.Normalize();

            // move in direction movedirection
            transform.position = new Vector3(transform.position.x + (tempMoveDirection.x * Time.deltaTime * moveSpeed), transform.position.y + (tempMoveDirection.y * Time.deltaTime * moveSpeed), 0);
        }
    }
}
