using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehaviour behaviour;

    [Range(10, 500)]
    public int startingCount = 250;
    const float AgentDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 100f)]
    public float neighbourRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;
    [Range(10f, 100f)]
    public float detectRadius = 25f;

    float squareMaxSpeed;
    float squareNeighbourRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitCircle * startingCount * AgentDensity + (Vector2)transform.position,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
                );
            newAgent.name = "Agent" + i;
            newAgent.Initialize(this);
            newAgent.GetComponent<ShipAttack>().enabled = true;
            agents.Add(newAgent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);
            Vector2 move = behaviour.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed) move = move.normalized * maxSpeed;
            if (float.IsNaN(move.x) || float.IsNaN(move.y)) move = agent.transform.up;
            agent.Move(move);
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent) 
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(agent.transform.position, neighbourRadius);
        foreach (Collider2D c in colliders)
        {
            if (c != agent.AgentCollider ) context.Add(c.transform);
        }
        return context;
    }

    public List<Transform> GetEnemyInRange(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(agent.transform.position, detectRadius);
        foreach (Collider2D c in colliders)
        {
            if (c != agent.AgentCollider) context.Add(c.transform);
        }
        return context;
    }
}
