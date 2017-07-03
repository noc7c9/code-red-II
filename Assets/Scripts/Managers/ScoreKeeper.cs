using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour {

    public static int score { get; private set; }
    public int maxStreakBonus;

    float lastEnemyKillTime;
    int streakCount;
    float streakExpiryTime = 1;

    void Awake() {
        FindObjectOfType<PlayerController>().OnDeath += OnPlayerDeath;
        Enemy.OnDeathStatic += OnEnemyKilled;

        score = 0;
    }

    void OnEnemyKilled() {
        if (Time.time < lastEnemyKillTime + streakExpiryTime) {
            streakCount++;
        } else {
            streakCount = 0;
        }

        lastEnemyKillTime = Time.time;

        score += 5 + Mathf.Min(2 * streakCount, maxStreakBonus);
    }

    void OnPlayerDeath() {
        Enemy.OnDeathStatic -= OnEnemyKilled;
    }

}
