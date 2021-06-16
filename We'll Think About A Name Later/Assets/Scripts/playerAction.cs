using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAction : MonoBehaviour
{
    int ms = 5, hp = 5;
    float lastRotateInput, lastShotInput, timeBetweenInputs = 0.2f, shotInterval = 0.5f;
    Sprite[] hpSprites;
    GameObject proj;
    SpriteRenderer playerSR, hpSR;
    
    // Start is called before the first frame update
    void Start()
    {
        proj = Resources.Load<GameObject>("Prefabs/playerProjectile");
        playerSR = transform.GetChild(0).GetComponent<SpriteRenderer>();
        hpSR = transform.GetChild(1).GetComponent<SpriteRenderer>();
        hpSprites = new Sprite[4];
        for(int i=0;i<4;i++){
            hpSprites[i] = Resources.Load<Sprite>("Sprites/" + i.ToString());
        }
        lastRotateInput = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
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
            if (Time.time - lastShotInput > shotInterval)
            {
                //fix rotation shooting
                //multiply the size by the rotation vector (-1 or 1) when rotated.
                Instantiate(proj, new Vector3(transform.position.x, transform.position.y + Mathf.Ceil(playerSR.bounds.size.y/2) + 0.1f, transform.position.z), transform.GetChild(0).rotation);
                lastShotInput = Time.time;
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
