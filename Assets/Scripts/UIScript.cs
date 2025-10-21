using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public GameObject player;
    GameObject mainCam;
    public bool isVelocityDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        mainCam = GameObject.Find("Main Camera");

        // toggle velocity display when button
        Button velocityButton = GetComponentInChildren<Button>();
        velocityButton.onClick.AddListener(toggleVelocityDisplay);
    }

    // Update is called once per frame
    void Update()
    {
        if (isVelocityDisplay) {
            // set VelocityDisplay render position
            gameObject.GetComponent<LineRenderer>().enabled = true;
            gameObject.GetComponent<LineRenderer>().SetPosition(0, gameObject.transform.position);
            gameObject.GetComponent<LineRenderer>().SetPosition(1, new Vector3(gameObject.transform.position.x + player.GetComponent<Rigidbody2D>().velocity.x, gameObject.transform.position.y + player.GetComponent<Rigidbody2D>().velocity.y, -8));
        } else {
            gameObject.GetComponent<LineRenderer>().enabled = false;
        }

        // update strings for UI numbers from variables and assign them to the text components
        string roundQty = ("" + player.GetComponent<PlayerScript>().roundCount);
        gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = (roundQty + " (LMB)");

        string grappleQty = ("" + player.GetComponent<PlayerScript>().grappleCount);
        gameObject.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = (grappleQty + " (RMB)");

        string coinQty = ("" + player.GetComponent<PlayerScript>().coinCount);
        gameObject.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = (coinQty + "/4 ●");   

        // display player velocity
        gameObject.transform.GetChild(4).gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = (("" + Mathf.Round(player.GetComponent<Rigidbody2D>().velocity.magnitude * 10) / 10f) + " m/s");  

        // display world iso count from main camera
        gameObject.transform.GetChild(5).GetComponent<TMP_Text>().text = ("" + mainCam.GetComponent<CameraScript>().worldIsoCount);
    }

    // toggle display when button
    void toggleVelocityDisplay() {
        isVelocityDisplay = !isVelocityDisplay;
    }
}
