using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossAction : MonoBehaviour
{
    GameObject player;
    GameObject projectile;
    SpriteRenderer spriteRenderer;
    int shotIdx;
    double[] shotIntervals = {1.25, 1, 0.75, 0.5, 0.25};
    double lastShot;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        projectile = Resources.Load<GameObject>("Prefabs/ProjectileBoss");
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        shotIdx = 0;
        lastShot = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastShot > shotIntervals[shotIdx]) {
            transform.position = new Vector2(player.transform.position.x, transform.position.y);
            Instantiate(projectile, new Vector3(transform.position.x, transform.position.y - Mathf.Ceil(spriteRenderer.bounds.size.y/2) - 0.1f, transform.position.z), transform.rotation);
            lastShot = Time.time;
            if(projectileBehavior.shots == projectileBehavior.shotsToChange && shotIdx < shotIntervals.Length - 1) {
                projectileBehavior.shots = 0;
                shotIdx++;
            }
        }
    }
}
