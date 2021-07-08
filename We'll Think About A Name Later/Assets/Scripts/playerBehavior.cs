using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBehavior : entity
{
    public float ms;
    [SerializeField] int hp;
    int dirId;
    float lastRotateInput, timeBetweenInputs, lastDashInput;
    Vector2 dashPos;
    GameObject boss;
    SpriteRenderer hpSR;
    Sprite[] hpSprites;
    public playerBehavior() : base("playerProjectile") { }
    void Start()
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
        timeBetweenInputs = 0.1f;
        lastRotateInput = Time.time;
        lastDashInput = Time.time;
        shotInterval = 0.5f;
        dashPos = new Vector2(0, 0);
    }

    void Update()
    {
        checkDash();
        checkMove();
        //checkRotate();
        //checkShoot();
    }

    void checkMove()
    {
        if (boss.GetComponent<bossBehavior>().canMove)
        {
            if (ms <= 10)
            {
                if (Input.GetKey("w") && Input.GetKey("a") || Input.GetKey("w") && Input.GetKey("d") || Input.GetKey("s") && Input.GetKey("a") || Input.GetKey("s") && Input.GetKey("d"))
                    ms = Mathf.Sqrt(Mathf.Pow(7, 2) / 2f); //TODO: if we change ms we gotta change this as well
                else
                    ms = 7;
                if (!(Input.GetKey("w") && Input.GetKey("s")))
                {
                    if (Input.GetKey("w") && ((transform.position.y + sr.bounds.size.y / 2) < camHeight))
                    {
                        transform.Translate(new Vector2(0, 1) * ms * Time.deltaTime);
                        dirId = 0;
                    }
                    if (Input.GetKey("s") && ((transform.position.y - sr.bounds.size.y / 2) > (camHeight * -1)))
                    {
                        transform.Translate(new Vector2(0, -1) * ms * Time.deltaTime);
                        dirId = 2;
                    }
                }

                if (!(Input.GetKey("a") && Input.GetKey("d")))
                {
                    if (Input.GetKey("a") && ((transform.position.x - sr.bounds.size.x / 2) > (camWidth * -1)))
                    {
                        transform.Translate(new Vector2(-1, 0) * ms * Time.deltaTime);
                        dirId = 1;
                    }
                    if (Input.GetKey("d") && ((transform.position.x + sr.bounds.size.x / 2) < camWidth))
                    {
                        transform.Translate(new Vector2(1, 0) * ms * Time.deltaTime);
                        dirId = 3;
                    }
                }
            }

            int dir = dirId * 90;
            if (dirId == 3 && Input.GetKey("w")) dir += 45;
            else if (dirId == 3 && Input.GetKey("s")) dir -= 45;
            if (dirId == 1 && Input.GetKey("w")) dir -= 45;
            else if (dirId == 1 && Input.GetKey("s")) dir += 45;
            transform.GetChild(sprite).rotation = Quaternion.Euler(0, 0, dir);
            //take larger angle and subtract 45

            //implement diagonal roataion
        }
    }
    void checkRotate()
    {
        if (Time.time - lastRotateInput > timeBetweenInputs)
        {
            if (Input.GetKey("q"))
            {
                transform.GetChild(sprite).Rotate(0, 0, 90);
                lastRotateInput = Time.time;
            }
            if (Input.GetKey("e"))
            {
                transform.GetChild(sprite).Rotate(0, 0, -90);
                lastRotateInput = Time.time;
            }
        }
    }

    //hitting the boss will add to score
    void checkShoot()
    {
        if (Input.GetKey("space") && Time.time - lastShotTime > shotInterval)
        {
            Instantiate(proj, transform.GetChild(sprite).GetChild(projSpawn).position, transform.GetChild(sprite).rotation);
            lastShotTime = Time.time;
        }
    }


    void checkDash()
    {
        //may change this "1" to based on the number of clones
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time - lastDashInput >= 3 - bossBehavior.nClones)
        {
            ms *= 5;
            dashPos = transform.position;
        }
        if (ms >= 10)
        {
            transform.Translate(transform.GetChild(sprite).up * ms * Time.deltaTime);

            //TODO WORK ON THIS
            //optimization for bounds
            if (Mathf.Abs(transform.position.x) + sr.bounds.size.x / 2 > camWidth)
            {
                transform.position = new Vector2(transform.position.x <= 0 ? -camWidth + 1 : camWidth - 1, transform.position.y);
            }
            if (Mathf.Abs(transform.position.y) + sr.bounds.size.x / 2 > camHeight)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y <= 0 ? -camHeight + 1 : camHeight - 1);
            }

            //check distance bt vars
            if (Vector2.Distance(dashPos, transform.position) >= 3) 
            {
                ms = 7;
                lastDashInput = Time.time;
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