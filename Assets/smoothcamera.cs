using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smoothcamera : MonoBehaviour
{
    public GameObject target;
    public float smoothspd;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 targetpos = target.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 targetpos = target.transform.position;
            Vector2 smoothed = Vector3.Lerp(transform.position, targetpos, smoothspd * Time.deltaTime);
            float newsmoothy = 0;
            if(smoothed.y < 0) { newsmoothy = 0; } else { newsmoothy = smoothed.y; }
            transform.position = new Vector3(smoothed.x, newsmoothy, -10);
        }
    }
}
