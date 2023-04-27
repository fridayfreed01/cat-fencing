using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackLevels : MonoBehaviour
{
    public Button[] Buttons;
    public int numInitialized = 2;
    private int[] Levels = {0, 0, 0, 0, 0 };
    private Color yellow = new Color( 1f, 1f, 0.7882f, 1f);
    private Color grey = new Color(1f, 1f, 0.7882f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < numInitialized; i++)
        {
            if (Levels[i] == 0)
            {
                Buttons[i].color = yellow;
            }

            else
            {
                Buttons[i].color = grey;
            }
        }
    }
}
