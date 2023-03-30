using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrevScene : MonoBehaviour
{

    private List<string> sceneHistory = new List<string>(); //list of scenes

    // Start is called before the first frame update
    void Start()
    {
    DontDestroyOnLoad(this.gameObject);  //Allow this object to persist between scene changes
        sceneHistory.Add("GameStart");
    }

    public void addScene(string sceneName) {
        sceneHistory.Add(sceneName);
    }

    public string PreviousScene() //this method was adapted from one found online
    {
        sceneHistory.RemoveAt(sceneHistory.Count -1);
        string toReturn = (sceneHistory[sceneHistory.Count -1]);
        return toReturn;
    }
    
}
  
