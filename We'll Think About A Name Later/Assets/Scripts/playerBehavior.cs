using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBehavior : entity
{
    private const int sprite = 0, hpBar = 1;
    private float rotateTime, rotateInterval;
    private GameObject boss;
    private SpriteRenderer hpSR;
    private Sprite[] hpSprites;
    [SerializeField] int ms, hp;

    //initializes all the variables 
    private void Awake() {
        boss = GameObject.FindGameObjectWithTag("Boss");
        hpSR = transform.GetChild(hpBar).GetComponent<SpriteRenderer>();
        hpSprites = new Sprite[4];
        for (int i = 0; i < 4; i++)
        {
            hpSprites[i] = Resources.Load<Sprite>("Sprites/" + i.ToString());
        }

        ms = 7;
        hp = 5;
        rotateInterval = 0.2f;
        rotateTime = Time.time;

        setShotInterval(0.5f);
        setSR(transform.GetChild(sprite).GetComponent<SpriteRenderer>());
    }

    //checks the input and activate its respectective actions
    protected override void checkState()
    {
        if (boss.GetComponent<bossAction>().canMove)
        {
            if (!(Input.GetKey("w") && Input.GetKey("s")))
            {
                if (Input.GetKey("w") && ((transform.position.y + sr.bounds.size.y/2) < camHeight))
                {
                    transform.Translate(new Vector2(0, 1) * ms * Time.deltaTime);
                }
                if (Input.GetKey("s") && ((transform.position.y - sr.bounds.size.y/2) > (camHeight*-1)))
                {
                    transform.Translate(new Vector2(0, -1) * ms * Time.deltaTime);
                }
            }
            if (!(Input.GetKey("a") && Input.GetKey("d")))
            {
                if (Input.GetKey("a") && ((transform.position.x - sr.bounds.size.x/2) > (camWidth*-1)))
                {
                    transform.Translate(new Vector2(-1, 0) * ms * Time.deltaTime);
                }
                if (Input.GetKey("d") && ((transform.position.x + sr.bounds.size.x/2) < camWidth))
                {
                    transform.Translate(new Vector2(1, 0) * ms * Time.deltaTime);
                }
            }
            if (Input.GetKey("q"))
            {
                if (Time.time - rotateTime >= rotateInterval)
                {
                    transform.GetChild(sprite).Rotate(0, 0, 90);
                    rotateTime = Time.time;
                }
            }
            if (Input.GetKey("r"))
            {
                if (Time.time - rotateTime >= rotateInterval)
                {
                    transform.GetChild(sprite).Rotate(0, 0, -90);
                    rotateTime = Time.time;
                }
            }

            //TODO hitting the boss adds score
            if (Input.GetKey("space"))
            {
                if (Time.time - time >= shotInterval)
                {
                    fireShot(transform.GetChild(sprite).GetChild(projSpawn).position, transform.GetChild(sprite).rotation);
                }
            }
        }
    }

    public void takeDamage() {
        hp--;
        hpSR.sprite = hpSprites[hp > 0 ? hp - 1 : 0];
        //TODO not implementing yet for testing purposes
        if (hp <= 0)
            Debug.Log("You have died");
    }
}