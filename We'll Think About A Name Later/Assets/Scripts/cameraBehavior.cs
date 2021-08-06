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
    }
    
    // Update is called once per frame
    void Update()
    {
        ms = player.GetComponent<playerBehavior>().ms;
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
    }
}
