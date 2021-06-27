using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProjectile : projectile
{
    public playerProjectile() : base(9) { }
    void Start()
    {
        transform.Rotate(0, 0, 90);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        collide(other);
    }
}
