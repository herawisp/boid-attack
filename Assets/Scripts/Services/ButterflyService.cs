using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum TeamType {Player, Enemy}

public enum ButterflyType {
    AdonisBlue, BlackHairstreak, Brimstone, BrownArgus,
    BronHairstreak, ChalkHillBlue, ChequeredSkipper, CloudedYellow,
    Comma, CommonBlue, CrypticWoodWhite, OrangeTip,
    PurpleEmperor, PurpleHairstreak, RedAdmiral, WoodWhite
}

public class ButterflyService: MonoBehaviour {

    public static ButterflyService Instance {get; private set;}

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }


    //================================================================================================//
    //================================================================================================//

    [Header("Butterfly Datas")]
    public List<ButterflyData> ButterflyDatas;
    public List<SteeringBehaviour> SteeringBehaviours;
    public Dictionary<TeamType, List<Butterfly>> TeamsButterflies = new() {
        [TeamType.Player] = new(),
        [TeamType.Enemy] = new()
    };

    [Header("Unity Events")]
    public UnityEvent ButterflyAdded;
    public UnityEvent ButterflyRemoved;

    //================================================================================================//
    //================================================================================================//

    public void AddButterfly(ButterflyType butterflyType, TeamType teamType) {
        Butterfly butterfly = InstantiateButterfly(butterflyType, teamType);
        butterfly.TeamType = teamType;
        butterfly.Disable();

        SteeringBehaviours.Add(butterfly.GetComponent<SteeringBehaviour>());
        TeamsButterflies[teamType].Add(butterfly);
        ButterflyAdded.Invoke();
    }

    public void RemoveButterfly(Butterfly member, TeamType teamType) {
        Destroy(member.gameObject);
        TeamsButterflies[teamType].Remove(member);
        ButterflyRemoved.Invoke();
    }

    public void SetButterflies(List<ButterflyType> butterflyTypes, TeamType teamType) {
        foreach (ButterflyType butterflyType in butterflyTypes) {
            Butterfly butterfly = InstantiateButterfly(butterflyType, teamType);
            butterfly.TeamType = teamType;
            butterfly.Disable();

            SteeringBehaviours.Add(butterfly.GetComponent<SteeringBehaviour>());
            TeamsButterflies[teamType].Add(butterfly);
        }
        ButterflyAdded.Invoke();
    }

    public void ClearButterflies(TeamType teamType) {
        foreach (Butterfly butterfly in TeamsButterflies[teamType]) {
            Destroy(butterfly.gameObject);
        }
        TeamsButterflies[teamType].Clear();
        ButterflyRemoved.Invoke();
    }

    public void EnableAllButterfly() {
        foreach (List<Butterfly> butterflies in TeamsButterflies.Values) {
            foreach (Butterfly butterfly in butterflies) {
                butterfly.Enable();
            }
        }
    }
    
    //================================================================================================//
    //================================================================================================//

    private Butterfly InstantiateButterfly(ButterflyType butterflyType, TeamType teamType) {
        ButterflyData butterflyData = ButterflyDatas[(int) butterflyType];
        Butterfly butterfly = Instantiate(butterflyData.Prefab, transform);
        butterfly.TeamType = teamType;

        Animator animator = butterfly.GetComponent<Animator>();
        animator.runtimeAnimatorController = butterflyData.AnimatorController;

        if (teamType == TeamType.Enemy) {
            SpriteRenderer spriteRenderer = butterfly.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new(1, 0.5f, 0.5f);
        }
        return butterfly;
    }

    //================================================================================================//
    //================================================================================================//
}
