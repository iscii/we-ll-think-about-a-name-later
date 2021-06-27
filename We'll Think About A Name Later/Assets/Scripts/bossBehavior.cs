using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossBehavior : entity
{
    public bool canMove;
    int shots, maxShots, shotsPerRotation, side, radius, phase, totalPhases, phase1BounceCount;
    float posY, rotZ, p3rotAngle;
    bool phaseDone, isLeft, changeMaxShot;
    GameObject player;
    public bossBehavior() : base("bossProjectile") { }
    void Awake()
    {
        //references
        player = GameObject.FindGameObjectWithTag("Player");

        //variable initializations
        shotsPerRotation = 8; //basically how many slices in a circle (360 / 8 = 45 degree)
        shotInterval = 0.75f;
        phase = -1; //will get incremented before first phase
        totalPhases = 5;
        radius = 10;
        canMove = false;
        phaseDone = true;
        changeMaxShot = true;
        lastShotTime = Time.time;

        //initial position "showdown"
        transform.position = new Vector2(player.transform.position.x, camHeight + Mathf.Ceil(sr.bounds.size.y / 2));
    }

    void Update()
    {

    }

    //add update and methods

}