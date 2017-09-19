using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Noc7c9.TheDigitalFrontier {

    /* Defines all UI Behaviour
     */
    public class GameUI : MonoBehaviour {

        public Image fadeScreen;
        public GameObject inGameUI;
        public GameObject gameOverUI;

        public HealthBar playerHealthBar;
        public HealthBar bossHealthBar;
        public HealthBar bossShieldHealthBar;
        public HealthBar[] subBossHealthBars;

        public float fadeTime;
        public Color fadeOutColor;

        PlayerController player;
        BossController boss;

        void Awake() {
            player = GameManager.Instance.GetPlayerController();
            player.Dying += PlayerDyingEventHandler;

            boss = GameManager.Instance.GetBossController();

            SceneManager.sceneUnloaded -= OnSceneUnloadedHandler;
            SceneManager.sceneUnloaded += OnSceneUnloadedHandler;
        }

        void Update() {
            UpdatePlayerUI();
            UpdateBossUI();
            UpdateSubBossUI();
        }

        void UpdatePlayerUI() {
            playerHealthBar.SetHealthValue(
                player == null ? 0 : player.health / player.startingHealth);
        }

        void UpdateBossUI() {
            if (boss == null) {
                bossHealthBar.Disable();
                return;
            }

            if (boss.barrierState == BossController.BarrierState.UP) {
                bossShieldHealthBar.SetHealthValue(1 - boss.GetHackPercentage());

                bossHealthBar.SetText("");
                bossShieldHealthBar.SetText("V_BARRIER.exe[1]");
            } else {
                bossShieldHealthBar.Disable();

                bossHealthBar.SetText("V_ROOT.exe[0]");
            }
            bossHealthBar.SetHealthValue(
                boss == null ? 0 : boss.health / boss.startingHealth);
        }

        void UpdateSubBossUI() {
            var subBosses = SubBossController.allSubBosses;
            for (int i = 0; i < subBosses.Length; i++) {
                var subBoss = subBosses[i];
                if (subBoss == null) {
                    subBossHealthBars[i].Disable();
                } else {
                    subBossHealthBars[i].SetHealthValue(subBoss.healthPercentage);
                    string title = "V_FORK.exe[" + (1 + subBoss.number) + "]";
                    subBossHealthBars[i].SetText(title);
                }
            }
        }

        void PlayerDyingEventHandler() {
            // trigger gameover

            Cursor.visible = true;

            StartCoroutine(Fade(Color.clear, fadeOutColor, fadeTime));

            inGameUI.SetActive(false);
            gameOverUI.SetActive(true);
        }

        IEnumerator Fade(Color from, Color to, float time) {
            float speed = 1 / time;
            float percent = 0;

            while (percent < 1) {
                percent += Time.deltaTime * speed;
                fadeScreen.color = Color.Lerp(from, to, percent);
                yield return null;
            }
        }

        void OnSceneUnloadedHandler(Scene _) {
            GameManager.sceneIsUnloading = false;
        }

        // UI input
        public void StartNewGame() {
            GameManager.sceneIsUnloading = true;
            SceneManager.LoadScene("Game");
        }

        public void ReturnToMenu() {
            GameManager.sceneIsUnloading = true;
            SceneManager.LoadScene("Menu");
        }

    }

}
