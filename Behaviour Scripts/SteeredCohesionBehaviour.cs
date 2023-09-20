using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Space Baghatur/Flock/Behaviour/Steered Cohesion")]
public class SteeredCohesionBehaviour : FilteredFlockBehaviour
{
    Vector2 _currentVelocity;

    public float agentSmoothTime = 0.5f;
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neighbours, return no adjustment
        if (context.Count == 0) return Vector2.zero;

        //add all points together and average
        Vector2 cohesionMove = Vector2.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            cohesionMove += (Vector2)item.position;
        }
        cohesionMove /= filteredContext.Count;
        //create offset from agent positin
        cohesionMove -= (Vector2)agent.transform.position;
        if (float.IsNaN(_currentVelocity.x) || float.IsNaN(_currentVelocity.y)) _currentVelocity = Vector2.zero;
        cohesionMove = Vector2.SmoothDamp(agent.transform.up, cohesionMove, ref _currentVelocity, agentSmoothTime);
        return cohesionMove;
    }
}
