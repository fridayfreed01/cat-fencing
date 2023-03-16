using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
   public string nextScene;

    public void doExitGame() {
     Application.Quit();
     Debug.Log("Quitting game.");
 }
    public void changeScene() {
      GameObject counter = GameObject.FindWithTag("sceneCounter");
      counter.GetComponent<PrevScene>().addScene(nextScene);
      SceneManager.LoadScene(nextScene);
 }

    public void prevScene() {
      GameObject counter = GameObject.FindWithTag("sceneCounter");
      string prevScene = counter.GetComponent<PrevScene>().PreviousScene();
      SceneManager.LoadScene(prevScene);
 }
}
