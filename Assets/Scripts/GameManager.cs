using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance { get; private set; }
    public GameObject livesCounter;
    private TextMeshProUGUI livesCounterText;
    public int lives;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
        }

        livesCounterText = livesCounter.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void decrementLives() {
        lives--;
        if (lives <= 0) {
            Debug.Log("died");
        }
        if (lives >= 0) {
            livesCounterText.text = "Lives: " + lives;
        }
    }
}
