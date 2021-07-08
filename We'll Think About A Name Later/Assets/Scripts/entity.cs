using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entity : MonoBehaviour
{
    string projName; //might be unnecessary if we're gonna need to override fireshot completly in boss behavior
    protected const int sprite = 0, projSpawn = 0, hpbar = 1;
    protected float camHeight, camWidth, lastShotTime, shotInterval;
    Camera cam;
    protected SpriteRenderer sr;
    protected GameObject proj;
    public entity(string projName)
    {
        this.projName = projName;
    }

    void Awake()
    {
        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        sr = gameObject.GetComponent<SpriteRenderer>();
        proj = Resources.Load<GameObject>($"Prefabs/{projName}");
    }

    protected void fireShot()
    {
        Instantiate(proj, transform.GetChild(projSpawn).position, transform.rotation);
        lastShotTime = Time.time;
    }
}