using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class view : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        float fov = 90;
        Vector3 origin = Vector3.zero;
        int raycount = 100;
        float angle = 0;
        float angleincrease = fov / raycount;
        float viewdistance = 50;

        Vector3[] vertices = new Vector3[raycount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[raycount * 3];

        vertices[0] = origin;
        float angleradius = angle * (Mathf.PI / 180);
        Vector3 radvector = new Vector3(Mathf.Cos(angleradius), Mathf.Sin(angleradius));

        int vertexindex = 1;
        int triangleindex = 0;
        for (int i =0; i < raycount; i++)
        {
            Vector3 vertex = origin + radvector * viewdistance;
            vertices[vertexindex] = vertex;

            if(i > 0)
            {
                triangles[triangleindex + 0] = 0;
                triangles[triangleindex + 1] = vertexindex - 1;
                triangles[triangleindex + 2] = vertexindex;
            }

            vertexindex++;

            angle -= angleincrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
