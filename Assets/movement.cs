using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float xforce;
    public float yforce;
    public bool onground = false;
    private bool canwallbounce = false;
    private int bounces = 2;
    private int maxbounces;
    private float dtt = 0;
    private KeyCode newkey = KeyCode.None;
    private bool candash = false;
    public float energyboostmultiplier = 0;
    public bool storing = false;
    public bool canstore = false;
    public BoxCollider2D boxcol;
    public ParticleSystem particlewb;
    public ParticleSystem particlest;
    public bool canmove;

    private float dashspeed = 10;
    private float jumpheight = 15;
    private float movespeed = 60;
    private float storespeed = 200;

    // Start is called before the first frame update
    void Start()
    {
        maxbounces = bounces;
    }

    // Update is called once per frame
    void Update()
    {
        if (canmove)
        {
            //Quick Dashing
            if (Input.GetKeyDown(newkey))
            {
                if (dtt > 0 && !onground && candash)
                {
                    if (newkey == KeyCode.A)
                    {
                        rb.AddForce(new Vector2(-dashspeed, 0), ForceMode2D.Impulse);
                    }
                    else
                    {
                        rb.AddForce(new Vector2(dashspeed, 0), ForceMode2D.Impulse);
                    }
                    dtt = 0;
                    candash = false;
                    Instantiate(particlest, transform.position, Quaternion.Euler(0, 0, 0));
                }
            }

            //Jumping and Wall Bouncing
            if (Input.GetKeyDown(KeyCode.W))
            {
                //Storing Energy Check
                if (canwallbounce == false && onground == false && storing == false && canstore)
                {
                    storing = true;
                    canstore = false;
                }

                //Wall Bounce
                if (canwallbounce == true && onground == false && bounces > 0 && storing == false)
                {
                    rb.velocity = new Vector2((xforce * -1), rb.velocity.y + (jumpheight*1.25f));
                    bounces -= 1;
                    newkey = KeyCode.None;
                    Vector3 pos = transform.position;
                    Vector3 rot = new Vector3(0, 0, 0);
                    if (xforce > 0)
                    {//Right
                        rot = new Vector3(0, -90, 0);
                        pos = new Vector3(pos.x + 0.2f, pos.y, pos.z);
                    }
                    else
                    {//Left
                        rot = new Vector3(0, 90, 0);
                        pos = new Vector3(pos.x - 0.2f, pos.y, pos.z);
                    }
                    ParticleSystem newps = Instantiate(particlewb, pos, transform.rotation);
                    newps.transform.rotation = Quaternion.Euler(rot);

                }

                //Jumping
                if (onground == true && storing == false)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpheight);
                }
            }

            //While Storing
            if (Input.GetKey(KeyCode.W) && storing)
            {
                if (onground == false)
                {
                    if (energyboostmultiplier < 100)
                    {
                        energyboostmultiplier += (storespeed*Time.deltaTime);
                        rb.gravityScale = 5 * ((100 - energyboostmultiplier) / 100);
                    }
                }
                else
                {
                    storing = false;
                    energyboostmultiplier = 0;
                    rb.gravityScale = 5;
                }
            }

            //Energy Release
            if (Input.GetKeyUp(KeyCode.W))
            {
                if (storing)
                {
                    if (energyboostmultiplier > 25)
                    {
                        //Debug.Log("Realeased with: " + energyboostmultiplier);
                        rb.AddForce(new Vector2(0, energyboostmultiplier / 8), ForceMode2D.Impulse);
                        storing = false;
                        energyboostmultiplier = 0;
                        rb.gravityScale = 5;
                        Instantiate(particlest, transform.position, Quaternion.Euler(0, 0, 0));
                    }
                }
            }

            //Move Right
            if (Input.GetKey(KeyCode.D))
            {
                float spd = movespeed;
                if (!onground)
                {
                    spd = movespeed / 2;
                    dtt = 1.0f;
                    newkey = KeyCode.D;
                }
                rb.velocity = new Vector2(rb.velocity.x + (spd*Time.deltaTime) * ((100 - energyboostmultiplier) / 100), rb.velocity.y);
                if (rb.velocity.x > xforce)
                {
                    xforce = rb.velocity.x;
                }
            }

            //Move Left
            if (Input.GetKey(KeyCode.A))
            {
                float spd = movespeed;
                if (!onground)
                {
                    spd = movespeed / 2;
                    dtt = 1.0f;
                    newkey = KeyCode.A;
                }
                rb.velocity = new Vector2(rb.velocity.x - (spd * Time.deltaTime) * ((100 - energyboostmultiplier) / 100), rb.velocity.y);
                if (rb.velocity.x < xforce)
                {
                    xforce = rb.velocity.x;
                }
            }

            //Reduce Stored Energy
            if (xforce > 0 && rb.velocity.x < xforce)
            {
                xforce -= 0.01f;
                if (xforce < 0) { xforce = 0; }
            }

            if (xforce < 0 && rb.velocity.x > xforce)
            {
                xforce += 0.01f;
                if (xforce > 0) { xforce = 0; }
            }

            //Double Tap Timer
            if (dtt > 0) { dtt -= 0.01f; }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Wall")
        {
            canwallbounce = true;
        }

        if(col.tag == "Floor")
        {
            onground = true;
            bounces = maxbounces;
            dtt = 0;
            newkey = KeyCode.None;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Floor")
        {
            onground = false;
            candash = true;
            canstore = true;
        }

        if(col.tag == "Wall")
        {
            canwallbounce = false;
        }
    }
}
