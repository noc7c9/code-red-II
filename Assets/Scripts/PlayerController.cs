using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Defines the player entity's behaviour.
 */
[RequireComponent (typeof (Rigidbody))]
public class PlayerController : LivingEntity {

    public float moveSpeed;

    Vector3 velocity;
    Rigidbody myRigidbody;

    void Awake() {
        myRigidbody = GetComponent<Rigidbody>();

        FindObjectOfType<Spawner>().OnNewWave += OnNewWave;
    }

    void OnNewWave(int waveNumber) {
        health = startingHealth;
    }

    protected override void Start() {
        base.Start();
    }

    void FixedUpdate() {
        myRigidbody.MovePosition(myRigidbody.position
                + velocity * Time.fixedDeltaTime);
    }

    public void Move(Vector3 direction) {
        velocity = moveSpeed * Vector3.ClampMagnitude(direction, 1f);
    }

    public void LookAt(Vector3 point) {
        // height correct the point, so that player doesn't look down
        point = new Vector3(point.x, transform.position.y, point.z);
        transform.LookAt(point);
    }

}
