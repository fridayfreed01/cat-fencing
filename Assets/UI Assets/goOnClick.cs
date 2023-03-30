using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goOnClick : MonoBehaviour
{
    public Transform toFollow;
    public float speed;
    private Vector3 targetPosition;
    private float startingX;

    void Start()
    {
        targetPosition = toFollow.position;
        startingX = toFollow.position.x;
    }

    void Update()
    {
        //cat on right side of screen

        if (Input.GetMouseButtonDown(0))
        {
            toFollow.transform.localScale = new Vector3(-1, 1, 1);
            targetPosition = Input.mousePosition;
            Debug.Log("moving to mouse");
        }
        else
        {
            //always update y and z values
            if (Mathf.Approximately(toFollow.position.x, startingX))
            {
                toFollow.transform.localScale = new Vector3(-1, 1, 1);
                targetPosition = new Vector3(startingX, Input.mousePosition.y, Input.mousePosition.z);
            }
            else if (Mathf.Approximately(toFollow.position.x, targetPosition.x))
            {
                toFollow.transform.localScale = new Vector3(1, 1, 1);
                targetPosition = new Vector3(startingX, Input.mousePosition.y, Input.mousePosition.z);
            }
        }
        toFollow.position = Vector3.MoveTowards(toFollow.position, targetPosition, 100* speed * Time.deltaTime);
    }

}