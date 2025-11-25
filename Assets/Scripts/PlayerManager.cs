using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public float playerY;
    public float playerWidth;
    public float borderWidth;
    public GameObject ballPrefab;

    private Rigidbody2D rb;
    private Vector3 mouseScreenPosition;
    private Vector3 mousePosition;
    private Vector3 targetPosition;
    private GameObject myBall;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = new Vector3(0, playerY, 0);
        rb.position = targetPosition;
        instantiateBall();
    }

    void Update() {
        mouseScreenPosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        //12.8 is half the size of background, will not change
        //might change border width or player width
        //could also get player width from collision box when it is added
        float boundaryPos = 12.8f - borderWidth - (playerWidth / 2);

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

        if (Input.GetMouseButtonUp(0)) {
            myBall.GetComponent<BallManager>().setStart();
            Debug.Log("called setstart");
        }
    }

    private void instantiateBall() {
        myBall = Instantiate(ballPrefab);
        myBall.transform.position = new Vector2(rb.position.x, rb.position.y + 0.7f);
    }
}