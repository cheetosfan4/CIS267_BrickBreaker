using UnityEngine;

public class BrickManager : MonoBehaviour {
    //public Sprite[] varieties;
    public Color[] varieties;
    private int level;
    private SpriteRenderer sr;

    void Start() {
        sr = GetComponent<SpriteRenderer>();
        level = Random.Range(0, varieties.Length);
        sr.color = varieties[level];
    }

    public void hit() {
        level--;
        if (level < 0) {
            Destroy(this.gameObject);
        }
        else {
            sr.color = varieties[level];
        }
    }
}