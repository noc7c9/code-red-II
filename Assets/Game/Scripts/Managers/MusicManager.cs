using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Noc7c9.TheDigitalFrontier {

    public class MusicManager : MonoBehaviour {

        public float crossFadeDuration;
        public AudioClip mainTheme;
        public AudioClip menuTheme;

        string sceneName;

        void Awake() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            if (scene.name != sceneName) {
                sceneName = scene.name;

                // required due to the fact that audio manager should be a singleton
                // but technically could be duplicated
                Invoke("PlayMusic", .2f);
            }
        }

        void PlayMusic() {
            AudioClip clipToPlay = null;

            if (sceneName == "Menu") {
                clipToPlay = menuTheme;
            } else if (sceneName == "Game") {
                clipToPlay = mainTheme;
            }

            if (clipToPlay != null) {
                AudioManager.Instance.PlayMusic(clipToPlay, crossFadeDuration);
            }
        }

    }

}
