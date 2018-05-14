/*
* Created by Daniel Mak
*/

using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class HorizontalPlatformManager : MonoBehaviour {

    public int index;
    public float speed = 10f;

    private float bound;

    private EdgeCollider2D edge;

    private void Start() {
        edge = GetComponent<EdgeCollider2D>();

        Vector3 dim = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        bound = dim.x - edge.bounds.size.x / 2;
    }

    private void Update() {
        float x = Input.GetAxis("Horizontal" + index.ToString()) * speed * Time.deltaTime;
        transform.Translate(x, 0, 0);
        Vector3 clampPos = new Vector3(Mathf.Clamp(transform.position.x, -bound, bound), transform.position.y, transform.position.z);
        transform.position = clampPos;
    }
}