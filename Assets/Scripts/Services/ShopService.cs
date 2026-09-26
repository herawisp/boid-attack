using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopService : MonoBehaviour {

    public static ShopService Instance { get; private set; }

    void Awake() {
        Debug.Log($"ShopService Awake on {gameObject.name}, AllPacks count at Awake: {AllPacks.Count}");
        if (Instance != null && Instance != this) {
            if (AllPacks != null && AllPacks.Count > 0 && (Instance.AllPacks == null || Instance.AllPacks.Count == 0)) {
                Destroy(Instance.gameObject);
                Instance = this;
            } else {
                Destroy(this);
            }
        } else {
            Instance = this;
        }
    }

    public List<CardPackData> AllPacks;
    public List<CardPackData> CurrentOffers;

    public event Action<List<CardPackData>> ShopOpened;
    public event Action<ButterflyType> PackOpened;

    public void OpenShop() {
        Debug.Log($"OpenShop running on {gameObject.name}, AllPacks count: {AllPacks.Count}");
        Debug.Log($"CurrentOffers is null: {CurrentOffers == null}, count: {CurrentOffers?.Count}");

        if (CurrentOffers == null || CurrentOffers.Count == 0)
            CurrentOffers = PickRandomPacks(3);

        ShopOpened?.Invoke(CurrentOffers);
    }

    List<CardPackData> PickRandomPacks(int count) {
        List<CardPackData> pool = new(AllPacks);
        List<CardPackData> result = new();

        for (int i = 0; i < count && pool.Count > 0; i++) {
            int index = UnityEngine.Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }
        return result;
    }

    public void ChoosePack(CardPackData pack) {
        Debug.Log($"ChoosePack called with {pack.PackName}");
        ButterflyType chosen = pack.PossibleButterflies[UnityEngine.Random.Range(0, pack.PossibleButterflies.Count)];
        ButterflyService.Instance.AddButterfly(chosen, TeamType.Player);
        Debug.Log($"Butterfly team {chosen}");

        PackOpened?.Invoke(chosen);
        CloseShop();
    }

    void CloseShop() {
        CurrentOffers = null;
        FloorService.Instance.ConsumeShopRoom();
    }
}