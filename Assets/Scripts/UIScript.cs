using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // update strings for UI numbers from variables and assign them to the text components
        string roundQty = ("" + player.GetComponent<PlayerScript>().roundCount);
        gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = (roundQty + " (LMB)");

        string grappleQty = ("" + player.GetComponent<PlayerScript>().grappleCount);
        gameObject.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = (grappleQty + " (RMB)");

        string coinQty = ("" + player.GetComponent<PlayerScript>().coinCount);
        gameObject.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = (coinQty + "/4 ●");   

        // display player velocity
        gameObject.transform.GetChild(4).gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = (("" + Mathf.Round(player.GetComponent<Rigidbody2D>().velocity.magnitude * 10) / 10f) + " m/s");  

    }
}
