using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public float playerY;
    public float lives;
    public GameObject ballPrefab;

    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private Vector3 mouseScreenPosition;
    private Vector3 mousePosition;
    private Vector3 targetPosition;
    private GameObject myBall;
    private float playerWidth;
    private float gameWidth;
    private float boundaryPos;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        gameWidth = 11.72f;
        playerWidth = bc.size.x;
    }

    void Start() {
        targetPosition = new Vector3(0, playerY, 0);
        rb.position = targetPosition;
        boundaryPos = (gameWidth/2) - (playerWidth / 2);
        instantiateBall();
    }

    void Update() {
        mouseScreenPosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        if (mousePosition.x > -boundaryPos && mousePosition.x < boundaryPos) {
            targetPosition.x = mousePosition.x;
        }
        else if (mousePosition.x < 0) {
            targetPosition.x = -boundaryPos;
        }
        else if (mousePosition.x > 0) {
            targetPosition.x = boundaryPos;
        }

        rb.position = targetPosition;

        if (myBall == null && GameManager.instance.lives > 0) {
            instantiateBall();
        }

        if (Input.GetMouseButtonUp(0) && myBall != null) {
            myBall.transform.SetParent(null, true);
            myBall.GetComponent<BallManager>().setStart();
            Debug.Log("called setstart");
        }
    }

    private void instantiateBall() {
        myBall = Instantiate(ballPrefab);
        myBall.transform.position = new Vector2(rb.position.x, rb.position.y);
        myBall.transform.SetParent(this.gameObject.transform);
    }

    public float getPlayerWidth() {
        return playerWidth;
    }
}