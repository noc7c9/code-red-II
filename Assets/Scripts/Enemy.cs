using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* Defines basic enemy behaviour.
 */
[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : MonoBehaviour {

    float pathRefreshRate = 0.25f;

    NavMeshAgent pathfinder;
    Transform target;

    void Start() {
        pathfinder = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath() {
        while (target != null) {
            Vector3 targetPosition = new Vector3(
                    target.position.x,0, target.position.z);
            pathfinder.SetDestination(targetPosition);

            yield return new WaitForSeconds(pathRefreshRate);
        }
    }

}
