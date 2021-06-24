using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entity : MonoBehaviour {
    protected float camHeight, camWidth;
    protected GameObject player, projectile;
    protected SpriteRenderer sr;

    private const int projSpawn = 0;
    private Camera cam;

    //Start is called before the first fram update
    /* public void Start() {
        //gets the width and height of the camera view borders
        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        player = GameObject.FindGameObjectWithTag("Player");
        projectile = Resources.Load<GameObject>("Prefabs/playerProjectile");

    } 
    
    //sets sr to a specific sprite : boss or the player
    public void setSR(SpriteRenderer tempSR) {
        sr = tempSR;
    }
    
    private void Update() {
        checkState();
    }
    
    protected virtual void checkState() {

    }*/

    /* void fireShot(Vector3 shotpos, Quaternion angle) {
        Instantiate()
    } */

    /* public void fireShot(Vector2 pos, Quaternion angle) {

    } */
}