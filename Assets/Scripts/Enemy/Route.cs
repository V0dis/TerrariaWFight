using System;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    [SerializeField] private List<Transform> _waypoints;
    
    private Queue<Transform> _sortedWaypoints = new Queue<Transform>();

    public void Initialize()
    {
        RebuildLoopedQueue(_waypoints);
    }

    public Transform GetWaypoint()
    {
        Transform waypoint = _sortedWaypoints.Peek();
        
        _sortedWaypoints.Enqueue(_sortedWaypoints.Dequeue());
        
        return waypoint;
    }

    private void RebuildLoopedQueue(List<Transform> originalWaypoints)
    {
        if (originalWaypoints == null || originalWaypoints.Count == 0)
            return;
        
        int nearestIndex = 0;
        float minDistance = Single.MaxValue;

        for (int i = 0; i < originalWaypoints.Count; i++)
        {
            float distance = (originalWaypoints[i].position - transform.position).sqrMagnitude;
            
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestIndex = i;
            }
        }
        
        for (int i = 0; i < originalWaypoints.Count; i++)
        {
            _sortedWaypoints.Enqueue(originalWaypoints[(nearestIndex + i) % originalWaypoints.Count]);
        }
    }
}
