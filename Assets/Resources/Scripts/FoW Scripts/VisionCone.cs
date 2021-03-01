using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Original Implementation of Field of View, Keep for backup.
 */
[RequireComponent(typeof (MeshFilter))]
public class VisionCone : MonoBehaviour
{
    Mesh mesh;

    Vector3[] verticies;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        Vector3 player = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 playerNormal = player;
        playerNormal = player;
        playerNormal.y += 1;

        getPoints(playerNormal);
        CreateShape(playerNormal);
    }

    void CreateShape(Vector3 PlayerNormal)
    {
        mesh.Clear();
        triangles = new int[]{
           36, 0, 1,
           36, 1, 2,
           36, 2, 3,
           36, 3, 4,
           36, 4, 5,
           36, 5, 6,
           36, 6, 7,
           36, 7, 8,
           36, 8, 9,
           36, 9, 10,
           36, 10, 11,
           36, 11, 12,
           36, 12, 13,
           36, 13, 14,
           36, 14, 15,
           36, 15, 16,
           36, 16, 17,
           36, 17, 18,
           36, 18, 19,
           36, 19, 20,
           36, 20, 21,
           36, 21, 22,
           36, 22, 23,
           36, 23, 24,
           36, 24, 25,
           36, 25, 26,
           36, 26, 27,
           36, 27, 28,
           36, 28, 29,
           36, 29, 30,
           36, 30, 31,
           36, 31, 32,
           36, 32, 33,
           36, 33, 34,
           36, 34, 35,
           36, 35, 0
        };
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        
    }

    void getPoints(Vector3 playerNormal)
    {        
        Vector3 Direction = Vector3.forward;
        verticies = new Vector3[100];
        RaycastHit hit;

        /* This solution works, but does not have edge detection.*/
        for(int i = 0; i < 36; i++)
        {
            if (Physics.Raycast(playerNormal, Direction, out hit))
            {
                verticies[i] = hit.point;
            }
            Direction = Quaternion.Euler(0, 10, 0) * Direction;
        }
        verticies[36] = playerNormal;       
    }


   
}
