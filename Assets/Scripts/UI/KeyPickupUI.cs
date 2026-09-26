using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyPickupUI : MonoBehaviour {

    public static KeyPickupUI Instance {get; private set;}

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    public GameObject RevealPanel;
    public TextMeshProUGUI RevealedLabel;
    public RectTransform KeyIcon;         // the icon inside the reveal panel
    public RectTransform KeyCornerTarget; // an empty RectTransform positioned where the HUD icon should land
    public Image HudKeyIcon;              // the permanent HUD icon, hidden until first key pickup

    public float RevealDuration = 1.5f;
    public float TweenDuration = 0.6f;

    Coroutine _sequence;

    public void ShowKeyFound() {
        if (_sequence != null) StopCoroutine(_sequence);
        _sequence = StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence() {
        RevealPanel.SetActive(true);
        RevealedLabel.text = "YOU FOUND\nA KEY!";
        KeyIcon.anchoredPosition = Vector2.zero;   // reset to its resting spot in the panel
        KeyIcon.gameObject.SetActive(true);

        yield return new WaitForSeconds(RevealDuration);

        // tween the icon from its panel position to the HUD corner
        Vector2 start = KeyIcon.position;
        Vector2 end = KeyCornerTarget.position;
        float t = 0f;

        while (t < TweenDuration) {
            t += Time.deltaTime;
            KeyIcon.position = Vector2.Lerp(start, end, t / TweenDuration);
            yield return null;
        }

        KeyIcon.gameObject.SetActive(false);
        RevealPanel.SetActive(false);
        HudKeyIcon.enabled = true;   // reveal the permanent HUD icon now that it's "arrived"
    }
}