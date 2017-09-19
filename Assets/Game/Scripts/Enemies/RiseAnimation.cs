using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class RiseAnimation : MonoBehaviour {

        public float targetHeight;
        public float speed;

        public void StartAnimation() {
            StartCoroutine(Animation());
        }

        public event System.Action Starting;
        protected virtual void OnStarting() {
            var evt = Starting;
            if (evt != null) {
                evt();
            }
        }

        public event System.Action Ending;
        protected virtual void OnEnding() {
            var evt = Ending;
            if (evt != null) {
                evt();
            }
        }

        IEnumerator Animation() {
            Vector3 start = transform.position;
            Vector3 end = transform.position;
            end.y = targetHeight;

            float t = 0;

            OnStarting();

            while (t <= 1) {
                t += Time.deltaTime * speed;
                transform.position = Vector3.Slerp(start, end, t);

                yield return null;
            }

            OnEnding();
        }

    }

}
