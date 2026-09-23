using UnityEngine;

public class Test : MonoBehaviour {
    
    public ServiceManager serviceManager;

    void Start()
    {
        serviceManager.ButterflyService.AddButterfly(ButterflyType.AdonisBlue, TeamType.Player);
        serviceManager.ButterflyService.EnableAllButterfly();
        // serviceManager.TeamService.AddMember(ButterflyType.AdonisBlue, TeamType.Enemy);
    }
}
