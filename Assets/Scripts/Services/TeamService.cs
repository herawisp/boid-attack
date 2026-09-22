using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum TeamType {Player, Enemy}

public class TeamService : MonoBehaviour {
    
    //================================================================================================//
    //================================================================================================//

    public UnityEvent MemberAdded;
    public UnityEvent MemberRemoved;
    
    Dictionary<TeamType, List<Butterfly>> _teamMembers;

    //================================================================================================//
    //================================================================================================//

    public void AddMember(Butterfly member, TeamType teamType) {
        _teamMembers[teamType].Add(member);
        MemberAdded.Invoke();
    }

    public void RemoveMember(Butterfly member, TeamType teamType) {
        Destroy(member.gameObject);
        _teamMembers[teamType].Remove(member);
        MemberRemoved.Invoke();
    }

    public void SetMembers(List<Butterfly> members, TeamType teamType) {
        _teamMembers[teamType] = members;
        MemberAdded.Invoke();
    }

    public void ClearMembers(TeamType teamType) {
        foreach (Butterfly butterfly in _teamMembers[teamType]) {
            Destroy(butterfly.gameObject);
        }
        _teamMembers[teamType].Clear();
        MemberRemoved.Invoke();
    }
    
    //================================================================================================//
    //================================================================================================//
}
