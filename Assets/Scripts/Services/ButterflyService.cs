using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum TeamType {Player, Enemy}

public enum ButterflyType {
    AdonisBlue, BlackHairstreak, Brimstone, BrownArgus,
    BrownHairstreak, ChalkHillBlue, ChequeredSkipper, CloudedYellow,
    Comma, MiniComma, CommonBlue, CrypticWoodWhite, OrangeTip,
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
    public Butterfly ButterflyPrefab;
    public List<ButterflyData> ButterflyDatas;
    public List<SteeringBehaviour> SteeringBehaviours;
    public Dictionary<TeamType, List<Butterfly>> TeamsButterflies = new() {
        [TeamType.Player] = new(),
        [TeamType.Enemy] = new()
    };

    //================================================================================================//
    //================================================================================================//

    public event System.Action<Butterfly> ButterflyDied;
    public event System.Action<Butterfly> ButterflyHealed;

    public void RaiseButterflyDied(Butterfly b) => ButterflyDied?.Invoke(b);
    public void RaiseButterflyHealed(Butterfly b) => ButterflyHealed?.Invoke(b);

    //================================================================================================//
    //================================================================================================//

    public void AddButterfly(ButterflyType butterflyType, TeamType teamType) {
        Butterfly butterfly = InstantiateButterfly(butterflyType, teamType);
        butterfly.TeamType = teamType;
        butterfly.Disable();

        SteeringBehaviours.Add(butterfly.GetComponent<SteeringBehaviour>());
        TeamsButterflies[teamType].Add(butterfly);
    }

    public void RemoveButterfly(Butterfly member, TeamType teamType) {
        Destroy(member.gameObject);
        TeamsButterflies[teamType].Remove(member);
    }

    public void SetButterflies(List<ButterflyType> butterflyTypes, TeamType teamType) {
        ClearButterflies(teamType);
        foreach (ButterflyType butterflyType in butterflyTypes) {
            Butterfly butterfly = InstantiateButterfly(butterflyType, teamType);
            butterfly.TeamType = teamType;
            butterfly.Disable();

            SteeringBehaviours.Add(butterfly.GetComponent<SteeringBehaviour>());
            TeamsButterflies[teamType].Add(butterfly);
        }
    }

    public void ClearSpawnedButterflies(TeamType teamType) {
        List<Butterfly> toRemove = TeamsButterflies[teamType].FindAll(b => b.IsSpawned);
        foreach (Butterfly b in toRemove) {
            SteeringBehaviours.Remove(b.GetComponent<SteeringBehaviour>());
            Destroy(b.gameObject);
            TeamsButterflies[teamType].Remove(b);
        }
    }

    public void ClearButterflies(TeamType teamType) {
        foreach (Butterfly butterfly in TeamsButterflies[teamType]) {
            Destroy(butterfly.gameObject);
        }
        TeamsButterflies[teamType].Clear();
    }

    public void EnableAllButterfly() {
        foreach (List<Butterfly> butterflies in TeamsButterflies.Values) {
            foreach (Butterfly butterfly in butterflies) {
                butterfly.Enable();
            }
        }
    }

    // ButterflyService.cs
    public void DisableAllButterfly() {
        foreach (List<Butterfly> butterflies in TeamsButterflies.Values) {
            foreach (Butterfly butterfly in butterflies) {
                butterfly.Disable();
            }
        }
    }

    public int CountEnabledButterfly() {
        int count = 0;
        foreach (List<Butterfly> butterflies in TeamsButterflies.Values) {
            foreach (Butterfly butterfly in butterflies) {
                if (butterfly.gameObject.activeInHierarchy) count++; 
            }
        }
        return count;
    }

    public List<Butterfly> GetButterflies(TeamType teamType) {
        return TeamsButterflies[teamType].FindAll(b => b.gameObject.activeInHierarchy);
    }
    
    public Butterfly SpawnButterfly(
        ButterflyType butterflyType, 
        TeamType teamType, 
        Vector3 position, 
        float scale = 1f, 
        float? healthOverride = null, 
        float? attackOverride = null
    ) {
        Butterfly butterfly = InstantiateButterfly(butterflyType, teamType);
        butterfly.transform.position = position;
        butterfly.transform.localScale *= scale;
        butterfly.TeamType = teamType;
        butterfly.IsSpawned = true;
        butterfly.Enable();

        if (healthOverride.HasValue) {
            butterfly.MaxHealth = healthOverride.Value;
            butterfly.Health = healthOverride.Value;
        }
        if (attackOverride.HasValue) {
            butterfly.AttackOverride = attackOverride.Value;
        }

        SteeringBehaviours.Add(butterfly.GetComponent<SteeringBehaviour>());
        TeamsButterflies[teamType].Add(butterfly);

        return butterfly;
    }
    //================================================================================================//
    //================================================================================================//

    private Butterfly InstantiateButterfly(ButterflyType butterflyType, TeamType teamType) {
        ButterflyData butterflyData = ButterflyDatas[(int) butterflyType];
        Butterfly butterfly = Instantiate(ButterflyPrefab, transform);

        Animator animator = butterfly.GetComponent<Animator>();
        animator.runtimeAnimatorController = butterflyData.AnimatorController;

        if (teamType == TeamType.Enemy) {
            SpriteRenderer spriteRenderer = butterfly.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new(1, 0.5f, 0.5f);
        }

        butterfly.Initialize(butterflyData, teamType);
        return butterfly;
    }

    //================================================================================================//
    //================================================================================================//
}
