using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossAction : MonoBehaviour
{
    public int shots = 0, shotsToChange = 5;
    public bool canMove = false;
    int phase, totalPhases = 0, shotIdx = 0;
    float[] shotIntervals = {1.25f, 1f, 0.75f, 0.5f, 0.25f};
    float time, camWidth, camHeight;
    bool phaseDone = true;
    Camera cam;
    ArrayList phaseRequirement;
    GameObject player, projectile;
    SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        //gets the radius of the width and height of the camera view borders
        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        player = GameObject.FindGameObjectWithTag("Player");
        projectile = Resources.Load<GameObject>("Prefabs/ProjectileBoss");
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        phaseRequirement = new ArrayList();
        time = Time.time;
        initializePhase();

        transform.position = new Vector2(player.transform.position.x, camHeight + Mathf.Ceil(spriteRenderer.bounds.size.y/2));
    }

    void initializePhase() {
        phaseRequirement.Add(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - time >= 0.1f && !canMove) {
            transform.position = new Vector2(transform.position.x, transform.position.y - 0.01f);
            if(transform.position.y <= camHeight)
                canMove = true;
        }
        if(canMove) {    
            if(phaseDone) {
                int idx = 0;
                for(int i=0; i<phaseRequirement.Count; i++) {
                    if(shots <= (int)(phaseRequirement[i])) {
                        idx = i; 
                        break;
                    }
                }
                phase = Random.Range(0, idx);
                phaseDone = false;
            }
            switch(phase) {
                case 0:
                    phase1();
                break;
            }
        }
    }

    void phase1() {
        if(Time.time - time > shotIntervals[shotIdx]) {
            transform.position = new Vector2(player.transform.position.x, transform.position.y);
            Instantiate(projectile, new Vector3(transform.position.x, transform.position.y - Mathf.Ceil(spriteRenderer.bounds.size.y/2) - 0.1f, transform.position.z), transform.rotation);
            shots++;
            time = Time.time;
            if(shots == shotsToChange && shotIdx < shotIntervals.Length - 1) {
                shots = 0;
                shotIdx++;
            }
            Debug.Log(shotIdx); //shotIdx goes up to 4 after a while
            if(shotIdx >= 25)
                phaseDone = true;
            
        }
    }
}
