using UnityEngine;

public class Test : MonoBehaviour {
    
    public ServiceManager serviceManager;

    void Start()
    {
        serviceManager.ButterflyService.AddButterfly(ButterflyType.AdonisBlue, TeamType.Player);
        serviceManager.ButterflyService.AddButterfly(ButterflyType.AdonisBlue, TeamType.Player);
        serviceManager.ButterflyService.AddButterfly(ButterflyType.AdonisBlue, TeamType.Player);
        serviceManager.ButterflyService.AddButterfly(ButterflyType.AdonisBlue, TeamType.Enemy);
        serviceManager.ButterflyService.AddButterfly(ButterflyType.AdonisBlue, TeamType.Enemy);
        serviceManager.ButterflyService.AddButterfly(ButterflyType.AdonisBlue, TeamType.Enemy);
        serviceManager.ButterflyService.EnableAllButterfly();
    }
}
