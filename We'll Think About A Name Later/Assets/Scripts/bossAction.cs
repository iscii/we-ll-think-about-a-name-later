using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossAction : MonoBehaviour
{
    public int shots = 0, shotsToChange = 5;
    public bool canMove = false;
    int phase, shotIdx = 0;
    float[] shotIntervals = {1.25f, 1f, 0.75f, 0.5f, 0.25f};
    float time, camWidth, camHeight;
    bool phaseDone = true;
    ArrayList phaseRequirement;
    Camera cam;
    GameObject player;
    GameObject projectile;
    SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        //gets the radius of the height and width of the camera borders
        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        player = GameObject.FindGameObjectWithTag("Player");
        projectile = Resources.Load<GameObject>("Prefabs/ProjectileBoss");
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        time = Time.time;
        phaseRequirement = new ArrayList();
        initializePhase();

        //Sets the position of the boss outside the camera view
        transform.position = new Vector2(player.transform.position.x, Mathf.Ceil(camHeight + spriteRenderer.bounds.size.y/2f));
        Debug.Log(transform.position.y);
    }

    void initializePhase() {
        phaseRequirement.Add(0); //phase1
        phaseRequirement.Add(0); //phase2
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - time >= 3f && !canMove) {
            transform.position = new Vector2(transform.position.x, transform.position.y - 0.01f);
            if(transform.position.y <= camHeight)
                canMove = true;
        }
        if(canMove) {
            if(phaseDone) {
                int idx = 0;
                for(int i=0; i<phaseRequirement.Count; i++) {
                    if(shots < (int)(phaseRequirement[i])) {
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
                case 1:
                    phase2();
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

    void phase2() {
        
    }
}
