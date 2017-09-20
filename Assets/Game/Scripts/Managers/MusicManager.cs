using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Noc7c9.TheDigitalFrontier {

    public class MusicManager : MonoBehaviour {

        public enum MusicTrack {
            NONE,
            MENU_THEME,
            BOSS_THEME_SLOW,
            BOSS_THEME_FAST,
            END_THEME,
        }

        public MusicTrack initialTrack;

        public AudioClip menuTheme;
        public AudioClip bossThemeSlow;
        public AudioClip bossThemeFast;
        public AudioClip endTheme;

        public float crossFadeDuration;

        public bool fadeInitialTrack;

        MusicTrack currentTrack = MusicTrack.NONE;
        string sceneName;

        void Start() {
            PlayTrack(initialTrack, fadeInitialTrack);
        }

        public void PlayTrack(MusicTrack track, bool crossFade=true) {
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
            } else if (track == MusicTrack.END_THEME) {
                clip = endTheme;
            }

            if (clip != null) {
                AudioManager.Instance.PlayMusic(clip,
                        crossFade ? crossFadeDuration : 0);
            }
        }

    }

}
