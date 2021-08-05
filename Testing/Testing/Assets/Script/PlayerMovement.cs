using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float mv;

    // Start is called before the first frame update
    void Start()
    {
        mv = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("d")) {
            transform.Translate(new Vector2(1, 0) * mv * Time.deltaTime);
        }  
        if(Input.GetKey("a")) {
            transform.Translate(new Vector2(-1, 0) * mv * Time.deltaTime);
        }  
        if(Input.GetKey("w")) {
            transform.Translate(new Vector2(0, 1) * mv * Time.deltaTime);
        }  
        if(Input.GetKey("s")) {
            transform.Translate(new Vector2(0, -1) * mv * Time.deltaTime);
        }  
    }
}
