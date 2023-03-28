using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goOnClick : MonoBehaviour
{
    public Transform toFollow;
    public float speed;

    void Start()
    {
    }

    void Update()
    {
        //cat on right side of screen
        //always update y and z values
        float oldX = toFollow.position.x;
        toFollow.position.Set(toFollow.position.x, Input.mousePosition.y, Input.mousePosition.z);

        if (Input.GetMouseButtonDown(0)) {
            //have cat move left to mouse then right
            toFollow.transform.localScale = new Vector3(1, 1, 1); //move left
            while (toFollow.position.x != Input.mousePosition.x) {
                float newX = toFollow.position.x - speed;
                Debug.Log("moving left");
                if (newX >= Input.mousePosition.x) {
                    toFollow.position.Set(newX, toFollow.position.y, toFollow.position.x);

                }
                else { 
                    toFollow.position.Set(Input.mousePosition.x, toFollow.position.y, toFollow.position.x); 
                }
            }

            toFollow.transform.localScale = new Vector3(-1, 1, 1); //move right
            while (toFollow.position.x != oldX) {
                float newX = toFollow.position.x + speed;
                Debug.Log("moving right");
                if (newX <= oldX) {
                    toFollow.position.Set(newX, toFollow.position.y, toFollow.position.x);

                }
                else { toFollow.position.Set(oldX, toFollow.position.y, toFollow.position.x);
                }
            }

        }
    }

}