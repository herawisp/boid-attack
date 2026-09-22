using System.Collections.Generic;
using UnityEngine;

public enum ButterflyType {
    AdonisBlue, BlackHairstreak, Brimstone, BrownArgus,
    BronHairstreak, ChalkHillBlue, ChequeredSkipper, CloudedYellow,
    Comma, CommonBlue, CrypticWoodWhite, OrangeTip,
    PurpleEmperor, PurpleHairstreak, RedAdmiral, WoodWhite
}

public class ButterflyService: MonoBehaviour {

    //================================================================================================//
    //================================================================================================//

    public Dictionary<TeamType, List<ButterflyType>> ButterflyTeams {get; private set;}
    public Dictionary<TeamType, List<GameObject>> ButterflyObjects {get; private set;}
    public List<ButterflyData> ButterflyDatas;
    public GameObject ButterflyPrefab;

    //================================================================================================//
    //================================================================================================//
    
    public void AddButterfly(ButterflyType butterflyType, TeamType teamType) {
        ButterflyTeams[teamType].Add(butterflyType);
    }

    public void ClearTeamButterflies(TeamType teamType) {
        foreach (GameObject butterflyObject in ButterflyObjects[teamType]) {
            Destroy(butterflyObject);
        }
    }

    public void ClearAllButterflies(){
        ClearTeamButterflies(TeamType.Player);
        ClearTeamButterflies(TeamType.Enemy);    
    }

    public void SpawnTeamButterflies(TeamType teamType) {
        foreach (ButterflyType butterflyType in ButterflyTeams[teamType]) {
            GameObject butterflyObject = Instantiate(ButterflyPrefab);
            Animator animatorController = butterflyObject.GetComponent<Animator>();
            animatorController.runtimeAnimatorController = ButterflyDatas[(int) butterflyType].animatorController;

            if (teamType == TeamType.Enemy) {
                SpriteRenderer spriteRenderer = butterflyObject.GetComponent<SpriteRenderer>();
                spriteRenderer.color = new(1, 0.5f, 0.5f);

                Butterfly butterfly = butterflyObject.GetComponent<Butterfly>();
                butterfly.TeamType = TeamType.Enemy;
            }
        }
    }

    //================================================================================================//
    //================================================================================================//

    void Awake()
    {
        ButterflyTeams = new() {
            [TeamType.Player] = new(),
            [TeamType.Enemy] = new(),
        };  
    }

    void Start()
    {
        AddButterfly(ButterflyType.AdonisBlue, TeamType.Player);
        // AddButterfly(ButterflyType.AdonisBlue, TeamType.Player);
        // AddButterfly(ButterflyType.AdonisBlue, TeamType.Player);
        // AddButterfly(ButterflyType.ChequeredSkipper, TeamType.Enemy);
        // AddButterfly(ButterflyType.ChequeredSkipper, TeamType.Enemy);
        // AddButterfly(ButterflyType.ChequeredSkipper, TeamType.Enemy);
        SpawnTeamButterflies(TeamType.Player);
        SpawnTeamButterflies(TeamType.Enemy);
    }

    //================================================================================================//
    //================================================================================================//
}
