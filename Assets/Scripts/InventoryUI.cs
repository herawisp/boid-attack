using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour {

    [Header("List")]
    public Transform ListContainer;   // "List"
    public GameObject SlotPrefab;
    public Button PrevButton;
    public Button NextButton;

    [Header("Description")]
    public Image DescriptionImage;
    public TMP_Text DescriptionTitle;
    public TMP_Text DescriptionText;
    public InventoryToggle Toggle; 

    [Header("Equip")]
    public Button EquipButton;
    public TMP_Text EquipButtonLabel;

    public Button BackButton;

    const int PageSize = 6;

    List<Butterfly> _owned;
    int _page;
    Butterfly _selected;

    void OnEnable() {
        if (ButterflyService.Instance == null) {
            Debug.LogWarning("InventoryUI enabled before ButterflyService was ready.");
            return;
        }
        _owned = ButterflyService.Instance.GetButterflies(TeamType.Player);
        _page = 0;
        _selected = null;

        RenderPage();
        RenderDescription();

        PrevButton.onClick.AddListener(OnPrevPage);
        NextButton.onClick.AddListener(OnNextPage);
        EquipButton.onClick.AddListener(OnEquipClicked);
        BackButton.onClick.AddListener(OnBackClicked);
    }

    void OnDisable() {
        PrevButton.onClick.RemoveListener(OnPrevPage);
        NextButton.onClick.RemoveListener(OnNextPage);
        EquipButton.onClick.RemoveListener(OnEquipClicked);
        BackButton.onClick.RemoveListener(OnBackClicked);
    }

    void RenderPage() {
        for (int i = ListContainer.childCount - 1; i >= 0; i--)
            Destroy(ListContainer.GetChild(i).gameObject);

        int start = _page * PageSize;
        int end = Mathf.Min(start + PageSize, _owned.Count);

        for (int i = start; i < end; i++) {
            Butterfly butterfly = _owned[i];
            GameObject slotObj = Instantiate(SlotPrefab, ListContainer);
            slotObj.GetComponent<InventorySlotUI>().Setup(butterfly, OnSlotClicked);
        }

        PrevButton.interactable = _page > 0;
        NextButton.interactable = end < _owned.Count;
    }

    void OnSlotClicked(Butterfly butterfly) {
        _selected = butterfly;
        RenderPage();
        RenderDescription();
    }

    void RenderDescription() {
        bool hasSelection = _selected != null;

        DescriptionImage.enabled = hasSelection;
        DescriptionImage.sprite = hasSelection ? _selected.ButterflyData.Sprite : null;
        DescriptionTitle.text = hasSelection ? _selected.ButterflyData.DisplayName : "";
        DescriptionText.text = hasSelection ? _selected.ButterflyData.Description : "";

        EquipButton.interactable = hasSelection;
        EquipButtonLabel.text = hasSelection && _selected.IsEquipped ? "UNEQUIP" : "EQUIP";
    }

    void OnPrevPage() { _page--; RenderPage(); }
    void OnNextPage() { _page++; RenderPage(); }

    void OnEquipClicked() {
        if (_selected == null) return;

        if (!_selected.IsEquipped) {
            int equipped = ButterflyService.Instance.CountEquipped(TeamType.Player);
            if (equipped >= ButterflyService.Instance.MaxEquippedTeamSize) {
                // TODO: "team full" feedback
                return;
            }
        }

        _selected.IsEquipped = !_selected.IsEquipped;
        RenderDescription();
        RenderPage();   // refresh the equipped badge on the grid
    }

    void OnBackClicked() {
        Toggle.OnInventoryClosed();
    }
}