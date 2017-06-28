using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Gun shells
 */
[RequireComponent (typeof (Renderer))]
[RequireComponent (typeof (Rigidbody))]
public class Shell : MonoBehaviour {

    public float minForce;
    public float maxForce;
    public float lifeTime = 4;
    public float fadeTime = 2;

    Rigidbody myRigidbody;

    void Start() {
        myRigidbody = GetComponent<Rigidbody>();

        float force = Random.Range(minForce, maxForce);
        myRigidbody.AddForce(transform.right * force);
        myRigidbody.AddTorque(Random.insideUnitSphere * force);

        StartCoroutine(Fade());
        Destroy(gameObject, lifeTime + fadeTime);
    }

    IEnumerator Fade() {
        yield return new WaitForSeconds(lifeTime);

        float percent = 0;
        float fadeSpeed = 1 / fadeTime;
        Material mat = GetComponent<Renderer>().material;
        Color initialColor = mat.color;

        while (percent < 1) {
            percent += Time.deltaTime * fadeSpeed;
            mat.color = Color.Lerp(initialColor, Color.clear, percent);
            yield return null;
        }
    }

}
