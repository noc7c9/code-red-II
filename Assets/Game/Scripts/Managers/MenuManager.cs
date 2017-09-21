using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Noc7c9.TheDigitalFrontier {

    public class MenuManager : MonoBehaviour {

        public GameObject mainMenu;
        public GameObject optionsMenu;

        public Slider masterVolumeSlider;
        public Slider sfxVolumeSlider;
        public Slider musicVolumeSlider;

        void Start() {
            masterVolumeSlider.value = AudioManager.Instance.masterVolumePercent;
            sfxVolumeSlider.value = AudioManager.Instance.sfxVolumePercent;
            musicVolumeSlider.value = AudioManager.Instance.musicVolumePercent;

            Cursor.visible = true;
        }

        public void Play() {
            SceneManager.LoadScene("Game");
        }

        public void Quit() {
            Application.Quit();
        }

        public void OptionsMenu() {
            mainMenu.SetActive(false);
            optionsMenu.SetActive(true);
        }

        public void MainMenu() {
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
        }

        public void SetMasterVolume(float value) {
            AudioManager.Instance.SetVolume(value, AudioManager.AudioChannel.Master);
        }

        public void SetSfxVolume(float value) {
            AudioManager.Instance.SetVolume(value, AudioManager.AudioChannel.Sfx);
        }

        public void SetMusicVolume(float value) {
            AudioManager.Instance.SetVolume(value, AudioManager.AudioChannel.Music);
        }

    }

}
