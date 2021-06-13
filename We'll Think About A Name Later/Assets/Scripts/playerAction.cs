using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAction : MonoBehaviour
{
    int ms;
    GameObject proj;
    SpriteRenderer spriteRenderer;
    double lastRotateInput, lastShotInput, timeBetweenInputs, shotInterval;
    // Start is called before the first frame update
    void Start()
    {
        ms = 5;
        proj = Resources.Load<GameObject>("Prefabs/playerProjectile");
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        lastRotateInput = Time.time;
        timeBetweenInputs = 0.2;
        shotInterval = 0.5;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            transform.Translate(new Vector2(0, 1) * ms * Time.deltaTime);
        }
        else if (Input.GetKey("s"))
        {
            transform.Translate(new Vector2(0, -1) * ms * Time.deltaTime);
        }
        if (Input.GetKey("a"))
        {
            transform.Translate(new Vector2(-1, 0) * ms * Time.deltaTime);
        }
        else if (Input.GetKey("d"))
        {
            transform.Translate(new Vector2(1, 0) * ms * Time.deltaTime);
        }

        if (Input.GetKey("r"))
        {
            if (Time.time - lastRotateInput > timeBetweenInputs)
            {
                transform.GetChild(0).Rotate(0, 0, -90);
                lastRotateInput = Time.time;
            }
        }

        if (Input.GetKey("space"))
        {
            if (Time.time - lastShotInput > shotInterval)
            {
                //fix rotation shooting
                //multiply the size by the rotation vector (-1 or 1) when rotated.
                Instantiate(proj, new Vector3(transform.position.x, transform.position.y + Mathf.Ceil(spriteRenderer.bounds.size.y/2) + 0.1f, transform.position.z), transform.GetChild(0).rotation);
                lastShotInput = Time.time;
            }
        }
    }
}
