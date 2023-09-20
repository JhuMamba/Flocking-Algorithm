using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Space Baghatur/Flock/Filter/Physics Filter")]
public class PhysicsLayerFilter : ContextFilter
{
    public LayerMask mask;

    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
    {
        List<Transform> filtered = new();
        foreach (Transform item in original)
        {
            if (mask == (mask | (1 << item.gameObject.layer))) filtered.Add(item);
        }
        return filtered;
    }
}
