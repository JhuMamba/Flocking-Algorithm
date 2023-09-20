using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Space Baghatur/Flock/Behaviour/Composite")]
public class CompositeBehaviour : FlockBehaviour
{
    public FlockBehaviour[] behaviours;
    public float[] weights;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //Handling data length error.
        if (weights.Length != behaviours.Length)
        {
            Debug.LogError("Data Lengths are not the same. In: " + name, this);
            return Vector2.zero;
        }
        //Move Setup
        Vector2 move = Vector2.zero;

        List<Transform> tmpContext = context;
        //Iterate through behaviours
        for (int i = 0; i < behaviours.Length; i++)
        {
            if (behaviours[i] is FollowBehaviour)
                context = agent.AgentFlock.GetEnemyInRange(agent);
            else
                context = tmpContext;

            Vector2 partialMove = behaviours[i].CalculateMove(agent, context, flock) * weights[i];
            if (partialMove != Vector2.zero)
            {
                if (partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                move += partialMove;
            }
        }

        return move;
    }
}
