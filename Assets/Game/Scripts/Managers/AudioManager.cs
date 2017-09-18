using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Noc7c9.TheDigitalFrontier {

    public class AudioManager : MonoBehaviour {

        static AudioManager instance_;
        public static AudioManager Instance {
            get {
                if (instance_ == null) {
                    instance_ = FindObjectOfType<AudioManager>();
                    DontDestroyOnLoad(instance_);
                }
                return instance_;
            }
        }

        public enum AudioChannel {Master, Sfx, Music};

        public float masterVolumePercent { get; private set; }
        public float sfxVolumePercent { get; private set; }
        public float musicVolumePercent { get; private set; }

        AudioSource sfx2DSource;

        AudioSource[] musicSources;
        int activeMusicSourceIndex;

        SoundLibrary library;

        void Awake() {
            if (instance_ != null) {
                Destroy(gameObject);
                return;
            }

            musicSources = new AudioSource[2];
            for (int i = 0; i < 2; i++) {
                GameObject newMusicSource = new GameObject("Music Source " + (i + 1));
                newMusicSource.transform.parent = transform;

                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                musicSources[i].loop = true;
            }

            GameObject newSfx2DSource = new GameObject("Sfx 2D Source");
            newSfx2DSource.transform.parent = transform;

            sfx2DSource = newSfx2DSource.AddComponent<AudioSource>();

            library = GetComponent<SoundLibrary>();

            masterVolumePercent = PlayerPrefs.GetFloat("master volume", 1);
            sfxVolumePercent = PlayerPrefs.GetFloat("sfx volume", 1);
            musicVolumePercent = PlayerPrefs.GetFloat("music volume", 1);
        }

        public void SetVolume(float volumePercent, AudioChannel channel) {
            switch (channel) {
                case AudioChannel.Master:
                    masterVolumePercent = volumePercent;
                    PlayerPrefs.SetFloat("master volume", volumePercent);
                    break;
                case AudioChannel.Sfx:
                    sfxVolumePercent = volumePercent;
                    PlayerPrefs.SetFloat("sfx volume", volumePercent);
                    break;
                case AudioChannel.Music:
                    musicVolumePercent = volumePercent;
                    PlayerPrefs.SetFloat("music volume", volumePercent);
                    break;
            }
            PlayerPrefs.Save();

            musicSources[activeMusicSourceIndex].volume
                = musicVolumePercent * masterVolumePercent;
        }

        public void PlayMusic(AudioClip clip, float fadeDuration=1) {
            activeMusicSourceIndex = 1 - activeMusicSourceIndex;
            musicSources[activeMusicSourceIndex].clip = clip;
            musicSources[activeMusicSourceIndex].Play();

            StartCoroutine(MusicCrossFade(fadeDuration));
        }

        public void PlaySound(AudioClip clip, Vector3 position) {
            if (clip != null) {
                AudioSource.PlayClipAtPoint(clip, position,
                        sfxVolumePercent * masterVolumePercent);
            }
        }

        public void PlaySound(string soundName, Vector3 position) {
            PlaySound(library.GetClipFromName(soundName), position);
        }

        public void PlaySound2D(string soundName) {
            sfx2DSource.PlayOneShot(library.GetClipFromName(soundName),
                    sfxVolumePercent * masterVolumePercent);
        }

        IEnumerator MusicCrossFade(float duration) {
            if (duration <= 0) {
                musicSources[activeMusicSourceIndex].volume =
                        musicVolumePercent * masterVolumePercent;
                musicSources[1 - activeMusicSourceIndex].volume = 0;
            } else {
                float speed = 1 / duration;
                float percent = 0;
                while (percent < 1) {
                    percent += Time.deltaTime * speed;
                    musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(
                            0, musicVolumePercent * masterVolumePercent, percent);
                    musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(
                            musicVolumePercent * masterVolumePercent, 0, percent);
                    yield return null;
                }
            }
        }

    }

}
