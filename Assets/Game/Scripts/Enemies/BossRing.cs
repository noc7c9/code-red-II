using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class BossRing : MonoBehaviour {

        public Transform holder;

        public Transform enemyPrefab;
        public float radius;
        public int numberOfEnemies;

        public Transform[] enemies { get; private set; }

        public event System.Action Initialized;
        protected virtual void OnInitialized() {
            var evt = Initialized;
            if (evt != null) {
                evt();
            }
        }

        void Start() {
#if UNITY_EDITOR
            holder.DestroyImmediateChildren();
#endif

            Vector3 offset = Vector3.up * radius;
            float angle = 360f / numberOfEnemies;
            Quaternion rotation = Quaternion.Euler(angle, 0, 0);
            enemies = new Transform[numberOfEnemies];
            for (int i = 0; i < numberOfEnemies; i++) {
                Transform enemy = Instantiate(enemyPrefab);
                enemy.parent = holder;
                enemy.localPosition = offset;
                enemy.localRotation = Quaternion.Euler(angle * i, 0, 0);

                enemies[i] = enemy;

                offset = rotation * offset;
            }

            OnInitialized();
        }

    }

}
