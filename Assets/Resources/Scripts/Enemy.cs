using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public GameObject player;
    public float gravity = 9.8f;
    public int speed;
    public float viewRange;
    private CharacterController controller;
    private Vector3 velocity;
    private bool canSeePlayer = false;

    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("Player");
        Transform position = GetComponent<Transform>();
        Vector3 move = new Vector3();
        Debug.Assert(position.position.y == 1);

        Collider[] collider = Physics.OverlapSphere(position.position, viewRange, 11);
        if (collider.Length == 3)//see if player is in visible range
        {
            RaycastHit hit;
            Vector3 direction = Straighten(player.transform.position, position.position.y);
            direction = (direction - position.position).normalized; 
            if (Physics.Raycast(position.position, direction, out hit))
            {
            Debug.DrawRay(position.position, direction, Color.blue);
                if(hit.transform.name == "Player")//See if line of sight can be made to player
                {
                    position.LookAt(Straighten(player.transform.position, position.position.y));
                    if (!controller.isGrounded)
                    {
                        velocity.y -= gravity * Time.deltaTime;
                        move += velocity;
                    }
                    else
                    {
                        velocity.y = 0;
                    }
                    move += transform.forward;
                    controller.Move(move * Time.deltaTime * speed);
                }
            }
        }
    }

    Vector3 Straighten(Vector3 point, float yVal)
    {
        point.y = yVal;
        return point;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            health -= collision.gameObject.GetComponent<Bullet>().damage;
            if(health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
