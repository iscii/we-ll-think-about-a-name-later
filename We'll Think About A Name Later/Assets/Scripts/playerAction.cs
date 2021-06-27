using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAction : MonoBehaviour
{
    //children indices
    const int sprite = 0, projSpawn = 0, hpbar = 1;
    int ms;
    [SerializeField] int hp;
    float lastRotateInput, timeBetweenInputs, lastShotInput, shotInterval;
    Sprite[] hpSprites;
    GameObject boss;
    SpriteRenderer playerSR, hpSR;
    private Camera cam;
    protected float camHeight, camWidth;
    protected GameObject player, projectile;


    // Start is called before the first frame update
    private void Awake()
    {
        //gets the width and height of the camera view borders
        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
        
        projectile = Resources.Load<GameObject>("Prefabs/playerProjectile");

        
        boss = GameObject.FindGameObjectWithTag("Boss");
        playerSR = transform.GetChild(sprite).GetComponent<SpriteRenderer>();
        hpSR = transform.GetChild(hpbar).GetComponent<SpriteRenderer>();
        hpSprites = new Sprite[4];
        for (int i = 0; i < 4; i++)
        {
            hpSprites[i] = Resources.Load<Sprite>("Sprites/" + i.ToString());
        }

        ms = 7;
        hp = 5;
        timeBetweenInputs = 0.2f;
        shotInterval = 0.5f;
        lastRotateInput = Time.time;
        //lastShotInput = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.GetComponent<bossAction>().canMove)
        {
            if (!(Input.GetKey("w") && Input.GetKey("s")))
            {
                if (Input.GetKey("w") && ((transform.position.y + playerSR.bounds.size.y/2) < camHeight))
                {
                    transform.Translate(new Vector2(0, 1) * ms * Time.deltaTime);
                }
                if (Input.GetKey("s") && ((transform.position.y - playerSR.bounds.size.y/2) > (camHeight*-1)))
                {
                    transform.Translate(new Vector2(0, -1) * ms * Time.deltaTime);
                }
            }
            if (!(Input.GetKey("a") && Input.GetKey("d")))
            {
                if (Input.GetKey("a") && ((transform.position.x - playerSR.bounds.size.x/2) > (camWidth*-1)))
                {
                    transform.Translate(new Vector2(-1, 0) * ms * Time.deltaTime);
                }
                if (Input.GetKey("d") && ((transform.position.x + playerSR.bounds.size.x/2) < camWidth))
                {
                    transform.Translate(new Vector2(1, 0) * ms * Time.deltaTime);
                }
            }

            if (Input.GetKey("q"))
            {
                if (Time.time - lastRotateInput > timeBetweenInputs)
                {
                    transform.GetChild(sprite).Rotate(0, 0, 90);
                    lastRotateInput = Time.time;
                }
            }
            if (Input.GetKey("r"))
            {
                if (Time.time - lastRotateInput > timeBetweenInputs)
                {
                    transform.GetChild(sprite).Rotate(0, 0, -90);
                    lastRotateInput = Time.time;
                }
            }

            if (Input.GetKey("space")) //hitting the boss adds score
            {
                if (Time.time - lastShotInput > shotInterval)
                {
                    Instantiate(projectile, transform.GetChild(sprite).GetChild(projSpawn).position, transform.GetChild(sprite).rotation);
                    lastShotInput = Time.time;
                }
            }
        }
    }

    public void takeDamage()
    {
        hp--;
        hpSR.sprite = hpSprites[hp > 0 ? hp - 1 : 0];
        if (hp <= 0)
            Debug.Log("You have died"); //not implementing yet for testing purposes
    }
}
