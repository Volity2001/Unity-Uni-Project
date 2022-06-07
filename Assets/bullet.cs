using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    private GameObject player;
    public Rigidbody2D rb;
    private float spd = 5;
    private GameObject handler;
    public GameObject particle;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        handler = GameObject.Find("Game Handler");
        Vector3 targetpos = (player.transform.position - transform.position).normalized;
        rb.AddForce(targetpos * spd, ForceMode2D.Impulse);
        Destroy(this.gameObject, 5);
    }

    private void Update()
    {
        transform.Rotate(0, 0, 5);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            handler.GetComponent<handler>().TeleportPlayer();
        }
        else
        {
            Instantiate(particle, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
