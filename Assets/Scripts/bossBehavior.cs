using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossBehavior : entity
{
    public static int nClones = 0;
    [HideInInspector] public bool canMove = false;
    protected int phase = 0;
    protected bool isClone = false;
    int shots, maxShots, shotsPerRotation, side, radius, totalPhases, phase0BounceCount;
    float posY, rotZ, p3rotAngle;
    bool phaseDone, isLeft, changeMaxShot;
    IEnumerator animRng;
    GameObject player;
    Animator bossAnimator;
    Animator faceAnimator;
    public bossBehavior() : base("bossProjectile") { }
    
    void Start()
    {
        //references
        player = GameObject.FindGameObjectWithTag("Player");
        bossAnimator = gameObject.GetComponent<Animator>();
        faceAnimator = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        startRng();

        //variable initializations
        shotsPerRotation = 8; //basically how many slices in a circle (360 / 8 = 45 degree)
        shotInterval = 0.75f;
        phaseDone = true;
        changeMaxShot = true;
        lastShotTime = Time.time;

        totalPhases = 5; //* make sure to change this totalPhases whenever we add a new phase method!!!

        //spawn boss at higher y-axis to initiate showdown
        if(isClone)
            transform.SetPositionAndRotation(new Vector2(player.transform.position.x, player.transform.position.y + camHeight * 1.75f), Quaternion.Euler(0, 0, 0));
        else {
            //transform.position = new Vector2(player.transform.position.x, player.transform.position.y + camHeight + Mathf.Ceil(sr.bounds.size.y / 2));
            transform.position = new Vector2(player.transform.position.x, player.transform.position.y + camHeight * 1.75f);
        }
    }

    //add update and methods
    void Update()
    {
        //showdown
        if (Time.time - lastShotTime >= 0.05f && !canMove && !isClone)
        {
            transform.Translate(new Vector2(0, -1) * 2 * Time.deltaTime);
            if (transform.position.y <= camHeight)
                canMove = true;
        }
        //begin phases
        if (canMove)
        {
            phaseAwaken();
            phaseInit();
        }
    }

    //IEnumerator coroutine - basically setInterval; calls code below yield every <3> seconds
    IEnumerator animationRng(){
        while(true){
            yield return new WaitForSeconds(3);
            //gen num and check
            int n = (int) Random.Range(0, 2);
            faceAnimator.SetTrigger("blink");
            switch(n){
                case 0:
                    StopCoroutine(animRng);
                    bossAnimator.SetTrigger("hoora");
                break;
            }
        }
    }

    void startRng(){
        animRng = animationRng();
        StartCoroutine(animRng);
    }

    void phaseAwaken()
    {
        if (phaseDone)
        {
            phase++;
            phaseDone = false;
            switch(phase){
                /* case 0:
                    phase0BounceCount = 0;
                    isLeft = Random.Range(0, 1) == 0;
                break; */
                case 3:
                    p3rotAngle = startAngle();
                    radius = 13;
                break;
                case 4:
                    radius = 14;
                    //sin of multiples of 90 gives 0, 1, 0, -1. Add 90 to shift the pattern to 1, 0, -1, 0 and multiply by radius to ignore switch statements
                    posY = player.transform.position.y + Mathf.RoundToInt(Mathf.Sin((side + 1) * 90 * Mathf.Deg2Rad)) * radius;
                    rotZ = side * 90;
                break;
            }
            if (phase == totalPhases)
            {
                phase = 0;
                phaseDone = true;
                transform.SetPositionAndRotation(new Vector2(player.transform.position.x, player.transform.position.y + camHeight), Quaternion.Euler(0, 0, 0));

                if (!isClone && nClones < 2)
                {
                    GameObject clone = Instantiate(gameObject, transform.position, transform.rotation);
                    clone.GetComponent<bossBehavior>().phase = nClones;
                    clone.GetComponent<bossBehavior>().isClone = true;
                    clone.GetComponent<bossBehavior>().canMove = true;
                    nClones++;
                }
            }
        }
    }

    void phaseInit()
    {
        switch (phase)
        {
            /* case 0:
                determineMaxShot(10, 15);
                phase0(7);
                break; */
            case 1:
                determineMaxShot(20, 25); //10, 15
                phase1();
                break;
            case 2:
                determineMaxShot(15, 20); //10, 15
                phase2();
                break;
            case 3:
                phase3();
                break;
            case 4:
                determineMaxShot(15, 20); //15, 20
                phase4();
                break;
        }
    }

    // Starts at the middle, and goes randomly to isLeft or right before traversing the other way, while shooting at the same time
    /* private void phase0(float bossMoveSpd)
    {
        //transform.Translate(new Vector2(isLeft ? -1 : 1, 0) * bossMoveSpd * Time.deltaTime);
        transform.Translate(new Vector2(transform.position.x < player.transform.position.x ? 1 : -1, transform.position.y - player.transform.position.y < camHeight ? 1 : 0) * bossMoveSpd * Time.deltaTime);
        if (Time.time - lastShotTime >= shotInterval)
        {
            fireShot();
            shots++;
        }
        if(shots >= maxShots) {
            finishedPhase();
            return;
        }
        /* if (phase0BounceCount < 2)
        {
            if (Mathf.Abs(transform.position.x) >= camWidth)
            {
                transform.position = new Vector2(camWidth * (isLeft ? -1 : 1) + (isLeft ? 0.1f : -0.1f), transform.position.y);
                isLeft = !isLeft;
                phase0BounceCount++;
            }
        }
        else
        {
            if (Mathf.Abs(transform.position.x) <= 0.1f)
            {
                phaseDone = true;
            }
        //}
    } */

    //Tracks the player's position and shoot
    private void phase1()
    {
        if (Time.time - lastShotTime >= shotInterval)
        {
            if (shots >= maxShots)
            {
                finishedPhase();
                return;
            }
            transform.position = new Vector2(player.transform.position.x, player.transform.position.y + camHeight);
            fireShot();
            shots++;
        }
    }

    //Randomly teleport around the screen, trying to hit the player through surprise 
    private void phase2()
    {
        if (Time.time - lastShotTime >= shotInterval)
        {
            if (shots >= maxShots)
            {
                finishedPhase();
                return;
            }

            side = Random.Range(0, 3);
            switch (side)
            {
                case 0:
                    transform.SetPositionAndRotation(new Vector2(player.transform.position.x, player.transform.position.y + camHeight), Quaternion.Euler(0, 0, 0));
                    fireShot();
                    break;
                case 1:
                    transform.SetPositionAndRotation(new Vector2(player.transform.position.x - camHeight * 1.25f, player.transform.position.y), Quaternion.Euler(0, 0, 90));
                    fireShot();
                    break;
                case 2:
                    transform.SetPositionAndRotation(new Vector2(player.transform.position.x, player.transform.position.y - camHeight), Quaternion.Euler(0, 0, 180));
                    fireShot();
                    break;
                case 3:
                    transform.SetPositionAndRotation(new Vector2(player.transform.position.x + camHeight * 1.25f, player.transform.position.y), Quaternion.Euler(0, 0, 270));
                    fireShot();
                    break;
            }
            shots++;
        }
    }

    //fully rotates around the player while shooting
    private void phase3()
    {
        if (p3rotAngle > startAngle() + 360)
        {
            finishedPhase();
            return;
        }

        //get pos around player relative to player
        Vector3 newPos = player.transform.position;
        newPos.x += radius * Mathf.Cos(p3rotAngle * Mathf.Deg2Rad); //xPos difference
        newPos.y += radius * Mathf.Sin(p3rotAngle * Mathf.Deg2Rad); //yPos difference

        transform.position = newPos;
        transform.up = -1 * (player.transform.position - transform.position); //look at player by directly modifying the "green (y) axis". dunno how it works, but it works

        //rotation angle controls speed of boss rotating around player
        p3rotAngle += 30 * Time.deltaTime; //30 default

        if (Time.time - lastShotTime >= shotInterval)
        {
            fireShot(); //TODO still gotta implement gradually increased shot speed, but idk which vars to use
            shots++;
        }
    }

    //rotates periodically around the player while shooting
    private void phase4()
    {
        if (Time.time - lastShotTime >= shotInterval)
        {
            if (shots >= maxShots)
            {
                finishedPhase();
                return;
            }

            //moduleShot resets the shot back to 0 when surpassing the shotsPerRotation, so 8 would become 0 since 8 % 8 gives 0, which resets back to the start
            int moduleShot = shots % shotsPerRotation;
            int halfShotsPerRotation = shotsPerRotation / 2;
            //neg test if it's on the isLeft of the circle or to the right
            bool neg = moduleShot < halfShotsPerRotation;
            //deltaY determines how much y-coord needs to be changed per one rotation/teleportation
            float deltaY = radius / (shotsPerRotation / 4);
            //if it's to the isLeft of the circle, decrease the y-coord, and vice-versa when it's to the right of the circle
            if (shots != 0)
            {
                if (moduleShot != 0 && moduleShot <= shotsPerRotation / 2)
                    posY = player.transform.position.y + (radius - deltaY * moduleShot);
                else
                    posY = moduleShot == 0 ? player.transform.position.y + radius : player.transform.position.y + (deltaY * (moduleShot % halfShotsPerRotation) - radius);
            }
            transform.SetPositionAndRotation(new Vector2(circleEquation(neg, posY), posY), Quaternion.Euler(0, 0, rotZ));
            fireShot();
            shots++;
            rotZ += 360 / shotsPerRotation;
        }
    }

    //determines a random limit of the number of shots for certain phases
    private void determineMaxShot(int min, int max)
    {
        if (changeMaxShot)
        {
            maxShots = Random.Range(min, max);
            shots = 0;
            changeMaxShot = false;

            if (phase == 4)
            {
                int addShotsBasedOnSide = (shotsPerRotation / 4) * side;
                shots += addShotsBasedOnSide;
                maxShots += addShotsBasedOnSide;
            }
        }
    }

    //executes after a phase is done to reset all the necessary variables back to its original values
    private void finishedPhase()
    {
        shots = 0;
        changeMaxShot = true;
        phaseDone = true;
    }

    //determines the x-position of a circle given a y-position
    private float circleEquation(bool neg, float y)
    {
        y = y - player.transform.position.y;
        //check to see if the discriminate is negative or not
        if (Mathf.Abs(y) > radius)
            return y < player.transform.position.y ? player.transform.position.y - radius : player.transform.position.y + radius;

        float x = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(y, 2));
        return neg ? -x + player.transform.position.x : x + player.transform.position.x;
    }

    //determines the starting angle base on the boss's previous location
    private int startAngle()
    {
        return side == 3 ? 0 : (side + 1) * 90;
    }

    //TODO try to find a way to simplify this while using fireShot() from entity. otherwise, there'd really be no point in having a fireshot method in entity.
    //Currently, in bossAction's fireShot(), theres:
    /* if (phase != 0)
            shots++; */
    //but we also just have shots++ after each fireShot() call in every phase besides 0. if we have that, we're just double counting - this conditional isn't necessary in fireShot()
    //so for now imma just leave it as that and inheriting the fireShot() method from entity.cs instead
}