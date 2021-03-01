using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    private void Start()
    {
        damage = 2;
        fireRate = 2;
        range = 20;
        clip = 10;
    }

    //void Shoot();
}
