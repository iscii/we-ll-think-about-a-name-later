using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entity : MonoBehaviour {
    //variables to be shared with subclasses
    protected const int projSpawn = 0;
    protected float camHeight, camWidth, time, shotInterval;
    protected GameObject player, projectile;
    protected SpriteRenderer sr;

    //personal variables
    Camera cam;

    // Start is called before the first frame update
    void Start() {
        //gets the width and height of the camera view borders
        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        player = GameObject.FindGameObjectWithTag("Player");
        projectile = Resources.Load<GameObject>("Prefabs/playerProjectile");

        time = Time.time;
    }

    //sets shotInterval based on whether it's the boss or the player
    protected virtual void setShotInterval(float tempShotInterval) {
        shotInterval = tempShotInterval;
    }
    
    //sets sr to a specific sprite : either the boss or the player
    protected virtual void setSR(SpriteRenderer tempSR) {
        sr = tempSR;
    }
    
    // Update is called once per frame
    private void Update() {
        checkState();
    }
    
    //Child classes will override this method to fulfill their intented purposes
    protected virtual void checkState() {

    }

    //fireShot if you don't want to change position of the sprite
    protected virtual void fireShot(Vector3 shotPos, Quaternion shotAngle) {
        Instantiate(projectile, shotPos, shotAngle);
        time = Time.time;
    }

    //fireShot if you want to change the position of the sprite before firing
    protected virtual void fireShot(Vector2 pos, Quaternion angle, Vector3 shotPos, Quaternion shotAngle) {
        transform.SetPositionAndRotation(pos, angle);
        Instantiate(projectile, shotPos, shotAngle);
        time = Time.time;
    }
}