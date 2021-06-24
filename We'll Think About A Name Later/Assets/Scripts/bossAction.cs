using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossAction : MonoBehaviour
{
    //children indices
    const int projectileSpawn = 0;
    public bool canMove = false;
    int shots, maxShots, shotsPerRotation, radius, phase, totalPhases, phase1BounceCount;
    float time, camWidth, camHeight, shotGap, posY, rotZ, p4rotAngle, p4rotSpeed;
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
        shotsPerRotation = 8; //basically how many slices in a circle (360 / 8 = 45 degree)
        radius = 10;
        phase = -1;
        p4rotAngle = 90; //if we want the boss to start off at the direction in which it leaves off from the next phase, set this to the direction's angle relative to the player
        p4rotSpeed = 0.03f; //might not need this if only one reference

        //TODO make sure to change this totalPhases whenever we add a new phase method!!!
        totalPhases = 5;

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
            if (phaseDone)
            {
                //phase++;
                phase = 4;
                phaseDone = false;
                if (phase == 0)
                {
                    phase1BounceCount = 0;
                    left = Random.Range(0, 1) == 0;
                }
                if(phase == 3) {
                    radius = 13;
                }
                if(phase == 4) {
                    posY = player.transform.position.y + radius;
                    radius = 10;
                    rotZ = 0;
                }
                if(phase == totalPhases) {
                    phase = -1;
                    transform.SetPositionAndRotation(new Vector2(0, camHeight), Quaternion.Euler(0, 0, 0));
                    //maybe implement a bool win to mark the end of this loop
                }
            }
            switch (phase)
            {
                case 0:
                    phase0(5);
                    break;
                case 1:
                    determineMaxShot(7, 10);
                    phase1();
                    break;
                case 2:
                    determineMaxShot(7, 10);
                    phase2();
                    break;
                case 3:
                    determineMaxShot(7, 10);
                    phase3();
                    break;
                case 4:
                    determineMaxShot(1000, 2000);
                    phase4();
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
            fireShot();
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
            fireShot(new Vector2(player.transform.position.x, transform.position.y), Quaternion.Euler(0, 0, 0));
            if(shots >= maxShots) {
                finishedPhase();
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
                    fireShot(new Vector2(player.transform.position.x, camHeight), Quaternion.Euler(0, 0, 0));
                break;
                case 1:
                    fireShot(new Vector2(camHeight * 1.75f, player.transform.position.y), Quaternion.Euler(0, 0, 270));
                break;
                case 2:
                    fireShot(new Vector2(player.transform.position.x, -camHeight), Quaternion.Euler(0, 0, 180));
                break;
                case 3:
                    fireShot(new Vector2(-camHeight * 1.75f, player.transform.position.y), Quaternion.Euler(0, 0, 90));
                break;
            }
            if(shots >= maxShots) {
                finishedPhase();
            }
        }
    }

    //fully rotates around the player while shooting
    //TODO: first three phases transition well into each other, until phase 3. create transition for phase3. phase3 should transtition into phase4 fine
    void phase3(){
        //get pos around player relative to player
        Vector3 newPos = player.transform.position;
        newPos.x += radius * Mathf.Cos(p4rotAngle * Mathf.Deg2Rad); //xPos difference
        newPos.y += radius * Mathf.Sin(p4rotAngle * Mathf.Deg2Rad); //yPos difference

        transform.position = newPos;
        transform.up = -1 * (player.transform.position - transform.position); //look at player by directly modifying the "green (y) axis". dunno how it works, but it works

        //rotation angle controls speed of boss rotating around player
        p4rotAngle+=p4rotSpeed;

        if(Time.time - time >= shotGap) {
            fireShot(); //still gotta implement gradually increased shot speed, but idk which vars to use
        }
    }

    //rotates periodically around the player while shooting
    void phase4() {
        if(Time.time - time >= shotGap) {
            //moduleShot resets the shot back to 0 when surpassing the shotsPerRotation, so 8 would become 0 since 8 % 8 gives 0, which resets back to the start
            int moduleShot = shots % shotsPerRotation; 
            int halfShotsPerRotation = shotsPerRotation / 2;
            //neg test if it's on the left of the circle or to the right
            bool neg = moduleShot < halfShotsPerRotation;
            //deltaY determines how much y-coord needs to be changed per one rotation/teleportation
            float deltaY = radius / (shotsPerRotation / 4);
            //if it's to the left of the circle, decrease the y-coord, and vice-versa when it's to the right of the circle
            if(shots != 0) {    
                if(moduleShot != 0 && moduleShot <= shotsPerRotation / 2) {
                    posY = player.transform.position.y + (radius - deltaY * moduleShot);
                }
                else {
                    posY = moduleShot == 0 ? player.transform.position.y + radius : player.transform.position.y + (deltaY * (moduleShot % halfShotsPerRotation) - radius);
                }
            }
            fireShot(new Vector2(circleEquation(neg, posY), posY), Quaternion.Euler(0, 0, rotZ));
            rotZ += 360 / shotsPerRotation;
            if(shots >= maxShots) {
                finishedPhase();
            }
        }
    }

    //determines a random limit of number of shots for certain phases
    private void determineMaxShot(int min, int max) {
        if(changeMaxShot) {
            maxShots = Random.Range(min, max);
            shots = 0;
            changeMaxShot = false;
        }
    }

    //executes after a phase is done to reset all the necessary variables back to its original values
    private void finishedPhase() {
        shots = 0;
        changeMaxShot = true;
        phaseDone = true;
    }

    //determines the x-position of a circle given a y-position
    private float circleEquation(bool neg, float y) {
        y = y - player.transform.position.y;
        if(Mathf.Abs(y) > radius) {
            return y < player.transform.position.y ? player.transform.position.y - radius : player.transform.position.y + radius;
        }
        float x = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(y, 2));
        return neg ? -x + player.transform.position.x : x + player.transform.position.x;
    }

    //to shot a projectile from the right angle and position relative to the boss
    //TODO: make a superclass for player and boss, and include this in it. Maybe even add OnCollisionEnter2D when player hits boss physically
    private void fireShot(){
        Instantiate(projectile, transform.GetChild(projectileSpawn).position, transform.rotation);
        if(phase != 0)
            shots++;
        time = Time.time;
    }

    //overloading
    private void fireShot(Vector2 pos, Quaternion angle) {
        transform.SetPositionAndRotation(pos, angle);
        Instantiate(projectile, transform.GetChild(projectileSpawn).position, transform.rotation);
        if(phase != 0)
            shots++;
        time = Time.time;
    }
}
