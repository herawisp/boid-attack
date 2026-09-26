
using UnityEngine;
using UnityEngine.UI;

public class DungeonWall : MonoBehaviour {
    
    public Sprite OpenWallSprite;
    public Sprite ClosedWallSprite;

    Image _image;

    void Awake() {
        _image = GetComponent<Image>();
    }

    public void SetOpen(bool isOpen) {
        _image.sprite = isOpen ? OpenWallSprite : ClosedWallSprite;
    }
}
