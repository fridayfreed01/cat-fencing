using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCursor2 : MonoBehaviour
{
    public Transform toFollow;
    public Vector3 mDisplacement;

    void Start()
    {
      // this sets the base cursor as invisible
      //Cursor.visible = false;
    }

    void Update()
    {
        if (toFollow.position.x > Input.mousePosition.x){
			toFollow.transform.localScale = new Vector3(-1, 1, 1);
			toFollow.position = Input.mousePosition - mDisplacement;
			//Debug.Log("flip");
		}
		else { //moving left
			toFollow.transform.localScale = new Vector3(1, 1, 1);
			toFollow.position = Input.mousePosition + mDisplacement;
		}
    }
}