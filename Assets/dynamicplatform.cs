using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamicplatform : MonoBehaviour
{
    private float movespeed = 1;
    private int movemode = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.x += (movespeed*Time.deltaTime) * movemode;
        transform.position = pos;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "DynamicTrigger")
        {
            Debug.Log("hit trigger");
            movemode *= -1;
        }
    }
}
