using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {

    public static GameOverUI Instance { get; private set; }

    void Awake() {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public GameObject Panel;
    public TextMeshProUGUI FloorReachedLabel;
    public Button RestartButton;

    void OnEnable() {
        RestartButton.onClick.AddListener(OnRestartClicked);
        Panel.SetActive(false);
    }

    void OnDisable() {
        RestartButton.onClick.RemoveListener(OnRestartClicked);
    }

    public void Show(int floorReached) {
        Panel.SetActive(true);
        FloorReachedLabel.text = $"YOU DIED\nFLOOR {floorReached}";
    }

    void OnRestartClicked() {
        Panel.SetActive(false);
        FloorService.Instance.StartNewRun();
    }
}