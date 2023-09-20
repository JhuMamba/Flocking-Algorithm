using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Space Baghatur/Flock/Behaviour/Follow Behaviour")]
public class FollowBehaviour : FilteredFlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neighbours, return no adjustment
        if (context.Count == 0) return Vector2.zero;

        //add all points together and average
        Vector2 followMove = Vector2.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            followMove += (Vector2)(item.position);
        }
        followMove /= filteredContext.Count;
        followMove -= (Vector2)agent.transform.position;
        return followMove;
    }
}
