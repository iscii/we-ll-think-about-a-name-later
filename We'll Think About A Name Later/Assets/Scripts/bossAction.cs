using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossAction : MonoBehaviour
{
    public int shots = 0, shotsToChange = 5;
    public bool canMove = false;
    int phase, totalPhases = 0, shotIdx = 0, phase1BounceCount;
    float[] shotIntervals = { 2f, 1.5f, 1f, 0.75f, 0.5f };
    float time, camWidth, camHeight;
    bool phaseDone = true, left;
    Camera cam;
    ArrayList phaseRequirement;
    GameObject player, projectile;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        //gets the radius of the width and height of the camera view borders
        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        player = GameObject.FindGameObjectWithTag("Player");
        projectile = Resources.Load<GameObject>("Prefabs/ProjectileBoss");
        sr = gameObject.GetComponent<SpriteRenderer>();
        phaseRequirement = new ArrayList();
        time = Time.time;
        initializePhase();

        transform.position = new Vector2(player.transform.position.x, camHeight + Mathf.Ceil(sr.bounds.size.y / 2));
    }

    void initializePhase()
    {
        phaseRequirement.Add(0); //phase1
        phaseRequirement.Add(1); //phase2
        phaseRequirement.Add(2); //phase3
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
                /*int idx = 0;
                for (int i = 0; i < phaseRequirement.Count; i++)
                {
                    if (totalPhases < (int)(phaseRequirement[i]))
                    {
                        break;
                    }
                    idx++;
                }
                phase = Random.Range(0, idx);*/
                phase = 2;
                if (phase == 0)
                {
                    phase1BounceCount = 0;
                    left = Random.Range(0, 2) == 0;
                    Debug.Log(left);
                }
                phaseDone = false;
            }
            switch (phase)
            {
                case 0:
                    phase1(3);
                    break;
                case 1:
                    phase2();
                    break;
                case 2:
                    phase3();
                    break;
            }
        }
    }

    // Starts at the middle, and goes randomly to left or right, while shooting at the same time
    void phase1(int ms)
    {
        transform.Translate(new Vector2(left ? -1 : 1, 0) * ms * Time.deltaTime);
        if (Time.time - time >= shotIntervals[shotIdx + 1])
        {
            time = Time.time;
            Instantiate(projectile, new Vector3(transform.position.x, transform.position.y - Mathf.Ceil(sr.bounds.size.y / 2) - 0.1f, transform.position.z), transform.rotation);
        }
        if (phase1BounceCount < 2)
        {
            if (Mathf.Abs(transform.position.x) >= camWidth)
            {
                transform.position = new Vector2(camWidth * (left ? -1 : 1) + (left ? 0.1f : -0.1f), transform.position.y);
                left = !left;
                phase1BounceCount++;
                shotIdx++;
            }
        }
        else
        {
            if (Mathf.Abs(transform.position.x) <= 0.1f)
            {
                phaseDone = true;
                totalPhases++;
                shotIdx = 0;
            }
        }
    }

    /*"Tracks" the player's position and shoot, slowly increases the time that the boss changes position, it's shooting speed,
        and the gap between the shots */
    void phase2()
    {
        if (Time.time - time > shotIntervals[shotIdx])
        {
            transform.position = new Vector2(player.transform.position.x, transform.position.y);
            Instantiate(projectile, new Vector3(transform.position.x, transform.position.y - Mathf.Ceil(sr.bounds.size.y / 2) - 0.1f, transform.position.z), transform.rotation);
            shots++;
            time = Time.time;
            if (shots == shotsToChange && shotIdx < shotIntervals.Length)
            {
                shots = 0;
                shotIdx++;
                if (shotIdx == shotIntervals.Length - 1)
                {
                    shotsToChange *= 2; //makes the last subphase shoot twice the amount of shots to make it harder
                }
            }
            if (shotIdx >= shotIntervals.Length)
            {
                shotIdx = 0;
                shotsToChange /= 2;
                phaseDone = true;
                totalPhases++;
            }
        }
    }

    //Randomly teleport around the screen, trying to hit the player through surprise 
    void phase3() {
        if(Time.time - time >= shotIntervals[shotIdx + 2]) {    
            int side = Random.Range(0, 4);
            switch(side) {
                case 0:
                    transform.SetPositionAndRotation(new Vector2(player.transform.position.x, camHeight), Quaternion.Euler(0, 0, 0));
                    Instantiate(projectile, new Vector2(transform.position.x, transform.position.y - Mathf.Ceil(sr.bounds.size.y / 2) - 0.1f), transform.rotation);
                break;
                case 1:
                    transform.SetPositionAndRotation(new Vector2(camWidth, player.transform.position.y), Quaternion.Euler(0, 0, 270));
                    Instantiate(projectile, new Vector2(transform.position.x - Mathf.Ceil(sr.bounds.size.x / 2) - 0.1f, transform.position.y), transform.rotation);
                break;
                case 2:
                    transform.SetPositionAndRotation(new Vector2(player.transform.position.x, -camHeight), Quaternion.Euler(0, 0, 180));
                    Instantiate(projectile, new Vector2(transform.position.x, transform.position.y + Mathf.Ceil(sr.bounds.size.y / 2) + 0.1f), transform.rotation);
                break;
                case 3:
                    transform.SetPositionAndRotation(new Vector2(-camWidth, player.transform.position.y), Quaternion.Euler(0, 0, 90));
                    Instantiate(projectile, new Vector2(transform.position.x + Mathf.Ceil(sr.bounds.size.x / 2) + 0.1f, transform.position.y), transform.rotation);
                break;
            }
            time = Time.time;
        }
    }
}
