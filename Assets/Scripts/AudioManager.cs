using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    float masterVolumePercent = 1;
    float sfxVolumePercent = 1;
    float musicVolumePercent = 1;

    AudioSource sfx2DSource;

    AudioSource[] musicSources;
    int activeMusicSourceIndex;

    Transform audioListener;
    Transform player;

    SoundLibrary library;

    void Awake() {
        if (instance_ != null) {
            Destroy(gameObject);
            return;
        }

        musicSources = new AudioSource[2];
        for (int i = 0; i < 2; i++) {
            GameObject newMusicSource = new GameObject("Music Source " + (i + 1));
            musicSources[i] = newMusicSource.AddComponent<AudioSource>();
            newMusicSource.transform.parent = transform;
        }

        GameObject newSfx2DSource = new GameObject("Sfx 2D Source");
        sfx2DSource = newSfx2DSource.AddComponent<AudioSource>();
        newSfx2DSource.transform.parent = transform;

        audioListener = FindObjectOfType<AudioListener>().transform;
        player = FindObjectOfType<PlayerController>().transform;

        library = GetComponent<SoundLibrary>();

        masterVolumePercent = PlayerPrefs.GetFloat("master volume", masterVolumePercent);
        sfxVolumePercent = PlayerPrefs.GetFloat("sfx volume", sfxVolumePercent);
        musicVolumePercent = PlayerPrefs.GetFloat("music volume", musicVolumePercent);
    }

    void Update() {
        if (player != null) {
            audioListener.position = player.position;
        }
    }

    public void SetMasterVolume(float volumePercent, AudioChannel channel) {
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

        musicSources[0].volume = musicVolumePercent * masterVolumePercent;
        musicSources[1].volume = musicVolumePercent * masterVolumePercent;
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
        float speed = 1 / duration;
        float percent = 0;
        while (percent < 1) {
            percent += Time.deltaTime * speed;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0,
                    musicVolumePercent * masterVolumePercent, percent);
            musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(
                    musicVolumePercent * masterVolumePercent, 0, percent);
            yield return null;
        }
    }

}
