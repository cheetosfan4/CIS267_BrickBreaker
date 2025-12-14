//====================================================================================================
//Author        :       Marc McLennan
//Date          :       12-14-2025
//Description   :       CIS267 Homework #2; Brick Breaker
//====================================================================================================

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
    public GameObject scoreCounter;
    private TextMeshProUGUI scoreCounterText;
    public GameObject menuGUI;
    public GameObject startButton;
    private TextMeshProUGUI startButtonText;
    public GameObject exitButton;
    private TextMeshProUGUI exitButtonText;
    public GameObject gameStatus;
    private TextMeshProUGUI gameStatusText;
    public int lives;
    public GameObject title;
    public GameObject[] layouts;

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
        DontDestroyOnLoad(this.gameObject);

        livesCounterText = livesCounter.GetComponentInChildren<TextMeshProUGUI>();
        scoreCounterText = scoreCounter.GetComponentInChildren<TextMeshProUGUI>();
        startButtonText = startButton.GetComponentInChildren<TextMeshProUGUI>();
        exitButtonText = exitButton.GetComponentInChildren<TextMeshProUGUI>();
        gameStatusText = gameStatus.GetComponentInChildren<TextMeshProUGUI>();

        brickList = new List<GameObject>();

        livesCounter.SetActive(false);
        scoreCounter.SetActive(false);
        gameStatus.SetActive(false);
        title.SetActive(true);
        gameLoaded = false;
        gamePaused = false;
        bricksLoaded = false;
        score = 0;
        died = false;
        won = false;
        lives = 3;
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += onSceneLoaded;
    }
    private void OnDisable() {
        SceneManager.sceneLoaded -= onSceneLoaded;
    }
    private void onSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "Gameplay") {
            spawnLevel();
        }
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
            scoreCounter.SetActive(true);
            title.SetActive(false);
            died = false;
            won = false;
            lives = 3;
            score = 0;
            updateLivesCounter();
            updateScoreCounter();
            Time.timeScale = 1;
            gameLoaded = true;
            gamePaused = false;
            bricksLoaded = false;
        }
        else if (gamePaused) {
            menuGUI.SetActive(false);
            livesCounter.SetActive(true);
            scoreCounter.SetActive(true);
            gameStatus.SetActive(true);
            title.SetActive(false);
            Time.timeScale = 1;
            shootPreventionTimer = 0.1f;
            gamePaused = false;
        }
    }

    public void exitGame() {
        if (gameLoaded) {
            SceneManager.LoadScene("MainMenu");
            died = false;
            won = false;
            gamePaused = false;
            menuGUI.SetActive(true);
            livesCounter.SetActive(false);
            scoreCounter.SetActive(false);
            gameStatus.SetActive(false);
            title.SetActive(true);
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
        spawnLevel();
        menuGUI.SetActive(false);
        livesCounter.SetActive(true);
        scoreCounter.SetActive(true);
        title.SetActive(false);
        gameLoaded = true;
        gamePaused = false;
        bricksLoaded = false;
        died = false;
        won = false;
        lives = 3;
        score = 0;
        updateLivesCounter();
        updateScoreCounter();
    }

    private void spawnLevel() {
        //spawns random level
        int random = Random.Range(0, layouts.Length);
        GameObject layout = Instantiate(layouts[random]);
        layout.transform.position = Vector2.zero;
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
                scoreCounter.SetActive(true);
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
        updateScore(-25);
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

    public void updateScoreCounter() {
        scoreCounterText.text = "Score: " + score;
    }

    public void updateScore(int amount) {
        score += amount;
        updateScoreCounter();
    }

    public float getShootPreventionTimer() {
        return shootPreventionTimer;
    }

    public bool isPaused() {
        return gamePaused;
    }

    public bool isOver() {
        if (won || died) {
            return true;
        }
        else {
            return false;
        }
    }
}
