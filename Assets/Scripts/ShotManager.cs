using UnityEngine;

public class ShotManager : MonoBehaviour {
    public float shotSpeed;
    private bool started;
    private Rigidbody2D rb;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        started = false;
    }

    void Update() {
        if (started) {
            rb.linearVelocityY = -shotSpeed;
        }
    }
    public void setStart() {
        started = true;
        Debug.Log("shot started set to true");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (started) {
            if (collision.gameObject.CompareTag("Player")) {
                Debug.Log("shot collided with player");
                GameManager.instance.decrementLives();
                Destroy(this.gameObject);
            }
            if (collision.gameObject.CompareTag("borderbottom")) {
                Debug.Log("shot collided with border");
                Destroy(this.gameObject);
            }
        }
    }
}
