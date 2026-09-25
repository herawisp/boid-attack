using UnityEngine;

public class Test : MonoBehaviour {

    void Start()
    {
        FloorService.Instance.GenerateFloor();
        ButterflyService.Instance.AddButterfly(ButterflyType.WoodWhite, TeamType.Player);
        ButterflyService.Instance.AddButterfly(ButterflyType.WoodWhite, TeamType.Player);
        ButterflyService.Instance.AddButterfly(ButterflyType.WoodWhite, TeamType.Player);
        ButterflyService.Instance.AddButterfly(ButterflyType.WoodWhite, TeamType.Enemy);
        ButterflyService.Instance.AddButterfly(ButterflyType.WoodWhite, TeamType.Enemy);
        ButterflyService.Instance.AddButterfly(ButterflyType.WoodWhite, TeamType.Enemy);
        ButterflyService.Instance.EnableAllButterfly();
    }
}
