using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretgun : MonoBehaviour
{
    private float spd = 5;
    private GameObject player;

    private float shottime = 1;
    private float currenttime;
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        currenttime = Random.Range(shottime,shottime*4);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < 7)
        {
            Vector2 dir = player.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, spd * Time.deltaTime);

            currenttime -= 1 * Time.deltaTime;
            if (currenttime <= 0)
            {
                Instantiate(bullet, transform.position, transform.rotation);
                currenttime = Random.Range(shottime, shottime * 4);
            }
        }
    }
}
