using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBehavior : entity
{
    [SerializeField] int ms, hp;
    float lastRotateInput, timeBetweenInputs;
    GameObject boss;
    SpriteRenderer hpSR;
    Sprite[] hpSprites;
    public playerBehavior() : base("playerProjectile") { }
    void Awake()
    {
        //references
        boss = GameObject.FindGameObjectWithTag("Boss");

        //healthbar initialization
        hpSR = transform.GetChild(hpbar).GetComponent<SpriteRenderer>();
        hpSprites = new Sprite[4];
        for (int i = 0; i < 4; i++)
        {
            hpSprites[i] = Resources.Load<Sprite>($"Sprites/{i.ToString()}");
        }

        //var initializations
        ms = 7;
        hp = 5;
        timeBetweenInputs = 0.2f;
        lastRotateInput = Time.time;
        shotInterval = 0.5f;
    }

    void Update()
    {
        checkMove();
        checkRotate();
        checkShoot();
    }

    void checkMove()
    {
        if(boss.GetComponent<bossAction>().canMove){
            if (!(Input.GetKey("w") && Input.GetKey("s")))
            {
                if (Input.GetKey("w") && ((transform.position.y + sr.bounds.size.y / 2) < camHeight))
                {
                    transform.Translate(new Vector2(0, 1) * ms * Time.deltaTime);
                }
                if (Input.GetKey("s") && ((transform.position.y - sr.bounds.size.y / 2) > (camHeight * -1)))
                {
                    transform.Translate(new Vector2(0, -1) * ms * Time.deltaTime);
                }
            }
            if (!(Input.GetKey("a") && Input.GetKey("d")))
            {
                if (Input.GetKey("a") && ((transform.position.x - sr.bounds.size.x / 2) > (camWidth * -1)))
                {
                    transform.Translate(new Vector2(-1, 0) * ms * Time.deltaTime);
                }
                if (Input.GetKey("d") && ((transform.position.x + sr.bounds.size.x / 2) < camWidth))
                {
                    transform.Translate(new Vector2(1, 0) * ms * Time.deltaTime);
                }
            }
        }
    }
    void checkRotate()
    {
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
    }

    //hitting the boss will add to score
    void checkShoot()
    {
        if (Input.GetKey("space"))
            {
                if (Time.time - lastShotTime > shotInterval)
                {
                    Instantiate(proj, transform.GetChild(sprite).GetChild(projSpawn).position, transform.GetChild(sprite).rotation);
                    lastShotTime = Time.time;
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