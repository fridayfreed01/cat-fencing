using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrackLevels : MonoBehaviour
{
    public GameObject[] Buttons;
    public int numInitialized = 5;
    private GameObject tracker;
    private int[] Levels = { 0, 0, 0, 0, 0 };
    private Color yellow = new Color(1f, 1f, 0.7882f, 1f);
    private Color grey = new Color(1f, 1f, 0.7882f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numInitialized; i++)
        {
            Buttons[i].GetComponent<Image>().color = grey;
            Buttons[i].GetComponent<Button>().interactable = false;
        }
        Debug.Log(tracker);
    }

    void Update()
    {
        tracker = GameObject.FindWithTag("Progress");
        for (int i = 0; i < numInitialized; i++)
        {
            Levels[i] = tracker.GetComponent<Progress>().values[i];
        }
        
            for (int i = 0; i < numInitialized; i++)
            {
                if (Levels[i] == 0)
                {
                    Buttons[i].GetComponent<Image>().color = grey;
                    Buttons[i].GetComponent<Button>().interactable = false;
                }

                else
                {
                    Buttons[i].GetComponent<Image>().color = yellow;
                    Buttons[i].GetComponent<Button>().interactable = true;
                //need to add code to link combat scenes
                string next = null;
                switch (i)
                {
                    case 0:
                        next = "vsPeanut";
                        break;
                    case 1:
                        next = "vsOllie";
                        break;
                    case 2:
                        next = "vsMoses";
                        break;
                    case 3:
                        next = "vsSnowball";
                        break;
                    case 4:
                        next = "vsFluffy";
                        break;

                }
                Buttons[i].GetComponent<Buttons>().nextScene = next;
            }
            }
        
    }
        
}

