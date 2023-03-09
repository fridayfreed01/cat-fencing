using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCursor : MonoBehaviour
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
        toFollow.position = Input.mousePosition + mDisplacement;

    }
}