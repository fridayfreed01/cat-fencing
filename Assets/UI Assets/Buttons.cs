using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
   public string nextScene;
    CanvasGroup canvasGroup;
    public GameObject overlay;

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

    public void openOverlay()
    {
        canvasGroup = overlay.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void closeOverlay()
    {
        canvasGroup = overlay.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
