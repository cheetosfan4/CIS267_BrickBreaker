using System.Data;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BallManager : MonoBehaviour {
    private bool started;
    private Vector2 angle;
    public float ballSpeed;
    public float ballHoldDistance;
    private Rigidbody2D rb;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        angle = Vector2.zero;
        angle.y = 1;
        started = false;
    }

    private void Start() {
    }

    void Update() {
        if (started) {
            rb.linearVelocity = angle * ballSpeed;
            rb.rotation += rb.linearVelocityX/-10;
        }
        else {
            rb.position = new Vector2(transform.parent.position.x, transform.parent.position.y + ballHoldDistance);
        }
    }

    public void setStart() {
        started = true;
        Debug.Log("started set to true");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (started) {
            Vector2 normal = collision.GetContact(0).normal;
            //reflects ball off of the borders
            angle = Vector2.Reflect(angle, normal);

            if (collision.gameObject.CompareTag("Player")) {
                PlayerManager player = collision.gameObject.GetComponent<PlayerManager>();
                float playerX = player.GetComponent<Rigidbody2D>().position.x;

                //gets a value between -1 and 1 on where the ball hit the player's bar
                float location = (rb.position.x - playerX) / (player.getPlayerWidth() / 2);

                //creates new vector2 based on the hit location
                //normalizes it, so the vector has a length of 1 before applying ballspeed
                angle = new Vector2(location, 1).normalized;
            }

            float minimumYVelocity = 0.3f;
            if (Mathf.Abs(angle.y) < minimumYVelocity) {
                //changes the y component of the vector to the minimum velocity if it is too low
                //keeps the sign of angle.y intact
                angle.y = Mathf.Sign(angle.y) * minimumYVelocity;
                //renormalizes the vector to make sure the ball doesn't move at the wrong speed
                angle = angle.normalized;
            }

            if (collision.gameObject.CompareTag("brick")) {
                BrickManager brick = collision.gameObject.GetComponent<BrickManager>();
                brick.hit();
            }
            if (collision.gameObject.CompareTag("borderbottom")) {
                GameManager.instance.lostBall();
                Destroy(this.gameObject);
            }
        }
    }
}