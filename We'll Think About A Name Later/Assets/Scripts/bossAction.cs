using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossAction : MonoBehaviour
{
    //children indices
    const int projectileSpawn = 0;
    public bool canMove = false;
    int shots, maxShots, phase, totalPhases, phase1BounceCount;
    float time, camWidth, camHeight, shotGap;
    bool phaseDone, left, changeMaxShot;
    Camera cam;
    GameObject player, projectile;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        //gets the width and height of the camera view borders
        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        player = GameObject.FindGameObjectWithTag("Player");
        projectile = Resources.Load<GameObject>("Prefabs/ProjectileBoss");
        sr = gameObject.GetComponent<SpriteRenderer>();

        shotGap = 0.75f;
        phase = -1;
        totalPhases = 3;
        phaseDone = true;
        changeMaxShot = true;
        time = Time.time;

        transform.position = new Vector2(player.transform.position.x, camHeight + Mathf.Ceil(sr.bounds.size.y / 2));
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - time >= 0.05f && !canMove)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 0.01f);
            if (transform.position.y <= camHeight)
                canMove = true;
        }
        if (canMove)
        {
            /* if (phaseDone)
            {
                phase++;
                phaseDone = false;
                if (phase == 0)
                {
                    phase1BounceCount = 0;
                    left = Random.Range(0, 1) == 0;
                }
                Debug.Log(phase);
                if(phase == totalPhases) {
                    phase = -1;
                    transform.SetPositionAndRotation(new Vector2(0, camHeight), Quaternion.Euler(0, 0, 0));
                    //maybe implement a bool win to mark the end of this loop
                }
            } */
            phase = 2;
            switch (phase)
            {
                case 0:
                    phase0(5);
                    break;
                case 1:
                    determineMaxShot();
                    phase1();
                    break;
                case 2:
                    determineMaxShot();
                    phase2();
                    break;
            }
        }
    }

    // Starts at the middle, and goes randomly to left or right before traversing the other way, while shooting at the same time
    void phase0(int bossMoveSpd)
    {
        transform.Translate(new Vector2(left ? -1 : 1, 0) * bossMoveSpd * Time.deltaTime);
        if (Time.time - time >= shotGap)
        {
            fireShot(false, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, 0));
            //Instantiate(projectile, new Vector2(transform.position.x, transform.position.y - Mathf.Ceil(sr.bounds.size.y / 2) - 0.1f), transform.rotation);
            time = Time.time;
        }
        if (phase1BounceCount < 2)
        {
            if (Mathf.Abs(transform.position.x) >= camWidth)
            {
                transform.position = new Vector2(camWidth * (left ? -1 : 1) + (left ? 0.1f : -0.1f), transform.position.y);
                left = !left;
                phase1BounceCount++;
            }
        }
        else
        {
            if (Mathf.Abs(transform.position.x) <= 0.1f)
            {
                phaseDone = true;
            }
        }
    }

    //Tracks the player's position and shoot
    void phase1()
    {
        if (Time.time - time >= shotGap)
        {
            fireShot(true, new Vector2(player.transform.position.x, transform.position.y), Quaternion.Euler(0, 0, 0));
            /* transform.position = new Vector2(player.transform.position.x, transform.position.y);
            Instantiate(projectile, new Vector2(transform.position.x, transform.position.y - Mathf.Ceil(sr.bounds.size.y / 2) - 0.1f), transform.rotation); */
            shots++;
            time = Time.time;
            if(shots >= maxShots) {
                shots = 0;
                changeMaxShot = true;
                phaseDone = true;
            }
        }
    }

    //Randomly teleport around the screen, trying to hit the player through surprise 
    void phase2() {
        if(Time.time - time >= shotGap) {    
            int side = Random.Range(0, 3);
            switch(side) {
                default:
                    break;
                case 0:
                    fireShot(true, new Vector2(player.transform.position.x, camHeight), Quaternion.Euler(0, 0, 0));
                break;
                case 1:
                    fireShot(true, new Vector2(camHeight * 1.75f, player.transform.position.y), Quaternion.Euler(0, 0, 270));
                break;
                case 2:
                    fireShot(true, new Vector2(player.transform.position.x, -camHeight), Quaternion.Euler(0, 0, 180));
                break;
                case 3:
                    fireShot(true, new Vector2(-camHeight * 1.75f, player.transform.position.y), Quaternion.Euler(0, 0, 90));
                break;
            }
            shots++;
            time = Time.time;
            if(shots >= maxShots) {
                shots = 0;
                changeMaxShot = true;
                phaseDone = true;
            }
        }
    }

    //determines a random limit of number of shots for certain phases
    private void determineMaxShot() {
        if(changeMaxShot) {
            maxShots = Random.Range(5, 10);
            shots = 0;
            changeMaxShot = false;
        }
    }

    //to shot a projectile from the right angle and position relative to the boss
    //TODO: make a superclass for player and boss, and include this in it. Maybe even add OnCollisionEnter2D when player hits boss physically
    private void fireShot(bool rotate, Vector2 pos, Quaternion angle) {
        Debug.Log(pos + ", " + angle);
        if(rotate) {
            transform.SetPositionAndRotation(pos, angle);
        }
        Instantiate(projectile, transform.GetChild(projectileSpawn).position, transform.rotation);
    }
}
