using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossAction : MonoBehaviour
{
    public int shots = 0, shotsToChange = 5;
    int phase, shotIdx = 0;
    ArrayList phaseRequirement;
    float[] shotIntervals = {1.25f, 1f, 0.75f, 0.5f, 0.25f};
    float lastShot;
    bool phaseDone = true;
    GameObject player;
    GameObject projectile;
    SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        projectile = Resources.Load<GameObject>("Prefabs/ProjectileBoss");
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        phaseRequirement = new ArrayList();
        lastShot = Time.time;
        initializePhase();
    }

    void initializePhase() {
        phaseRequirement.Add(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(phaseDone) {
            int idx = 0;
            for(int i=0; i<phaseRequirement.Count; i++) {
                if(shots <= (int)(phaseRequirement[i])) {
                    idx = i; 
                    break;
                }
            }
            phase = Random.Range(0, idx);
            Debug.Log(phase);
            phaseDone = false;
        }
        switch(phase) {
            case 0:
                phase1();
            break;
        }
    }

    void phase1() {
        if(Time.time - lastShot > shotIntervals[shotIdx]) {
            transform.position = new Vector2(player.transform.position.x, transform.position.y);
            Instantiate(projectile, new Vector3(transform.position.x, transform.position.y - Mathf.Ceil(spriteRenderer.bounds.size.y/2) - 0.1f, transform.position.z), transform.rotation);
            shots++;
            lastShot = Time.time;
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
