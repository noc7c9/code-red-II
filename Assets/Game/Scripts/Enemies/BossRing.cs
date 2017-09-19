using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class BossRing : MonoBehaviour {

        public Transform holder;

        public Transform enemyPrefab;
        public ParticleSystem explodeEffect;

        public float radius;
        public int numberOfEnemies;

        public Transform[] enemies { get; private set; }

        public float animationSpeed;
        public float animationDelay;

        Vector3 defaultInfectedScale;

        float gapAngle {
            get {
                return 360f / numberOfEnemies;
            }
        }

        public event System.Action Initialized;
        protected virtual void OnInitialized() {
            var evt = Initialized;
            if (evt != null) {
                evt();
            }
        }

        void Awake() {
            defaultInfectedScale = enemyPrefab.localScale;
        }

        void Start() {
#if UNITY_EDITOR
            holder.DestroyImmediateChildren();
#endif

            enemies = new Transform[numberOfEnemies];
            for (int i = 0; i < numberOfEnemies; i++) {
                Transform enemy = Instantiate(enemyPrefab);
                enemy.parent = holder;
                enemy.localRotation = Quaternion.Euler(gapAngle * i, 0, 0);
                enemy.gameObject.SetActive(false);

                enemies[i] = enemy;
            }

            OnInitialized();

            InitializeAnimation();
        }

        public void InitializeAnimation() {
            Vector3 offset = Vector3.up * radius;
            Quaternion rotation = Quaternion.Euler(gapAngle, 0, 0);
            for (int i = 0; i < numberOfEnemies; i++) {
                StartCoroutine(
                        AnimateInfected(enemies[i], offset, i * animationDelay));

                offset = rotation * offset;
            }
        }

        Quaternion upRotation =
            Quaternion.FromToRotation(Vector3.forward, Vector3.up);
        public void ExplodeAnimation() {
            // stop animations if there any
            StopAllCoroutines();

            for (int i = 0; i < numberOfEnemies; i++) {
                Transform enemy = enemies[i];
                Vector3 position = enemy.position;

                // move the enemy to the center
                enemy.localPosition = Vector3.zero;
                enemy.gameObject.SetActive(false);

                // spawn the explode effect at its former position
                GameObject effect = Instantiate(explodeEffect.gameObject,
                        position, upRotation);
                Destroy(effect, explodeEffect.main.startLifetime.constant);
            }
        }

        IEnumerator AnimateInfected(Transform instance,
                Vector3 finalPosition, float initialPause) {
            yield return new WaitForSeconds(initialPause);

            Vector3 startPos = Vector3.zero;
            Vector3 endPos = finalPosition;
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = defaultInfectedScale;
            float t = 0;

            instance.localPosition = startPos;
            instance.localScale = startScale;
            instance.gameObject.SetActive(true);

            while (t <= 1) {
                t += Time.deltaTime * animationSpeed;
                instance.localPosition = Vector3.Lerp(startPos, endPos, t);
                instance.localScale = Vector3.Lerp(startScale, endScale, t);

                yield return null;
            }
        }

    }

}
