using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Defines all UI Behaviour
 */
public class GameUI : MonoBehaviour {

    public Image fadeScreen;
    public GameObject gameOverUI;

    public float fadeTime;

    void Start() {
        FindObjectOfType<PlayerController>().OnDeath += OnGameOver;
    }

    void OnGameOver() {
        StartCoroutine(Fade(Color.clear, Color.black, fadeTime));
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
        SceneManager.LoadScene("Main");
    }

}
