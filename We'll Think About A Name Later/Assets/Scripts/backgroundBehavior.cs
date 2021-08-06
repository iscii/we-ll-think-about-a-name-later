using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class backgroundBehavior : MonoBehaviour
{
    static int id = -1;
    static List<Vector3> location = new List<Vector3>();
    GameObject player;
    SpriteRenderer backgroundSR;
    Vector2 sr, playerPos;
    Vector3 pos;
    float camWidth, camHeight;
    int[] colors = {255, 3, 255}; //green, red, blue
    int idx, mod;

    // Start is called before the first frame update
    void Start()
    {
        id++;
        backgroundSR = gameObject.GetComponent<SpriteRenderer>();
        sr = backgroundSR.bounds.size;
        player = GameObject.FindGameObjectWithTag("Player");
        camWidth = player.GetComponent<playerBehavior>().camWidth;
        camHeight = player.GetComponent<playerBehavior>().camHeight;

        pos = transform.position;
        location.Add(new Vector3(pos.x, pos.y, pos.z));

        idx = 0;
        mod = -1;

        backgroundSR.color = new Color(255, 255, 255);
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        spawnClone();
        despawnClone();
        changeColor();   
    }

    bool inRange() {
        if(playerPos.x < pos.x + sr.x / 2 && playerPos.x > pos.x - sr.x / 2 && playerPos.y < pos.y + sr.y / 2 && playerPos.y > pos.y - sr.y / 2) {
            return true;
        }

        return false;
    }

    bool checkLocation(Vector3 newPos) {
        if(location.Contains(newPos)) {
            return false;
        }

        return true;
    }

    void spawnClone() {
       if(inRange()) {    
            if(playerPos.x + camWidth >= pos.x + sr.x / 2 - camWidth * 1.25f) {
                Vector3 newPos = new Vector3(pos.x + sr.x, pos.y, pos.z);
                if(checkLocation(newPos)) {
                    GameObject clone = Instantiate(gameObject, newPos, transform.rotation);
                }
            }
            else if(playerPos.x - camWidth <= pos.x - sr.x / 2 + camWidth * 1.25f) {
                Vector3 newPos = new Vector3(pos.x - sr.x, pos.y, pos.z);
                if(checkLocation(newPos)) {
                    GameObject clone = Instantiate(gameObject, newPos, transform.rotation);
                }
            }

            if(playerPos.y + camHeight >= pos.y + sr.y / 2 - camHeight * 1.25f) {
                Vector3 newPos = new Vector3(pos.x, pos.y + sr.y, pos.z);
                if(checkLocation(newPos)) {
                    GameObject clone = Instantiate(gameObject, newPos, transform.rotation);
                }
            }
            else if(playerPos.y - camHeight <= pos.y - sr.y / 2 + camHeight * 1.25f) {
                Vector3 newPos = new Vector3(pos.x, pos.y - sr.y, pos.z);
                if(checkLocation(newPos)) {
                    GameObject clone = Instantiate(gameObject, newPos, transform.rotation);
                }
            }
        }
    }

    void despawnClone() {
        if(playerPos.x >= pos.x + sr.x * 2 || playerPos.x <= pos.x - sr.x * 2|| playerPos.y >= pos.y + sr.y * 2 || playerPos.y <= pos.y - sr.y * 2) {
            Destroy(gameObject);
            location.Remove(new Vector3(pos.x, pos.y, pos.z));
        }
    }

    void changeColor() {
        Debug.Log("here");
        backgroundSR.color = new Color(colors[1], colors[0], colors[2]);
        if(colors[idx] <= 3 || colors[idx] >= 255) {
            idx++;
            idx %= 3;
            mod = colors[idx] <= 3 ? 1 : -1;
        }
        colors[idx] = colors[idx] + mod;
    }
}
