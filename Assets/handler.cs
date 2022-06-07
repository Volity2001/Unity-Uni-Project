using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class handler : MonoBehaviour
{
    public int points;
    public int currentpoints;
    public Transform checkpoint;
    public Transform startpoint;
    public GameObject player;
    public GameObject text;
    private float textalpha = 0;
    public bool textonscreen = false;
    private int textfademode = 1;
    public GameObject[] segments;
    public GameObject[] checkpoints;
    public GameObject[] barriers;
    public GameObject timer;
    public GameObject[] backgroundbasearray;
    public GameObject[] backgroundfirstarray;
    public GameObject backgroundbase;
    public GameObject backgroundfirst;
    public GameObject[] screenwave;
    public GameObject redflash;
    public GameObject blackflash;
    public bool waving;
    private float wavepos = 25;
    private bool flashing = false;
    private int flashmode = 0;
    private string leveltext;
    private bool dotext;
    private int textmode;
    private bool blackflashing;
    private int blackmode;
    public string[] textpopups;

    private float redspeed = 4;
    private float slidespeed = 200;
    private float blackspeed = 1.0f;
    private float textspeed = 0.8f;

    public GameObject movingwall;
    private GameObject currwall;

    public GameObject checkbox;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        startpoint = GameObject.Find("StartPoint").gameObject.transform;
        checkpoint = GameObject.Find("StartPoint").gameObject.transform;

        for(int i = 0; i < segments.Length; i++)
        {
            Debug.Log(i + " iterations");
            if(i >= points)
            {
                Debug.Log("removing " + i);
                segments[i].SetActive(false);
            }
        }

        CreateBackground(backgroundbasearray, backgroundbase,5, 15, 3, 8, "Base Background");
        CreateBackground(backgroundfirstarray, backgroundfirst, 5, 15, 6, 15, "Level 1 Background");
        Application.targetFrameRate = 144;
    }

    // Update is called once per frame
    void Update()
    {
        if(textonscreen == true)
        {
            if(textfademode == 1)
            {
                textalpha += 0.003f;
                if (textalpha >= 1.0f) { textfademode *= -1; }
            }
            else
            {
                textalpha -= 0.003f;
                if (textalpha <= 0.0f) { textfademode *= -1; textonscreen = false; }
            }
            Color newcol = text.GetComponent<Text>().color;
            newcol.a = textalpha;
            text.GetComponent<Text>().color = newcol;
        }

        if (Vector3.Distance(new Vector3(player.transform.position.x, 0, 0), new Vector3(backgroundbasearray[0].transform.position.x, 0, 0)) > 30)
        {
            ShiftBackground(backgroundbasearray, -1, backgroundbase, 5, 15, 3, 8, "Base Background");
            Debug.Log(Time.deltaTime + ": Added new to the RIGHT");
        }

        if (Vector3.Distance(new Vector3(player.transform.position.x, 0, 0), new Vector3(backgroundbasearray[9].transform.position.x, 0, 0)) > 30)
        {
            ShiftBackground(backgroundbasearray, 1, backgroundbase, 5, 15, 3, 8, "Base Background");
            Debug.Log(Time.deltaTime + ": Added new to the LEFT");
        }

        if (Vector3.Distance(new Vector3(player.transform.position.x, 0, 0), new Vector3(backgroundfirstarray[0].transform.position.x, 0, 0)) > 30)
        {
            ShiftBackground(backgroundfirstarray, -1, backgroundfirst, 5, 15, 6, 15, "Level 1 Background");
            Debug.Log(Time.deltaTime + ": Added new to the RIGHT");
        }

        if (Vector3.Distance(new Vector3(player.transform.position.x, 0, 0), new Vector3(backgroundfirstarray[9].transform.position.x, 0, 0)) > 30)
        {
            ShiftBackground(backgroundfirstarray, 1, backgroundfirst, 5, 15, 6, 15, "Level 1 Background");
            Debug.Log(Time.deltaTime + ": Added new to the LEFT");
        }

        if (Input.GetKey(KeyCode.R)) { TeleportPlayer(); }
    }

    private void FixedUpdate()
    {
        if (flashing) { DoRedFlash(); }
        if (waving) { DoScreenWave(); }
        if (blackflashing) { DoBlackFlash(); }
        if (dotext) { DoText(); }
    }

    public void TeleportPlayer()
    {
        if ((points == 11 && currentpoints == 10) && (checkpoint.position.x < currwall.transform.position.x))
        {
            ResetWorld();
        }
        else
        {
            player.transform.position = checkpoint.transform.position;
            ResetVariables();
            TriggerRedFlash();
        }
    }

    public void EnableObjects()
    {
        for (int i = 0; i < segments.Length; i++)
        {
            if (i < points)
            {
                Debug.Log("adding " + i);
                segments[i].SetActive(true);
                checkpoints[i].SetActive(true);
                barriers[i].SetActive(true);
            }
        }
    }

    public void ResetWorld()
    {
        player.transform.position = startpoint.transform.position;
        ResetVariables();
        currentpoints = 0;
        EnableObjects();
        checkpoint = startpoint;
        timer.GetComponent<timer>().TimeReset(100);
        TriggerBlackFlash();
        checkbox.GetComponent<Text>().text = currentpoints + "/" + points;

        if (currwall != null)
        {
            Destroy(currwall);
            currwall = null;
        }

        for(int i = 0; i < backgroundbasearray.Length; i++)
        {
            Destroy(backgroundbasearray[i]);
        }

        for (int i = 0; i < backgroundfirstarray.Length; i++)
        {
            Destroy(backgroundfirstarray[i]);
        }

        CreateBackground(backgroundbasearray, backgroundbase, 5, 15, 3, 8, "Base Background");
        CreateBackground(backgroundfirstarray, backgroundfirst, 5, 15, 6, 15, "Level 1 Background");
    }

    public void ResetVariables()
    {
        player.GetComponent<movement>().onground = true;
        player.GetComponent<movement>().storing = false;
        player.GetComponent<movement>().canstore = false;
        player.GetComponent<movement>().energyboostmultiplier = 0;
        player.GetComponent<Rigidbody2D>().gravityScale = 5;
    }
    public void CalcPoints()
    {
        Debug.Log("calculating points");
        currentpoints++;
        if(currentpoints >= points)
        {
            if (points < 11)
            {
                points++;
                ResetWorld();
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("End Scene");
            }
        }
        else
        {
            barriers[currentpoints-1].SetActive(false);
            checkpoints[currentpoints - 1].SetActive(false);
            Debug.Log("DISABLE OBJECTS: " + (currentpoints - 1));

            if(currentpoints >= 3)
            {
                barriers[currentpoints - 3].SetActive(true);
                segments[currentpoints - 3].SetActive(false);
            }
        }

        if(points == 11 && currentpoints == 10)
        {
            Vector3 wallpos = new Vector3(player.transform.position.x - 11, 0.15f, 26.59912f);
            currwall = Instantiate(movingwall, wallpos, transform.rotation);
        }

        checkbox.GetComponent<Text>().text = currentpoints + "/" + points;
    }

    public void DisplayText(string display)
    {
        textonscreen = true;
        text.GetComponent<Text>().text = display;
        textalpha = 0;
        textfademode = 1;
    }

    public void CreateBackground(GameObject[] background,GameObject backgroundobj, int xscalemin, int xscalemax, int yscalemin, int yscalemax,string parentname)
    {
        //Scale 3:1 to position

        //start at camera -10, end at camera +10
        float pos = -15;
        for (int i = 0; i < 10; i++){
            int xscalevalue = 13; //Mathf.RoundToInt(Random.Range(xscalemin, xscalemax));
            int yscalevalue = Mathf.RoundToInt(Random.Range(yscalemin, yscalemax));
            BackgroundObj(backgroundobj, xscalevalue, yscalevalue, parentname, pos, i, 0, background);
            pos += 3.5f;
        }
    }

    public void BackgroundObj(GameObject backgroundobj, int xscalevalue, int yscalevalue, string parentname, float pos, int arraypos, int mode, GameObject[] background)
    {
        float newpos = 0;
        float ypos = 0;
        if (mode == 0) //Adding to the right
        {
            newpos = pos + (xscalevalue / 3);
            if(background[8] != null) ypos = background[8].transform.position.y; else ypos = -4.5f;
        }
        else //Adding to the left
        {
            newpos = pos - (xscalevalue / 3);
            if (background[1] != null) ypos = background[1].transform.position.y; else ypos = -4.5f;
        }
        GameObject newbackground = Instantiate(backgroundobj, new Vector3(newpos, ypos, 0), Quaternion.Euler(0, 0, 0));
        background[arraypos] = newbackground;
        newbackground.transform.SetParent(GameObject.Find(parentname).transform);
        newbackground.transform.localScale = new Vector3(xscalevalue, yscalevalue, 0);
    }

    public void ShiftBackground(GameObject[] background, int direction, GameObject backgroundobj, int xscalemin, int xscalemax, int yscalemin, int yscalemax, string parentname)
    {
        GameObject[] storedbackground = background;

        if(direction == -1) //moving right
        {
            Destroy(background[0]);
            for(int i = 1; i <= 9; i++)
            {
                background[i-1] = storedbackground[i];
            }
            background[9] = null;

            float endpos = background[8].transform.position.x + (background[8].transform.localScale.x / 3) - 1;
            int xscalevalue = Mathf.RoundToInt(Random.Range(xscalemin, xscalemax));
            int yscalevalue = Mathf.RoundToInt(Random.Range(yscalemin, yscalemax));
            BackgroundObj(backgroundobj, xscalevalue, yscalevalue, parentname, endpos, 9, 0, background);
        }
        else if(direction == 1) //moving left
        {
            Destroy(background[9]);
            for (int i = 8; i >= 0; i--)
            {
                background[i + 1] = storedbackground[i];
            }
            background[0] = null;

            float endpos = background[1].transform.position.x - (background[1].transform.localScale.x / 3) + 1;
            int xscalevalue = Mathf.RoundToInt(Random.Range(xscalemin, xscalemax));
            int yscalevalue = Mathf.RoundToInt(Random.Range(yscalemin, yscalemax));
            BackgroundObj(backgroundobj, xscalevalue, yscalevalue, parentname, endpos, 0, 1, background);
        }
    }

    public void DoScreenWave()
    {
        screenwave[0].transform.position = new Vector3(screenwave[0].transform.position.x - (slidespeed*Time.deltaTime), 0, 0);
        screenwave[1].transform.position = new Vector3(screenwave[1].transform.position.x - ((slidespeed*0.8f)*Time.deltaTime), 0, 0);
        if (screenwave[1].transform.position.x <= -(Camera.main.ScreenToViewportPoint(Vector3.one * 0.5f).x + wavepos)) { waving = false; }
    }

    public void TriggerScreenWave()
    {
        waving = true;
        Vector3 pos = Camera.main.transform.position;
        screenwave[0].transform.position = new Vector3(pos.x + wavepos, pos.y, 0);
        screenwave[1].transform.position = new Vector3(pos.x + wavepos, pos.y, 0);
    }

    public void TriggerRedFlash()
    {
        flashing = true;
        flashmode = 0;

        Vector3 pos = redflash.transform.position;
        pos.x = Camera.main.transform.position.x;
        pos.y = Camera.main.transform.position.y;
        redflash.transform.position = pos;
    }

    public void TriggerBlackFlash()
    {
        blackflashing = true;
        blackmode = 0;
        player.GetComponent<movement>().canmove = false;
        Color currentcol = blackflash.GetComponent<Image>().color;
        currentcol.a = 1.0f;
        blackflash.GetComponent<Image>().color = currentcol;
        TriggerText();
        Debug.Log("triggered black");

        Vector3 pos = blackflash.transform.position;
        pos.x = Camera.main.transform.position.x;
        pos.y = Camera.main.transform.position.y;
        blackflash.transform.position = pos;
    }

    public void TriggerText()
    {
        dotext = true;
        textmode = 0;
        text.GetComponent<Text>().color = new Color(1, 1, 1, 0);
        text.GetComponent<Text>().text = textpopups[points-1];
        Debug.Log("triggered text");
    }

    public void DoRedFlash()
    {
        Color currentcol = redflash.GetComponent<Image>().color;
        if (flashmode == 0)
        {
            currentcol.a += redspeed*Time.deltaTime;
            if (currentcol.a >= 0.5f) { flashmode = 1; }
        }
        else
        {
            currentcol.a -= redspeed*Time.deltaTime;
            if (currentcol.a <= 0) { flashing = false; }
        }
        redflash.GetComponent<Image>().color = currentcol;
    }

    public void DoBlackFlash()
    {
        Color currentcol = blackflash.GetComponent<Image>().color;
        if(blackmode == 1)
        {
            currentcol.a -= blackspeed*Time.deltaTime;
            if (currentcol.a <= 0) { blackflashing = false; TriggerScreenWave(); player.GetComponent<movement>().canmove = true; }
        }
        blackflash.GetComponent<Image>().color = currentcol;
    }

    public void DoText()
    {
        Color currentcol = text.GetComponent<Text>().color;
        if (textmode == 0)
        {
            Debug.Log("text enhance");
            currentcol.a += textspeed*Time.deltaTime;
            if (currentcol.a >= 0.5f) { textmode = 1; }
        }
        else
        {
            Debug.Log("text dehance");
            currentcol.a -= textspeed*Time.deltaTime;
            if (currentcol.a <= 0) { dotext = false; blackmode = 1;}
        }
        text.GetComponent<Text>().color = currentcol;
    }
}
