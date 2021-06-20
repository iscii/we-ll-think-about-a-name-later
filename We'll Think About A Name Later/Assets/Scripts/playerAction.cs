using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAction : MonoBehaviour
{
    int ms, hp;
    float lastRotateInput, timeBetweenInputs, lastShotInput, shotInterval, camHeight, camWidth;
    Sprite[] hpSprites;
    Camera cam;
    GameObject proj, boss;
    SpriteRenderer playerSR, hpSR;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        camHeight = cam.orthographicSize; //10
        camWidth = camHeight * cam.aspect;

        proj = Resources.Load<GameObject>("Prefabs/playerProjectile");
        boss = GameObject.FindGameObjectWithTag("Boss");
        playerSR = transform.GetChild(0).GetComponent<SpriteRenderer>();
        hpSR = transform.GetChild(1).GetComponent<SpriteRenderer>();
        hpSprites = new Sprite[4];
        for (int i = 0; i < 4; i++)
        {
            hpSprites[i] = Resources.Load<Sprite>("Sprites/" + i.ToString());
        }

        ms = 5;
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
                    transform.GetChild(0).Rotate(0, 0, 90);
                    lastRotateInput = Time.time;
                }
            }
            if (Input.GetKey("r"))
            {
                if (Time.time - lastRotateInput > timeBetweenInputs)
                {
                    transform.GetChild(0).Rotate(0, 0, -90);
                    lastRotateInput = Time.time;
                }
            }

            /* if (Input.GetKey("space"))
            {
                if (Time.time - lastShotInput > shotInterval)
                {
                    //can prolly optimize this by finding a way to turn rotation into a relative position vector and multiplying the vector3 position parameeter with that.
                    switch(transform.GetChild(0).rotation.eulerAngles.z) {
                        case 0:
                            Instantiate(proj, new Vector3(transform.position.x, transform.position.y + playerSR.bounds.size.y/2 + 0.1f, transform.position.z), transform.GetChild(0).rotation);
                        break;
                        case 270:
                            Instantiate(proj, new Vector3(transform.position.x + playerSR.bounds.size.x/2 + 0.1f, transform.position.y, transform.position.z), transform.GetChild(0).rotation);
                        break;
                        case 180:
                            Instantiate(proj, new Vector3(transform.position.x, transform.position.y - playerSR.bounds.size.y/2 - 0.1f, transform.position.z), transform.GetChild(0).rotation);
                        break;
                        case 90:
                            Instantiate(proj, new Vector3(transform.position.x - playerSR.bounds.size.x/2 - 0.1f, transform.position.y, transform.position.z), transform.GetChild(0).rotation);
                        break;
                    }
                    lastShotInput = Time.time;
                }
            } */
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
