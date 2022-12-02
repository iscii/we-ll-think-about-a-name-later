using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossProjectile : projectile
{
    GameObject player;
    public bossProjectile() : base(13) { }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.Rotate(0, 0, -90);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag != "Boss Projectile" && other.gameObject.tag != "Boss")
        {
            collide(other);
        }

        switch (other.gameObject.tag)
        {
            case "Player":
            if(player.GetComponent<playerBehavior>().ms < 20)
                player.GetComponent<playerBehavior>().takeDamage();
                break;
            case "Player Projectile":
                Destroy(other.gameObject);
                break;
        }
    }

}