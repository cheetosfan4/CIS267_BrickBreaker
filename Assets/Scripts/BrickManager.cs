using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class BrickManager : MonoBehaviour {
    //public Sprite[] varieties;
    public Color[] varieties;
    public Sprite[] effects;
    public GameObject ballPrefab;
    public GameObject ghostBallPrefab;
    public GameObject shotPrefab;
    public float effectPercentChance;
    public GameObject overlay;

    private int level;
    private SpriteRenderer sr;
    private SpriteRenderer overlaySR;
    private int effect;

    void Start() {
        sr = GetComponent<SpriteRenderer>();
        overlaySR = overlay.GetComponent<SpriteRenderer>();
        level = Random.Range(0, varieties.Length);
        sr.color = varieties[level];
        effect = -1;
        chooseEffect();
    }

    private void chooseEffect() {
        int random = Random.Range(0, 100);

        if (random < effectPercentChance) {
            effect = Random.Range(0, effects.Length);
            overlaySR.sprite = effects[effect];
        }
    }

    public void hit() {
        level--;
        if (level < 0) {
            triggerEffect();
            GameManager.instance.updateScore(25);
            GameManager.instance.removeBrick(this.gameObject);
            Destroy(this.gameObject);
        }
        else {
            sr.color = varieties[level];
            GameManager.instance.updateScore(10);
        }
    }

    private void triggerEffect() {
        switch (effect) {
            //duplicate balls
            case 0: {
                    GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
                    for (int i = 0; i < balls.Length; i++) {
                        GameObject ghostBall = Instantiate(ghostBallPrefab);
                        ghostBall.transform.position = new Vector2(balls[i].transform.position.x, balls[i].transform.position.y);
                        
                        //sets the angle for the ghost ball to be the same as its original ball
                        //but, the x value is flipped so the new ball doesn't just overlap with the old one
                        //additionally makes sure that the ghost ball diverges if the original ball is going straight up
                        Vector2 newAngle = balls[i].GetComponent<BallManager>().getAngle();
                        if (Mathf.Abs(newAngle.x) < 0.2f) {
                            newAngle.x += 0.2f;
                        }
                        newAngle.x = -newAngle.x;
                        ghostBall.GetComponent<BallManager>().setAngle(newAngle);
                        ghostBall.GetComponent<BallManager>().setStart();
                    }
                    break;
                }
            //grant balls
            case 1: {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    PlayerManager playerScript = player.GetComponent<PlayerManager>();
                    //only grants the player ghost balls if they don't already have some
                    if (playerScript.ghostBallCount() <= 0) {
                        List<GameObject> balls = new List<GameObject>();
                        for (int i = 0; i < 3; i++) {
                            GameObject ghostBall = Instantiate(ghostBallPrefab);
                            //spaces the balls out along the player's bar
                            float ghostDistance = (i - 1) * (playerScript.getPlayerWidth() / 3);
                            ghostBall.transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
                            ghostBall.transform.SetParent(player.gameObject.transform);
                            ghostBall.GetComponent<BallManager>().setGhostDistance(ghostDistance);
                            balls.Add(ghostBall);
                        }
                        playerScript.addGhostList(balls);
                    }
                    break;
                }
            //fire shot
            case 2: {
                    GameObject shot = Instantiate(shotPrefab);
                    ShotManager shotScript = shot.GetComponent<ShotManager>();
                    shotScript.GetComponent<Rigidbody2D>().transform.position = new Vector2 (this.transform.position.x, this.transform.position.y);
                    shotScript.setStart();
                    Debug.Log("called setstart for shot");
                    break;
                }
        }
    }
}