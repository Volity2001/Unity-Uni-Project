using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hazard : MonoBehaviour
{
    private GameObject handler;
    // Start is called before the first frame update
    void Start()
    {
        handler = GameObject.Find("Game Handler");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            handler.GetComponent<handler>().TeleportPlayer();
        }
    }

}
