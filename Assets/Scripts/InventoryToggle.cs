using UnityEngine;
using UnityEngine.UI;

public class InventoryToggle : MonoBehaviour {

    public Button OpenInventoryButton;   // the button visible during normal play
    public GameObject InventoryUIPanel;  // the InventoryUI GameObject itself

    void OnEnable() {
        OpenInventoryButton.onClick.AddListener(OnOpenClicked);
    }

    void OnDisable() {
        OpenInventoryButton.onClick.RemoveListener(OnOpenClicked);
    }

    void OnOpenClicked() {
        OpenInventoryButton.gameObject.SetActive(false);
        InventoryUIPanel.SetActive(true);
    }

    public void OnInventoryClosed() {
        InventoryUIPanel.SetActive(false);
        OpenInventoryButton.gameObject.SetActive(true);
    }
}