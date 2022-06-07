using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallaxbackground : MonoBehaviour
{
    public Vector2 parallaxeffectmultiplier;

    public Transform cameratransform;
    private Vector3 lastcamerapos;
    // Start is called before the first frame update
    void Start()
    {
        cameratransform = Camera.main.transform;
        lastcamerapos = cameratransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 deltamovement = cameratransform.position - lastcamerapos;
        Vector3 change = deltamovement * parallaxeffectmultiplier;
        transform.position = new Vector3(transform.position.x+change.x,transform.position.y + change.y, transform.position.z + change.z);
        lastcamerapos = cameratransform.position;
    }
}
