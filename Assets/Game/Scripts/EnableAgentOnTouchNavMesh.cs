using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Noc7c9.TheDigitalFrontier {

    [RequireComponent (typeof (UnityEngine.AI.NavMeshAgent))]
    public class EnableAgentOnTouchNavMesh : MonoBehaviour {

        public float touchRadius;
        public Vector3 offset;

        Vector3 position {
            get {
                return transform.position + offset;
            }
        }

        UnityEngine.AI.NavMeshAgent agent;

        void Awake() {
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.enabled = false;
        }

        void Update() {
            if (!agent.enabled) {
                UnityEngine.AI.NavMeshHit _;
                bool touching = UnityEngine.AI.NavMesh.SamplePosition(
                        position, out _, touchRadius, NavMesh.AllAreas);
                if (touching) {
                    agent.enabled = true;
                    this.enabled = false;
                }
            }
        }

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(position, touchRadius);
        }

    }

}
