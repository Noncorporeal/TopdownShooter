using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float camMaxDistance;
    public float camHight;
    public float gravity = 9.81f;
    //[HideInInspector]
    public bool isMoving;

    private Transform playerTransform;
    private Vector3 mouse;
    private Vector3 newCamPosition;
    private Transform camPosition;
    private Camera cam;
    private CharacterController controller;
    private Vector3 velocity;


    // Start is called before the first frame update
    void Start()
    {
        newCamPosition = new Vector3();
        cam = GetComponentInChildren<Camera>();
        playerTransform = GetComponent<Transform>().GetChild(0).transform;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Sprint
        Transform transform = GetComponent<Transform>();

        if (Input.GetKeyDown("left shift"))
            speed = speed * 2;
        else if (Input.GetKeyUp("left shift"))
            speed = speed / 2;

        Vector3 move = new Vector3();
        if (Input.GetKey("w"))
            move += Vector3.forward;
        if (Input.GetKey("s"))
            move += Vector3.back;
        if (Input.GetKey("d"))
            move += Vector3.right;
        if (Input.GetKey("a"))
            move += Vector3.left;

        if (!controller.isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
            move += velocity;
        }
        else
        {
            velocity.y = 0;
        }

        if (move != Vector3.zero)
        {
            controller.Move(move * Time.deltaTime * speed);
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        //Camera Movement based on mouse postion
        mouse = Input.mousePosition;
        if (mouse.x < Screen.width && mouse.x > 0 && mouse.y < Screen.height && mouse.y > 0)
        {
            camPosition = this.gameObject.transform.GetChild(1);

            newCamPosition.x = ((mouse.x * camMaxDistance / Screen.width) - (camMaxDistance / 2));
            newCamPosition.z = (mouse.y * camMaxDistance / Screen.height) - (camMaxDistance / 2) - 7;
            newCamPosition.y = camHight;
            camPosition.localPosition = newCamPosition;
        }

        //turn character towards mouse cursor
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(mouse);
        if (Physics.Raycast(ray, out hit))
        {
            playerTransform.LookAt(straighten(hit.point, playerTransform.position.y));
        }
    }

    //Sets raycast points to be at the same hight as the player.
    Vector3 straighten(Vector3 point, float yVal)
    {
        point.y = yVal;
        return point;
    }
}

