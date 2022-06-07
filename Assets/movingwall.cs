using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingwall : MonoBehaviour
{
    private float spd = 1;
    private GameObject handler;
    // Start is called before the first frame update
    void Start()
    {
        handler = GameObject.Find("Game Handler");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.x += spd*Time.deltaTime;
        transform.position = pos;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            handler.GetComponent<handler>().ResetWorld();
            Destroy(this.gameObject);
        }
    }

}
