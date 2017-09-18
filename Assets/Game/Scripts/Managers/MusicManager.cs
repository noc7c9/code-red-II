using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Noc7c9.TheDigitalFrontier {

    public class MusicManager : MonoBehaviour {

        public enum MusicTrack {
            MENU_THEME,
            BOSS_THEME_SLOW,
            BOSS_THEME_FAST,
        }

        public MusicTrack initialTrack;

        public AudioClip menuTheme;
        public AudioClip bossThemeSlow;
        public AudioClip bossThemeFast;

        public float crossFadeDuration;

        MusicTrack currentTrack;
        string sceneName;

        void Start() {
            PlayTrack(initialTrack);
        }

        public void PlayTrack(MusicTrack track) {
            if (currentTrack == track) {
                return;
            }
            currentTrack = track;

            AudioClip clip = null;
            if (track == MusicTrack.MENU_THEME) {
                clip = menuTheme;
            } else if (track == MusicTrack.BOSS_THEME_SLOW) {
                clip = bossThemeSlow;
            } else if (track == MusicTrack.BOSS_THEME_FAST) {
                clip = bossThemeFast;
            }

            if (clip != null) {
                AudioManager.Instance.PlayMusic(clip, crossFadeDuration);
            }
        }

    }

}
