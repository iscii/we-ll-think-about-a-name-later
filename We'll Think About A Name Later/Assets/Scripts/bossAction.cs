using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossAction : MonoBehaviour
{
    GameObject player;
    GameObject projectile;
    SpriteRenderer spriteRenderer;
    double lastShot, shotInterval;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        projectile = Resources.Load<GameObject>("Prefabs/ProjectileBoss");
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        lastShot = Time.time;
        shotInterval = 0.5;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(player.transform.position.x, transform.position.y);
        if(Time.time - lastShot > shotInterval) {
            Instantiate(projectile, new Vector3(transform.position.x, transform.position.y - Mathf.Ceil(spriteRenderer.bounds.size.y/2) - 0.1f, transform.position.z), transform.rotation);
            lastShot = Time.time;
        }
    }
}
