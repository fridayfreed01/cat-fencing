using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    //store 0 here if level isn't open, 1 if open
    public int[] values = { 0, 0, 0, 0, 0 };

    void Start()
    {
        values[0] = 1;
    }
}
