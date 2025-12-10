using UnityEngine;

public class MusicManager : MonoBehaviour {
    private AudioSource musicPlayer;

    private void Start() {
        musicPlayer = GetComponent<AudioSource>();
    }

    private void Update() {
        if (GameManager.instance.isPaused() || GameManager.instance.isOver()) {
            musicPlayer.volume = 0.33f;
        }
        else {
            musicPlayer.volume = 1;
        }
    }
}
