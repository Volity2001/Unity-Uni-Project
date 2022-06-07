using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject handler;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        handler = GameObject.Find("Game Handler");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && col.GetInstanceID() == GameObject.Find("Player").GetComponent<movement>().boxcol.GetInstanceID())
        {
            Debug.Log("Collided with " + col.name + " " + col.GetInstanceID());
            handler.GetComponent<handler>().checkpoint = gameObject.transform;
            handler.GetComponent<handler>().CalcPoints();
        }
    }
}
