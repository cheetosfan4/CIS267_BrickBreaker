using UnityEngine;

public class ManagerSpawner : MonoBehaviour {
    public GameObject managerPrefab;
    void Awake() {
        if (GameManager.instance == null) {
            Instantiate(managerPrefab);
            GameManager.instance.debugMode();
        }
        Destroy(this.gameObject);
    }
}
