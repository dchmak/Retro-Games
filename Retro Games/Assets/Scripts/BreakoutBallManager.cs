/*
* Created by Daniel Mak
*/

using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class BreakoutBallManager : MonoBehaviour {

    public float speed = 5f;
    [Range(0f, 90f)] public float maxAngle = 75f;

    private Rigidbody2D rb;
    private CircleCollider2D circle;
    private BreakoutGameManager game;
    private SpriteRenderer sr;
    private GameObject paddle;
    private Vector2 bound;
    private Vector3 velocity;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
        game = FindObjectOfType<BreakoutGameManager>();
        sr = GetComponent<SpriteRenderer>();
        paddle = GameObject.FindGameObjectWithTag("Paddle");

        Vector3 dim = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        bound = new Vector2(dim.x - circle.radius, dim.y - circle.radius);
        //Debug.Log(bound);
        
        rb.velocity = new Vector2(Random.Range(-1, 1), -1).normalized * speed;
    }

    private void Update() {
        float clampX = Mathf.Clamp(transform.position.x, -bound.x, bound.x);
        float clampY = Mathf.Clamp(transform.position.y, -bound.y, bound.y);
        transform.position = new Vector2(clampX, clampY);

        //Debug.Log(transform.position);
        if (Mathf.Abs(transform.position.x) >= bound.x) {
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        }

        if (transform.position.y >= bound.y) {
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
        }

        if (transform.position.y <= -bound.y) {
            game.TakeDamage();
            game.ResetBall();
        }
    }

    private void LateUpdate() {
        velocity = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        int maxLen = 10;
        ContactPoint2D[] contacts = new ContactPoint2D[maxLen];
        int len = collision.GetContacts(contacts);

        if (len < maxLen) {
            if (collision.gameObject.tag == "Paddle") {
                float x = contacts[0].point.x - collision.transform.position.x;
                float angle = Mathf.Lerp(0, maxAngle, Mathf.Abs(x) / collision.collider.bounds.size.x) * Mathf.PI / 180;

                Vector2 dir = new Vector2(Mathf.Sign(x) * Mathf.Tan(angle), 1).normalized;

                //Debug.Log(Mathf.Pow(comboMultiplier, combo));
                rb.velocity = dir * speed;
            } else if (collision.gameObject.tag == "Brick") {
                Vector3 normal = contacts[0].normal.normalized;
                rb.velocity = Vector3.Reflect(velocity, normal);

                //Destroy(collision.gameObject);
                game.RemoveBrick(collision.gameObject);
            } else Debug.LogWarning("Unknown collision!");
        } else {
            Debug.LogWarning("Too much contact points!");
        }
    }
}