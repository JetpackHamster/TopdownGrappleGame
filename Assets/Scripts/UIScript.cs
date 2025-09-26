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
        string roundQty = ("" + player.GetComponent<PlayerScript>().roundCount);
        gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = (roundQty + "");

        string grappleQty = ("" + player.GetComponent<PlayerScript>().grappleCount);
        gameObject.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = (grappleQty + "");
    }
}
