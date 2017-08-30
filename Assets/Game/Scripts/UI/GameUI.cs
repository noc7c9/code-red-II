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
        public GameObject gameOverUI;

        public RectTransform newWaveBanner;
        public Text newWaveNumber;
        public Text newWaveEnemyCount;

        public RectTransform healthBar;

        public float fadeTime;
        public Color fadeOutColor;

        PlayerController player;

        void Awake() {
            player = GameManager.Instance.GetPlayerController();
            player.Dying += PlayerDyingEventHandler;
        }

        void Update() {
            float healthPercent = 0;
            if (player != null) {
                healthPercent = player.health / player.startingHealth;
            }
            healthBar.localScale = new Vector3(healthPercent, 1, 1);
        }

        IEnumerator currentNewWaveCoroutine;
        void DisplayNewWaveBanner() {
            if (currentNewWaveCoroutine != null) {
                StopCoroutine(currentNewWaveCoroutine);
            }
            StartCoroutine(currentNewWaveCoroutine = AnimateBanner());
        }

        IEnumerator AnimateBanner() {
            float visibleY = -200;
            float hiddenY = -450;
            float delay = 2f;
            float speed = 1.5f;
            float percent = 0;

            // display banner
            newWaveBanner.gameObject.SetActive(true);
            while (percent <= 1) {
                percent += Time.deltaTime * speed;

                newWaveBanner.anchoredPosition = Vector2.up
                    * Mathf.Lerp(hiddenY, visibleY, percent);
                yield return null;
            }

            // wait a moment
            yield return new WaitForSeconds(delay);

            // hide banner
            while (percent >= 0) {
                percent -= Time.deltaTime * speed;

                newWaveBanner.anchoredPosition = Vector2.up
                    * Mathf.Lerp(hiddenY, visibleY, percent);
                yield return null;
            }
            newWaveBanner.gameObject.SetActive(false);
        }

        string NumberToWord(int num) {
            string[] words = {"One", "Two", "Three", "Four", "Five"};
            return words[num-1];
        }

        void PlayerDyingEventHandler() {
            // trigger gameover

            Cursor.visible = true;

            StartCoroutine(Fade(Color.clear, fadeOutColor, fadeTime));

            healthBar.transform.parent.gameObject.SetActive(false);

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

        // UI input
        public void StartNewGame() {
            SceneManager.LoadScene("Game");
        }

        public void ReturnToMenu() {
            SceneManager.LoadScene("Menu");
        }

    }

}
