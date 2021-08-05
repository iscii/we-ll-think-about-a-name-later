using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraBehavior : MonoBehaviour
{
    float camWidth, camHeight, ms;
    GameObject player, background;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        background = GameObject.FindGameObjectWithTag("Background");
        camWidth = player.GetComponent<playerBehavior>().camWidth;
        camHeight = player.GetComponent<playerBehavior>().camHeight;
    }
    
    // Update is called once per frame
    void Update()
    {
        ms = player.GetComponent<playerBehavior>().ms;
        float backgroundX = background.transform.position.x, backgroundY = background.transform.position.y;
        float backgroundSRX = background.GetComponent<SpriteRenderer>().bounds.size.x / 2, backgroundSRY = background.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        float playerX = player.transform.position.x, playerY = player.transform.position.y;
        float x = transform.position.x, y = transform.position.y, z = transform.position.z;
        int modX = playerX < x ? 1 : -1;
        int modY = playerY < y ? 1 : -1;
        if(Mathf.Abs(playerX - x) >= camWidth * 3 / 4 && Mathf.Abs(playerY - y) >= camHeight * 3 / 4){
            transform.position = new Vector3(playerX + modX * camWidth * 3/4, playerY + modY * camHeight * 3/4, z);
        }
        else{
            if(Mathf.Abs(playerX - x) >= camWidth * 3 / 4) {
                transform.position = new Vector3(playerX + modX * camWidth * 3 / 4, y, z);
            }
            if(Mathf.Abs(playerY - y) >= camHeight * 3 / 4) {
                transform.position = new Vector3(x, playerY + modY * camHeight * 3 / 4, z);
            }
        }
        Debug.Log("cam: " + (x + camWidth));
        Debug.Log("background: " + (backgroundX + backgroundSRX - camWidth * 0.5f));

        if(x + camWidth >= backgroundX + backgroundSRX - camWidth * 0.25f) {
            background.transform.position = new Vector2(x - backgroundX + backgroundSRX - camWidth * 0.5f, backgroundY);
            Debug.Log("changed: " + (backgroundX - backgroundSRX + camWidth * 0.5f));
        }
        else if(x - camWidth <= backgroundX - backgroundSRX + camWidth * 0.25f) {
            background.transform.position = new Vector2(x + backgroundX - backgroundSRX + camWidth * 0.5f, backgroundY);
        }
        else if(y + camHeight >= backgroundY + backgroundSRY - camHeight * 0.25f) {
            background.transform.position = new Vector2(backgroundX, y - backgroundY + backgroundSRY - camHeight * 0.5f);
        }
        else if(y - camHeight <= backgroundY - backgroundSRY + camHeight * 0.25f) {
            background.transform.position = new Vector2(backgroundX, y + backgroundY - backgroundSRY + camHeight * 0.5f);
        }
    }
}
