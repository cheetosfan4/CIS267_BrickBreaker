using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance { get; private set; }
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
        }
    }

    public int lives;

    public void lostBall() {
        lives--;
        if (lives <= 0) {
            Debug.Log("died");
        }
    }
}
