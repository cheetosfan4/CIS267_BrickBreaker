using UnityEngine;

public class BrickManager : MonoBehaviour {
    //public Sprite[] varieties;
    public Color[] varieties;
    public Sprite[] effects;
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
            Destroy(this.gameObject);
        }
        else {
            sr.color = varieties[level];
        }
    }
}