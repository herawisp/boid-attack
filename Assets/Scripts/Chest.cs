using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour {

    public Image ChestImage;
    public Sprite LockedSprite;
    public Sprite UnlockedSprite;
    public Button InteractButton;

    bool _unlocked;

    public void SetState(bool hasKey) {
        _unlocked = hasKey;
        ChestImage.sprite = hasKey ? UnlockedSprite : LockedSprite;
    }

    void OnEnable() {
        InteractButton.onClick.AddListener(OnClicked);
    }

    void OnDisable() {
        InteractButton.onClick.RemoveListener(OnClicked);
    }

    void OnClicked() {
        if (!_unlocked) {
            // TODO: "locked" feedback — shake, sound, a text prompt saying you need the key
            return;
        }

        FloorService.Instance.OpenChest();
    }
}