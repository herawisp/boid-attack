using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour {

    Butterfly _butterfly;

    void Awake() {
        _butterfly = GetComponent<Butterfly>();
    }

    public Butterfly GetNearestOpposingButterfly(TeamType teamType, Vector3 position) {
        TeamType opposingTeam = (teamType == TeamType.Player) ? TeamType.Enemy : TeamType.Player;
        List<Butterfly> opposingTeamButterflies = _butterfly.ServiceManager.ButterflyService.TeamsButterflies[opposingTeam];

        if (opposingTeamButterflies == null || opposingTeamButterflies.Count == 0) {
            return null;
        }
        
        Butterfly closestButterfly = opposingTeamButterflies[0];
        float closestDistance = Mathf.Infinity;

        foreach (Butterfly butterfly in opposingTeamButterflies) {
            Vector3 butterflyPosition = butterfly.transform.position;
            float distance = Vector3.Distance(butterflyPosition, position);
            if (distance >= closestDistance) continue;

            closestButterfly = butterfly;
            closestDistance = distance;
        }    
        return closestButterfly;
    }
}
