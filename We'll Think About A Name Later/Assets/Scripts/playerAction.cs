using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAction : MonoBehaviour
{
<<<<<<< HEAD
    int ms = 5, hp = 4;
    GameObject proj;
    GameObject boss;
    SpriteRenderer spriteRenderer;
||||||| bab0359
    int ms = 5, hp = 4;
    GameObject proj;
    SpriteRenderer spriteRenderer;
=======
    int ms = 5, hp = 5;
>>>>>>> bad9b460e154b466b30e9b212e193a3521f0abcb
    float lastRotateInput, lastShotInput, timeBetweenInputs = 0.2f, shotInterval = 0.5f;
    Sprite[] hpSprites;
    GameObject proj;
    SpriteRenderer playerSR, hpSR;
    
    // Start is called before the first frame update
    void Start()
    {
        proj = Resources.Load<GameObject>("Prefabs/playerProjectile");
<<<<<<< HEAD
        boss = GameObject.FindGameObjectWithTag("Boss");
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
||||||| bab0359
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
=======
        playerSR = transform.GetChild(0).GetComponent<SpriteRenderer>();
        hpSR = transform.GetChild(1).GetComponent<SpriteRenderer>();
        hpSprites = new Sprite[4];
        for(int i=0;i<4;i++){
            hpSprites[i] = Resources.Load<Sprite>("Sprites/" + i.ToString());
        }
>>>>>>> bad9b460e154b466b30e9b212e193a3521f0abcb
        lastRotateInput = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        if(boss.GetComponent<bossAction>().canMove) {   
            if (Input.GetKey("w"))
            {
                transform.Translate(new Vector2(0, 1) * ms * Time.deltaTime);
            }
            else if (Input.GetKey("s"))
            {
                transform.Translate(new Vector2(0, -1) * ms * Time.deltaTime);
            }
            if (Input.GetKey("a"))
            {
                transform.Translate(new Vector2(-1, 0) * ms * Time.deltaTime);
            }
            else if (Input.GetKey("d"))
            {
                transform.Translate(new Vector2(1, 0) * ms * Time.deltaTime);
            }
||||||| bab0359
        if (Input.GetKey("w"))
        {
            transform.Translate(new Vector2(0, 1) * ms * Time.deltaTime);
        }
        else if (Input.GetKey("s"))
        {
            transform.Translate(new Vector2(0, -1) * ms * Time.deltaTime);
        }
        if (Input.GetKey("a"))
        {
            transform.Translate(new Vector2(-1, 0) * ms * Time.deltaTime);
        }
        else if (Input.GetKey("d"))
        {
            transform.Translate(new Vector2(1, 0) * ms * Time.deltaTime);
        }
=======
        if(!(Input.GetKey("w") && Input.GetKey("s"))){
            if (Input.GetKey("w"))
            {
                transform.Translate(new Vector2(0, 1) * ms * Time.deltaTime);
            }
            if (Input.GetKey("s"))
            {
                transform.Translate(new Vector2(0, -1) * ms * Time.deltaTime);
            }
        }
        if(!(Input.GetKey("a") && Input.GetKey("d"))){
            if (Input.GetKey("a"))
            {
                transform.Translate(new Vector2(-1, 0) * ms * Time.deltaTime);
            }
            if (Input.GetKey("d"))
            {
                transform.Translate(new Vector2(1, 0) * ms * Time.deltaTime);
            }
        }
>>>>>>> bad9b460e154b466b30e9b212e193a3521f0abcb

            if (Input.GetKey("r"))
            {
                if (Time.time - lastRotateInput > timeBetweenInputs)
                {
                    transform.GetChild(0).Rotate(0, 0, -90);
                    lastRotateInput = Time.time;
                }
            }

            if (Input.GetKey("space"))
            {
<<<<<<< HEAD
                if (Time.time - lastShotInput > shotInterval)
                {
                    //fix rotation shooting
                    //multiply the size by the rotation vector (-1 or 1) when rotated.
                    Instantiate(proj, new Vector3(transform.position.x, transform.position.y + Mathf.Ceil(spriteRenderer.bounds.size.y/2) + 0.1f, transform.position.z), transform.GetChild(0).rotation);
                    lastShotInput = Time.time;
                }
||||||| bab0359
                //fix rotation shooting
                //multiply the size by the rotation vector (-1 or 1) when rotated.
                Instantiate(proj, new Vector3(transform.position.x, transform.position.y + Mathf.Ceil(spriteRenderer.bounds.size.y/2) + 0.1f, transform.position.z), transform.GetChild(0).rotation);
                lastShotInput = Time.time;
=======
                //fix rotation shooting
                //multiply the size by the rotation vector (-1 or 1) when rotated.
                Instantiate(proj, new Vector3(transform.position.x, transform.position.y + Mathf.Ceil(playerSR.bounds.size.y/2) + 0.1f, transform.position.z), transform.GetChild(0).rotation);
                lastShotInput = Time.time;
>>>>>>> bad9b460e154b466b30e9b212e193a3521f0abcb
            }
        }
    }

    public void takeDamage(){
        hp--;
        hpSR.sprite = hpSprites[hp > 0 ? hp-1 : 0];
        if(hp <= 0)
            Debug.Log("You have died"); //not implementing yet for testing purposes
    }
}
