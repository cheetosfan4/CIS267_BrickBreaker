using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance { get; private set; }
    public GameObject livesCounter;
    private TextMeshProUGUI livesCounterText;
    public GameObject menuGUI;
    public GameObject startButton;
    private TextMeshProUGUI startButtonText;
    public GameObject exitButton;
    private TextMeshProUGUI exitButtonText;
    public GameObject gameStatus;
    private TextMeshProUGUI gameStatusText;
    public int lives;

    private bool gameLoaded;
    private bool gamePaused;
    private bool bricksLoaded;
    private int score;
    private bool died;
    private bool won;
    private float shootPreventionTimer;
    private List<GameObject> brickList;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
        }
        livesCounterText = livesCounter.GetComponentInChildren<TextMeshProUGUI>();
        startButtonText = startButton.GetComponentInChildren<TextMeshProUGUI>();
        exitButtonText = exitButton.GetComponentInChildren<TextMeshProUGUI>();
        gameStatusText = gameStatus.GetComponentInChildren<TextMeshProUGUI>();

        brickList = new List<GameObject>();

        livesCounter.SetActive(false);
        gameStatus.SetActive(false);
        gameLoaded = false;
        gamePaused = false;
        bricksLoaded = false;
        score = 0;
        died = false;
        won = false;
        lives = 3;
    }

    private void Update() {
        detectPause();
        endScreen();
        if (shootPreventionTimer > 0) {
            shootPreventionTimer -= Time.deltaTime;
        }
    }

    public void startGame() {
        if (!gameLoaded || died || won) {
            SceneManager.LoadScene("Gameplay");
            menuGUI.SetActive(false);
            livesCounter.SetActive(true);
            died = false;
            won = false;
            lives = 3;
            updateLivesCounter();
            score = 0;
            Time.timeScale = 1;
            gameLoaded = true;
            gamePaused = false;
            bricksLoaded = false;
        }
        else if (gamePaused) {
            menuGUI.SetActive(false);
            livesCounter.SetActive(true);
            gameStatus.SetActive(true);
            Time.timeScale = 1;
            shootPreventionTimer = 0.1f;
            gamePaused = false;
        }
    }

    public void exitGame() {
        if (gameLoaded) {
            SceneManager.LoadScene("MainMenu");
            menuGUI.SetActive(true);
            livesCounter.SetActive(false);
            gameStatus.SetActive(false);
            gameLoaded = false;
            bricksLoaded = false;
            startButtonText.text = "Start";
            exitButtonText.text = "Exit";
        }
        else {
            Application.Quit();
        }
    }

    public void debugMode() {
        menuGUI.SetActive(false);
        livesCounter.SetActive(true);
        gameLoaded = true;
        gamePaused = false;
        bricksLoaded = false;
        died = false;
        won = false;
        lives = 3;
        updateLivesCounter();
        score = 0;
    }

    private void detectPause() {
        if (Input.GetKeyDown(KeyCode.P) && gameLoaded && !died) {
            if (!gamePaused) {
                menuGUI.SetActive(true);
                gameStatus.SetActive(true);
                startButtonText.text = "Resume";
                exitButtonText.text = "Main Menu";
                gameStatusText.text = "PAUSED";
                Time.timeScale = 0;
                gamePaused = true;
            }
            else {
                menuGUI.SetActive(false);
                livesCounter.SetActive(true);
                Time.timeScale = 1;
                gamePaused = false;
            }
        }
    }

    private void endScreen() {
        if (died) {
            Debug.Log("died");
            menuGUI.SetActive(true);
            gameStatus.SetActive(true);
            startButtonText.text = "Retry";
            exitButtonText.text = "Main Menu";
            gameStatusText.text = "LOST";
            Time.timeScale = 0;
            gamePaused = true;
        }
        else if (won) {
            Debug.Log("won");
            menuGUI.SetActive(true);
            gameStatus.SetActive(true);
            startButtonText.text = "Retry";
            exitButtonText.text = "Main Menu";
            gameStatusText.text = "WON";
            Time.timeScale = 0;
            gamePaused = true;
        }
    }

    public void decrementLives() {
        lives--;
        if (lives <= 0) {
            died = true;
        }
        if (lives >= 0) {
            updateLivesCounter();
        }

    }

    private void loadBricks() {
        GameObject[] brickArray = GameObject.FindGameObjectsWithTag("brick");
        brickList = new List<GameObject>(brickArray);
        bricksLoaded = true;
    }

    public void removeBrick(GameObject brick) {
        if (!bricksLoaded) {
            loadBricks();
        }
        brickList.Remove(brick);
        if (brickList.Count <= 0) {
            won = true;
        }
    }

    private void updateLivesCounter() {
        livesCounterText.text = "Lives: " + lives;
    }

    public void increaseScore(int amount) {
        score += amount;
    }

    public float getShootPreventionTimer() {
        return shootPreventionTimer;
    }
}
