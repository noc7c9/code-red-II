using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Crosshair dynamic behaviour.
 */
public class Crosshairs : MonoBehaviour {

    public float rotationSpeed;
    public SpriteRenderer dot;
    public LayerMask targetMask;

    public Color dotNormalColor;
    public Color dotHighlightColor;

    float rayLength = 100;

    void Start() {
        Cursor.visible = false;
        dot.color = dotNormalColor;
    }

    void Update() {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    public void DetectTargets(Ray ray) {
        if (Physics.Raycast(ray, rayLength, targetMask)) {
            dot.color = dotHighlightColor;
        } else {
            dot.color = dotNormalColor;
        }
    }

}
