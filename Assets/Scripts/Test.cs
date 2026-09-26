using UnityEngine;

public class Test : MonoBehaviour {

    void Start()
    {
        FloorService.Instance.StartNewRun();
        // ButterflyService.Instance.AddButterfly(ButterflyType.PurpleEmperor, TeamType.Player);
        // ButterflyService.Instance.AddButterfly(ButterflyType.CommonBlue, TeamType.Player);
        // ButterflyService.Instance.AddButterfly(ButterflyType.CommonBlue, TeamType.Player);
        // ButterflyService.Instance.AddButterfly(ButterflyType.PurpleEmperor, TeamType.Enemy);
        // ButterflyService.Instance.AddButterfly(ButterflyType.CommonBlue, TeamType.Enemy);
        // ButterflyService.Instance.AddButterfly(ButterflyType.CommonBlue, TeamType.Enemy);
        ButterflyService.Instance.EnableAllButterfly();
    }
}
