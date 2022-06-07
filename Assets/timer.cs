using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    public float currenttime;
    public float starttime;

    // Start is called before the first frame update
    void Start()
    {
        currenttime = starttime;
    }

    // Update is called once per frame
    void Update()
    {
        currenttime -= 1 * Time.deltaTime;
        int newtime = Mathf.RoundToInt(currenttime);
        GetComponent<Text>().text = newtime.ToString();
        if(newtime <= 0)
        {
            GameObject.Find("Game Handler").GetComponent<handler>().ResetWorld();
        }
    }

    public void TimeReset(float newtime)
    {
        starttime = newtime;
        currenttime = starttime;
    }
}
