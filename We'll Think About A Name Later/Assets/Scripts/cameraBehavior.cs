using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraBehavior : MonoBehaviour
{
    float camWidth, camHeight, ms;
    GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camWidth = player.GetComponent<playerBehavior>().camWidth;
        camHeight = player.GetComponent<playerBehavior>().camHeight;
        ms = 7;
    }
    
    // Update is called once per frame
    void Update()
    {
        float playerX = player.transform.position.x, playerY = player.transform.position.y;
        float x = transform.position.x, y = transform.position.y;
        int modX = playerX < x ? -1 : 1;
        int modY = playerY < y ? -1 : 1;
        //player @ camwidth * 3 / 4 || camheight * 3 / 4}-> move with player
        //transform.position.x = player.transform.position.x - camwidth * 3/4 //if player x is positive, subtract
        //transform.position.x = player.transform.position.x + camwidth * 3/4 //if player x is negative, add * -1 * position.x/Math.abs(position.x)
        if(Mathf.Abs(playerX) >= Mathf.Abs(x + modX * camWidth * 3 / 4))
            transform.Translate(new Vector2(modX, 0) * ms * Time.deltaTime);
            /*transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x + (camWidth * 3 / 4 * -1 * player.transform.position.x / Mathf.Abs(player.transform.position.x)), 
            transform.position.y, transform.position.z), 0.5f * Time.deltaTime);*/
        //......................Debug.Log("modY: " + modY + ", ms: " + ms);
        if(Mathf.Abs(playerY) >= Mathf.Abs(y + modY * camHeight * 3 / 4)) {
            transform.Translate(new Vector2(0, modY) * ms * Time.deltaTime);
            Debug.Log("here");
        }
        /*transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 
        player.transform.position.y + (camHeight * 3 / 4 * -1 * player.transform.position.y / Mathf.Abs(player.transform.position.y)), transform.position.z), 0.5f * Time.deltaTime);*/
    }
}
