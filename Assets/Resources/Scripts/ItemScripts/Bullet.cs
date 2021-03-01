using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Transform bulletTransform;
    public float speed;
    public int life;
    public int damage;
    // Start is called before the first frame update
    Bullet(float _speed, int _life, int _damage)
    {
        speed = _speed;
        life = _life;
        damage = _damage;
    }

    // Update is called once per frame
    void Update()
    {
        bulletTransform.Translate(Vector3.forward * Time.deltaTime * speed);
        life -= 1;
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);//TODO get trigger working
                                //TODO create bullet pool
        }
    }
}
