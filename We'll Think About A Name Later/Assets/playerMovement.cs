using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("w")){
            gameObject.transform.Translate(new Vector2(0, 1) * 3 * Time.deltaTime);
        }
        if(Input.GetKey("a")){
            gameObject.transform.Translate(new Vector2(-1, 0) * 3 * Time.deltaTime);
        }
        if(Input.GetKey("s")){
            gameObject.transform.Translate(new Vector2(0, -1) * 3 * Time.deltaTime);
        }
        if(Input.GetKey("d")){
            gameObject.transform.Translate(new Vector2(1, 0) * 3 * Time.deltaTime);
        }
    }
}
