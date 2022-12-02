using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBehavior : entity
{
    public float ms;
    [SerializeField] int hp;
    int dirId, dir;
    float lastRotateInput, timeBetweenInputs, lastDashInput, dist = 3;
    Vector3 dashedPosition;
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
                    if (Input.GetKey("w"))
                    {
                        transform.Translate(new Vector2(0, 1) * ms * Time.deltaTime);
                        dirId = 0;
                    }
                    if (Input.GetKey("s"))
                    {
                        transform.Translate(new Vector2(0, -1) * ms * Time.deltaTime);
                        dirId = 2;
                    }
                }

                if (!(Input.GetKey("a") && Input.GetKey("d")))
                {
                    if (Input.GetKey("a"))
                    {
                        transform.Translate(new Vector2(-1, 0) * ms * Time.deltaTime);
                        dirId = 1;
                    }
                    if (Input.GetKey("d"))
                    {
                        transform.Translate(new Vector2(1, 0) * ms * Time.deltaTime);
                        dirId = 3;
                    }
                }
            }

            dir = dirId * 90;
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
            dir = (dir + 90) % 360;
            float angle = dir * Mathf.Deg2Rad;
            if(dir % 45 == 0) {
                int modX = (dir < 90 || dir > 270 ? 1 : -1), modY = (dir < 180 ? 1 : -1);
                if(dir % 90 == 0) {
                    dashedPosition = new Vector3(transform.position.x + dist * Mathf.Abs(Mathf.Cos(angle)) * modX, transform.position.y + dist * Mathf.Abs(Mathf.Sin(angle)) * modY, transform.position.z);
                }
                else {
                    dist = Mathf.Sqrt(Mathf.Pow(dist, 2) / 2);
                    dashedPosition = new Vector3(transform.position.x + dist * modX, transform.position.y + dist * modY, transform.position.z);
                }
            }
            dist = 3;
        }
        if (ms >= 10)
        {
            transform.position = Vector3.MoveTowards(transform.position, dashedPosition, Time.deltaTime * ms);

            //exit condition
            if (transform.position == dashedPosition) 
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
        if (hp <= 0){
            Debug.Log("You have died"); //not implementing yet for testing purposes
        }
    }
}