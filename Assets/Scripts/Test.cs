using UnityEngine;

public class Test : MonoBehaviour {

    void Start()
    {
        ButterflyService.Instance.AddButterfly(ButterflyType.AdonisBlue, TeamType.Player);
        ButterflyService.Instance.AddButterfly(ButterflyType.AdonisBlue, TeamType.Player);
        ButterflyService.Instance.AddButterfly(ButterflyType.AdonisBlue, TeamType.Player);
        ButterflyService.Instance.AddButterfly(ButterflyType.AdonisBlue, TeamType.Enemy);
        ButterflyService.Instance.AddButterfly(ButterflyType.AdonisBlue, TeamType.Enemy);
        ButterflyService.Instance.AddButterfly(ButterflyType.AdonisBlue, TeamType.Enemy);
        ButterflyService.Instance.EnableAllButterfly();
    }
}
