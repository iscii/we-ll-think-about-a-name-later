using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileBehavior : MonoBehaviour
{
    public static int shots = 0, shotsToChange = 5, playerShotSpeed = 7, bossShotSpeed = 3;
    Sprite hitSprite;
    RuntimeAnimatorController hitAnimator;
    bool isHit = false;
    // Start is called before the first frame update
    void Start()
    {
        hitSprite = Resources.Load<Sprite>("Warped Shooting Fx/Pixel Art/Hits/Hit-4/hits-4-1");
        hitAnimator = Resources.Load<RuntimeAnimatorController>("Warped Shooting Fx/Pixel Art/Hits/Hit-4/hits-4-1 (1)");
        switch(gameObject.tag){
            case "Player Projectile":
                transform.Rotate(0, 0, 90); //sprite is rotated -90
            break;
            case "Boss Projectile":
                transform.Rotate(0, 0, -90);
                shots++;
                if(shots == shotsToChange && bossShotSpeed <= 11) {
                    bossShotSpeed += 2;
                }
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(gameObject.tag) {
            case "Player Projectile":
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
    private void OnCollisionEnter2D(Collision2D other) {
        switch(other.gameObject.tag){
            case "Boss":
                //Debug.Log("Boss--");
                Destroy(gameObject);
            break; 
            //based on boss projectile
            case "Player":
                //Debug.Log("Player--");
                Destroy(gameObject);
            break;
            case "Player Projectile": 
                //case "Boss Projectile":
                /* idea: if boss projectiles collide with each other, they get buffed
                if(other.gameObject.tag == "Boss Projectile" && gameObject.tag != "Boss Projectile")
                if(other.gameObject.tag == "Player Projectile" && gameObject.tag != "Player Projectile") */

                //Debug.Log("Destroy Projectiles");
                isHit = true;
                gameObject.GetComponent<SpriteRenderer>().sprite = hitSprite;
                gameObject.GetComponent<Animator>().runtimeAnimatorController = hitAnimator;
                Destroy(gameObject.GetComponent<CapsuleCollider2D>());
                Destroy(other.gameObject);
            break;
        }
    }
    void Disappear(){ //to be used in animation event
        Debug.Log("disappear");
        Destroy(gameObject);
    }
}
