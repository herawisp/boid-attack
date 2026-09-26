using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour {

    public Image Icon;
    public Color EquippedTint = new Color(0.6f, 1f, 0.6f);   // greenish
    public Color UnequippedTint = Color.white;

    Butterfly _butterfly;
    System.Action<Butterfly> _onClicked;

    public void Setup(Butterfly butterfly, System.Action<Butterfly> onClicked) {
        _butterfly = butterfly;
        _onClicked = onClicked;

        Icon.sprite = butterfly.ButterflyData.Sprite;
        Icon.color = butterfly.IsEquipped ? EquippedTint : UnequippedTint;

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => _onClicked?.Invoke(_butterfly));
    }
}