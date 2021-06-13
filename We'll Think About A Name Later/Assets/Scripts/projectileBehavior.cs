using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileBehavior : MonoBehaviour
{
    int ms;
    // Start is called before the first frame update
    void Start()
    {
        switch(gameObject.tag){
            case "Player Projectile":
                transform.Rotate(0, 0, 90); //sprite is rotated -90
            break;
            case "Boss Projectile":
                transform.Rotate(0, 0, -90);
            break;
        }
        ms = 7;
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.Translate(new Vector2(1, 0) * ms * Time.deltaTime);

        //out of bounds optimization
        if(Mathf.Abs(gameObject.transform.position.x) > 30 || Mathf.Abs(gameObject.transform.position.y) > 30){
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        switch(other.gameObject.tag){
            case "Player":
                Debug.Log("Player--");
                //Destroy(gameObject);
            break;
            case "Boss":
                Debug.Log("Boss--");
                //Destroy(gameObject);
            break; 
            case "Boss Projectile":
            case "Player Projectile": 
                //idea: if boss projectiles collide with each other, they get buffed
                //if(other.gameObject.tag == "Boss Projectile" && gameObject.tag != "Boss Projectile")
                //if(other.gameObject.tag == "Player Projectile" && gameObject.tag != "Player Projectile")
                Debug.Log("Destroy Projectiles");
                //Destroy(other.gameObject);
                //Destroy(gameObject); 
            break;
        }
    }
}
