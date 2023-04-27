using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//full disclosure: most of this code is from a unity forum I found online
//link: https://answers.unity.com/questions/1832889/how-to-have-different-background-music-through-dif.html
//-Sage

public class musicControl : MonoBehaviour
{
        static musicControl instance;

        // Drag in the .mp3 files here, in the editor
        public AudioClip[] MusicClips;

        public AudioSource Audio;
    

        // Singelton to keep instance alive through all scenes
        void Awake()
        {
            if (instance == null) { instance = this; }
            else { Destroy(gameObject); }

            DontDestroyOnLoad(gameObject);

            // Hooks up the 'OnSceneLoaded' method to the sceneLoaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // Called whenever a scene is loaded
        void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {

        Debug.Log("On Scene Loaded" + scene.name);
            // Plays different music in different scenes
            switch (scene.name)
            {
                case "vsPeanut":
                case "vsFluffy":
                if (Audio.clip != MusicClips[1])
                {
                    Audio.enabled = false;
                    Audio.clip = MusicClips[1];
                    Audio.enabled = true;
                }
                    break;
                default:
                if (Audio.clip != MusicClips[0])
                {
                    Audio.enabled = false;
                    Audio.clip = MusicClips[0];
                    Audio.enabled = true;
                }
                    break;
            }

        }
}
