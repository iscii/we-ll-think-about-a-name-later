using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileBehavior : MonoBehaviour
{
    static int playerShotSpeed = 9, bossShotSpeed = 9;
    bool isHit = false;
    GameObject player, boss;
    Sprite hitSprite;
    RuntimeAnimatorController hitAnimator;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boss = GameObject.FindGameObjectWithTag("Boss");
        hitSprite = Resources.Load<Sprite>("Warped Shooting Fx/Pixel Art/Hits/Hit-4/hits-4-1");
        hitAnimator = Resources.Load<RuntimeAnimatorController>("Warped Shooting Fx/Pixel Art/Hits/Hit-4/hits-4-1 (1)");

        switch(gameObject.tag){
            case "Player Projectile":
                transform.Rotate(0, 0, 90); //sprite is rotated -90
            break;
            case "Boss Projectile":
                transform.Rotate(0, 0, -90);
                /* if(boss.GetComponent<bossAction>().shots == boss.GetComponent<bossAction>().shotsToChange && bossShotSpeed <= 11) {
                    bossShotSpeed += 2;
                } */
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(gameObject.tag) { //can be revised to less lines with ternary for shotspeed
            case "Player Projectile":
                if(!isHit)
                    gameObject.transform.Translate(new Vector2(1, 0) * playerShotSpeed * Time.deltaTime);
            break;
            case "Boss Projectile":
                if(!isHit)
                    gameObject.transform.Translate(new Vector2(1, 0) * bossShotSpeed * Time.deltaTime);
            break;
        }
        //out of bounds optimization
        if(Mathf.Abs(gameObject.transform.position.x) > 30 || Mathf.Abs(gameObject.transform.position.y) > 30){
            Destroy(gameObject);
        }
    }

    //check event for collisions that this projectile hit
    private void OnCollisionEnter2D(Collision2D other) {
        //play animation
        if(other.gameObject.tag != "Boss Projectile"){
            isHit = true;
            gameObject.transform.position = other.GetContact(0).point;
            gameObject.GetComponent<SpriteRenderer>().sprite = hitSprite;
            gameObject.GetComponent<Animator>().runtimeAnimatorController = hitAnimator;
            Destroy(gameObject.GetComponent<CapsuleCollider2D>());
        }

        switch(other.gameObject.tag){
            case "Boss":
                //Debug.Log("Boss--");
            break; 
            //based on boss projectile
            case "Player":
                player.GetComponent<playerAction>().takeDamage();
            break;
            case "Player Projectile": 
                //Debug.Log("Destroy Projectiles");
                Destroy(other.gameObject);
            break;
        }
    }

    void Disappear(){ //to be used in animation event
        Destroy(gameObject);
    }
}
