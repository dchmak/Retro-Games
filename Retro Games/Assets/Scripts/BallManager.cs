/*
* Created by Daniel Mak
*/

using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class BallManager : MonoBehaviour {

    public float speed = 5f;
    [Range(1f, 5f)] public float comboMultiplier = 1.3f;
    [Range(1, 10)] public int maxCombo = 5;
    [Range(0f, 90f)] public float maxAngle = 75f;
    public Color noComboColor = Color.white;
    public Color maxComboColor = Color.red;

    private Rigidbody2D rb;
    private CircleCollider2D circle;
    private GameManager game;
    private SpriteRenderer sr;
    private Vector2 bound;
    private string lastHit;
    private int combo;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
        game = FindObjectOfType<GameManager>();
        sr = GetComponent<SpriteRenderer>();

        Vector3 dim = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        bound = new Vector2(dim.x - circle.radius, dim.y - circle.radius);
        //Debug.Log(bound);

        //rb.velocity = Random.insideUnitCircle.normalized * speed;
        rb.velocity = new Vector2(speed, 0);
    }

    private void Update() {
        float clampX = Mathf.Clamp(transform.position.x, -bound.x, bound.x);
        float clampY = Mathf.Clamp(transform.position.y, -bound.y, bound.y);
        transform.position = new Vector2(clampX, clampY);

        //Debug.Log(transform.position);
        if (transform.position.x >= bound.x || transform.position.y <= -bound.y) {
            //Debug.Log("Player 1 scores!");
            game.AddScore1();
            game.ResetBall();
        }

        if (transform.position.x <= -bound.x || transform.position.y >= bound.y) {
            //Debug.Log("Player 2 scores!");
            game.AddScore2();
            game.ResetBall();
        }

        sr.color = Color.Lerp(noComboColor, maxComboColor, (float)combo / (float)maxCombo);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == lastHit) {
            if (combo < maxCombo) {
                combo++;
                //Debug.Log(combo + " Combo!");
            }
        } else {
            combo = 0;
        }

        lastHit = collision.gameObject.tag;
        //Debug.Log(combo);

        int maxLen = 10;
        ContactPoint2D[] contact = new ContactPoint2D[maxLen];
        int len = collision.GetContacts(contact);

        if (len < maxLen) {
            VerticalPlatformManager verticalManager = collision.gameObject.GetComponent<VerticalPlatformManager>();
            HorizontalPlatformManager horizontalManager = collision.gameObject.GetComponent<HorizontalPlatformManager>();

            if (verticalManager != null) {
                float y = contact[0].point.y - collision.transform.position.y;
                float angle = Mathf.Lerp(0, maxAngle, Mathf.Abs(y) / collision.collider.bounds.size.y) * Mathf.PI / 180;

                Vector2 dir;
                if (verticalManager.index == 1) {
                    dir = new Vector2(1, Mathf.Sign(y) * Mathf.Tan(angle)).normalized;
                } else {
                    dir = new Vector2(-1, Mathf.Sign(y) * Mathf.Tan(angle)).normalized;
                }

                Debug.Log(Mathf.Pow(comboMultiplier, combo));
                rb.velocity = dir * speed * Mathf.Pow(comboMultiplier, combo);
            } else if (horizontalManager != null) {
                float x = contact[0].point.x - collision.transform.position.x;
                float angle = Mathf.Lerp(0, maxAngle, Mathf.Abs(x) / collision.collider.bounds.size.x) * Mathf.PI / 180;

                Vector2 dir;
                if (horizontalManager.index == 1) {
                    dir = new Vector2(Mathf.Sign(x) * Mathf.Tan(angle), -1).normalized;
                } else {
                    dir = new Vector2(Mathf.Sign(x) * Mathf.Tan(angle), 1).normalized;
                }

                Debug.Log(Mathf.Pow(comboMultiplier, combo));
                rb.velocity = dir * speed * Mathf.Pow(comboMultiplier, combo);
            } else Debug.LogWarning("Unknown collision!");
        } else {
            Debug.LogWarning("Too much contact points!");
        }
    }
}