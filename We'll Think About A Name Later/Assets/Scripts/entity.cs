using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entity : MonoBehaviour
{
    string projName; //might be unnecessary if we're gonna need to override fireshot completly in boss behavior
    protected const int sprite = 0, projSpawn = 0, hpbar = 1;
    protected float  lastShotTime, shotInterval;
    public float camHeight, camWidth;
    protected SpriteRenderer sr;
    protected GameObject proj;
    public entity(string projName)
    {
        this.projName = projName;
    }

    void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;

        sr = gameObject.GetComponent<SpriteRenderer>();
        proj = Resources.Load<GameObject>($"Prefabs/{projName}");
    }

    protected void fireShot()
    {
        Instantiate(proj, transform.GetChild(projSpawn).position, transform.rotation);
        lastShotTime = Time.time;
    }
}