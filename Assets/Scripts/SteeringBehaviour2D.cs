using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SteeringBehaviour : MonoBehaviour {
    
    public Vector2 Position = Vector2.zero;
    public Vector2 Velocity = Vector2.zero;
    public Vector2 SteeringForce = Vector2.zero;
    public float MaxSpeed = 0.007f;
    public float MaxForce = 5e-05f;

    [Header("Behaviour Weights")]
    public float SeekForceWeight;
    public float FleeForceWeight;
    public float PursueForceWeight;
    public float EvadeForceWeight;
    public float ArriveForceWeight;
    public float WanderForceWeight;
    public float AlignForceWeight;
    public float CohesionForceWeight;
    public float SeperationForceWeight;

    [Header("Seek & Flee Behaviour")]
    public Vector2 SeekTargetPosition;
    public Vector2 FleeTargetPosition;
    
    [Header("Pursue, Evade, & Arrive Behaviour")]
    public SteeringBehaviour PursuedAgent;
    public SteeringBehaviour EvadeAgent;
    public Vector2 ArriveTargetPosition;
    public float ArriveSlowingRadius;

    [Header("Wander Behaviour")]
    public float WanderDistance = 5f;
    public float WanderPower = 3f;
    public float WanderChange = 7f;
    public float WanderAngle;

    [Header("Flocking Behaviour")]
    public List<SteeringBehaviour> AgentFlockList;
    public float SeperationDistance;

    void Update() {
        if (SeekForceWeight != 0) ApplyForce(SeekForce(SeekTargetPosition.x, SeekTargetPosition.y), SeekForceWeight);
        if (FleeForceWeight != 0) ApplyForce(FleeForce(FleeTargetPosition.x, FleeTargetPosition.y), FleeForceWeight);
        if (PursueForceWeight != 0) ApplyForce(PursueForce(PursuedAgent), PursueForceWeight);
        if (EvadeForceWeight != 0) ApplyForce(EvadeForce(EvadeAgent), EvadeForceWeight);
        if (ArriveForceWeight != 0) ApplyForce(ArriveForce(ArriveTargetPosition.x, ArriveTargetPosition.y, ArriveSlowingRadius), ArriveForceWeight);
        if (WanderForceWeight != 0) ApplyForce(WanderForce(), WanderForceWeight);
        if (AlignForceWeight != 0) ApplyForce(AlignForce(), AlignForceWeight);
        if (CohesionForceWeight != 0) ApplyForce(CohesionForce(), CohesionForceWeight);
        if (SeperationForceWeight != 0) ApplyForce(SeperationForce(), SeperationForceWeight);


        Velocity += SteeringForce;
        Velocity = Vector2.ClampMagnitude(Velocity, MaxSpeed);
        Position += Velocity;

        SteeringForce = Vector2.zero;
    }

    private void ApplyForce(Vector2 force, float weight) {
        force *= weight;
        SteeringForce += force;
    }

    private Vector2 SeekForce(float x, float y) {
        Vector2 vector = new(x, y);
        vector -= Position;
        vector = vector.normalized * MaxSpeed;
        vector -= Velocity;
        vector = Vector2.ClampMagnitude(vector, MaxForce);
        return vector;
    }

    private Vector2 FleeForce(float x, float y) {
        Vector2 vector = new(x, y);
        vector -= Position;
        vector = vector.normalized * MaxSpeed;
        vector *= -1;
        vector -= Velocity;
        vector = Vector2.ClampMagnitude(vector, MaxForce);
        return vector;
    }

    private Vector2 PursueForce(SteeringBehaviour other) {
        Vector2 vector = other.Velocity;
        vector *= 10;
        vector += other.Position;
        return SeekForce(vector.x, vector.y);
    }

    private Vector2 EvadeForce(SteeringBehaviour other) {
        Vector2 vector = other.Velocity;
        vector *= 10;
        vector += other.Position;
        return FleeForce(vector.x, vector.y);
    }

    private Vector2 ArriveForce(float x, float y, float slowingRadius) {
        Vector2 vector = new(x, y);
        vector -= Position;

        float dist = vector.magnitude;
        if (dist > slowingRadius) {
            vector = vector.normalized * MaxSpeed;
        } else {
            vector = vector.normalized * (dist / slowingRadius);
        }

        vector -= Velocity;
        vector = Vector2.ClampMagnitude(vector, MaxForce);
        return vector;
    }

    private Vector2 WanderForce() {
        Vector2 vector = Velocity;
        vector = vector.normalized * WanderDistance;
        
        float radians = (transform.eulerAngles.z + WanderAngle) * Mathf.Deg2Rad;
        Vector2 lengthDirVector = new(
            Mathf.Cos(radians) * WanderPower,
            Mathf.Sin(radians) * WanderPower
        );

        vector += lengthDirVector;
        vector = Vector2.ClampMagnitude(vector, MaxForce);
        WanderAngle += Random.Range(-WanderChange, WanderChange);
        return vector;
    }

    private Vector2 AlignForce() {
        Vector2 vector = new(); 
        int count = 0;

        foreach (SteeringBehaviour agent in AgentFlockList) {
            if (agent == this) continue;
            vector += agent.Velocity;
            count ++;
        }

        if (count > 0) {
            vector = vector.normalized * MaxForce;
        }

        return vector;
    }

    private Vector2 CohesionForce() {
        Vector2 vector = new(); 
        int count = 0;

        foreach (SteeringBehaviour agent in AgentFlockList) {
            if (agent == this) continue;
            vector += agent.Position;
            count ++;
        }

        if (count > 0) {
            vector /= count;
            vector = SeekForce(vector.x, vector.y);
        }

        return vector;
    }

    private Vector2 SeperationForce() {
        Vector2 vector = new(); 
        Vector2 vectorTo; 
        int count = 0;

        foreach (SteeringBehaviour agent in AgentFlockList) {
            if (agent == this) continue;
            vectorTo = Position - agent.Position;
            float dist = Mathf.Min(vectorTo.magnitude, SeperationDistance);
            float scale = 1 - (dist / SeperationDistance);
            vectorTo *= scale;
            vector += vectorTo;
            count ++;
        }

        if (count > 0) {
            vector = vector.normalized * MaxForce;
        }

        return vector;
    }
}
