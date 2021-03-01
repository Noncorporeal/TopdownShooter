using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepStable : MonoBehaviour
{
    float worldPos;
    // Start is called before the first frame update
    void Awake()
    {
        worldPos = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = GetComponent<Transform>().position;
        temp.z = worldPos;
    }

}
