// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using UnityEngine.UI;

// public class TeamSelectUI : MonoBehaviour {

//     [Header("Grid")]
//     public InventorySlotUI[] Slots;      // the 6 slots, assigned in Inspector, in grid order
//     public Button PrevPageButton;
//     public Button NextPageButton;

//     [Header("Detail Panel")]
//     public Image DetailIcon;
//     public Text DetailName;
//     public Text DetailStats;
//     public Button ActionButton;
//     public Text ActionButtonLabel;

//     [Header("Nav")]
//     public Button BackButton;

//     List<ButterflyData> _owned;
//     int _page;
//     ButterflyData _selected;

//     const int PageSize = 6;

//     void OnEnable() {
//         RefreshOwnedList();
//         _page = 0;
//         _selected = null;
//         RenderPage();
//         RenderDetail();

//         PrevPageButton.onClick.AddListener(OnPrevPage);
//         NextPageButton.onClick.AddListener(OnNextPage);
//         ActionButton.onClick.AddListener(OnActionClicked);
//         BackButton.onClick.AddListener(OnBackClicked);
//     }

//     void OnDisable() {
//         PrevPageButton.onClick.RemoveListener(OnPrevPage);
//         NextPageButton.onClick.RemoveListener(OnNextPage);
//         ActionButton.onClick.RemoveListener(OnActionClicked);
//         BackButton.onClick.RemoveListener(OnBackClicked);
//     }

//     void RefreshOwnedList() {
//         _owned = ButterflyService.Instance.GetButterflies(TeamType.Player)
//             .Select(b => b.ButterflyData)
//             .Distinct()
//             .ToList();
//     }

//     void RenderPage() {
//         int start = _page * PageSize;

//         for (int i = 0; i < Slots.Length; i++) {
//             int index = start + i;
//             ButterflyData data = index < _owned.Count ? _owned[index] : null;

//             Slots[i].Setup(data, OnSlotClicked);
//             Slots[i].SetSelected(data != null && data == _selected);
//         }

//         PrevPageButton.interactable = _page > 0;
//         NextPageButton.interactable = start + PageSize < _owned.Count;
//     }

//     void OnSlotClicked(ButterflyData data) {
//         _selected = data;
//         RenderPage();     // refresh highlight state
//         RenderDetail();
//     }

//     void RenderDetail() {
//         bool hasSelection = _selected != null;

//         DetailIcon.enabled = hasSelection;
//         DetailIcon.sprite = hasSelection ? _selected.Sprite : null;
//         DetailName.text = hasSelection ? _selected.displayName : "";
//         DetailStats.text = hasSelection
//             ? $"HP {_selected.Health}   ATK {_selected.AttackDamage}   CD {_selected.Cooldown}s"
//             : "";

//         ActionButton.interactable = hasSelection;
//         ActionButtonLabel.text = "Select";   // see note below on what this button should actually do
//     }

//     void OnPrevPage() {
//         _page--;
//         RenderPage();
//     }

//     void OnNextPage() {
//         _page++;
//         RenderPage();
//     }

//     void OnActionClicked() {
//         if (_selected == null) return;
//         // TODO — see note below
//     }

//     void OnBackClicked() {
//         gameObject.SetActive(false);
//         // TODO: show whatever screen this returns to (dungeon view, main menu, etc.)
//     }
// }