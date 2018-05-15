/*
* Created by Daniel Mak
*/

using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class PongVerticalPlatformManager : MonoBehaviour {

    public int index;
    public float speed = 10f;

    private float bound;

    private EdgeCollider2D edge;
    
	private void Start () {
        edge = GetComponent<EdgeCollider2D>();

        Vector3 dim = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        bound = dim.y - edge.bounds.size.y / 2;
	}
	
	private void Update () {
        float y = Input.GetAxis("Vertical" + index.ToString()) * speed * Time.deltaTime;
        transform.Translate(0, y, 0);
        Vector3 clampPos = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -bound, bound), transform.position.z);
        transform.position = clampPos;
	}
}