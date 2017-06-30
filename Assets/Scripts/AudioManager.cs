using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    static AudioManager instance_;
    public static AudioManager Instance {
        get {
            if (instance_ == null) {
                instance_ = FindObjectOfType<AudioManager>();
            }
            return instance_;
        }
    }

    float masterVolumePercent = 1;
    float sfxVolumePercent = 1;
    float musicVolumePercent = 1;

    AudioSource[] musicSources;
    int activeMusicSourceIndex;

    Transform audioListener;
    Transform player;

    void Awake() {
        musicSources = new AudioSource[2];
        for (int i = 0; i < 2; i++) {
            GameObject newMusicSource = new GameObject("Music Source " + (i + 1));
            musicSources[i] = newMusicSource.AddComponent<AudioSource>();
            newMusicSource.transform.parent = transform;
        }

        audioListener = FindObjectOfType<AudioListener>().transform;
        player = FindObjectOfType<PlayerController>().transform;
    }

    void Update() {
        if (player != null) {
            audioListener.position = player.position;
        }
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
