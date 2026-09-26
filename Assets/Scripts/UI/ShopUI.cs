using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour {

    public GameObject Panel;
    public Button[] PackButtons;
    public Image[] PackIcons;
    public GameObject RevealPanel;
    public TextMeshProUGUI RevealedButterflyLabel;
    public float RevealDuration = 2.5f;
    public Animator RevealedButterflyAnimator;

    void OnEnable() {
        ShopService.Instance.ShopOpened += OnShopOpened;
        ShopService.Instance.PackOpened += OnPackOpened;
        RevealedButterflyAnimator.gameObject.SetActive(false);
    }

    void OnDisable() {
        ShopService.Instance.ShopOpened -= OnShopOpened;
        ShopService.Instance.PackOpened -= OnPackOpened;
    }

    void OnShopOpened(List<CardPackData> offers) {
        Panel.SetActive(true);
        for (int i = 0; i < PackButtons.Length; i++) {
            CardPackData pack = offers[i];
            PackIcons[i].sprite = pack.Icon;

            PackButtons[i].onClick.RemoveAllListeners();
            PackButtons[i].onClick.AddListener(() => ShopService.Instance.ChoosePack(pack));
        }
    }

    void OnPackOpened(ButterflyType revealed) {
        Panel.SetActive(false);
        RevealPanel.SetActive(true);

        ButterflyData data = ButterflyService.Instance.ButterflyDatas[(int) revealed];
        RevealedButterflyAnimator.runtimeAnimatorController = data.AnimatorController;
        RevealedButterflyAnimator.gameObject.SetActive(true);

        RevealedButterflyLabel.text = $"YOU GOT A\n\n{DisplayName(revealed)}\n\nBUTTERFLY!!";

        StopAllCoroutines();   // in case a previous reveal is still mid-countdown
        StartCoroutine(HideRevealAfterDelay());
    }

    IEnumerator HideRevealAfterDelay() {
        yield return new WaitForSeconds(RevealDuration);
        RevealPanel.SetActive(false);
        RevealedButterflyAnimator.gameObject.SetActive(false);
    }

    string DisplayName(ButterflyType type) {
        return Regex.Replace(type.ToString(), "(?<!^)([A-Z])", " $1").ToUpper();
    }
}