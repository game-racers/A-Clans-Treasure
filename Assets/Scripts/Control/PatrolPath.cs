using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {        
        const float waypointGizmoRadius = 0.3f;
        private void OnDrawGizmos() 
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWayPoint(i), waypointGizmoRadius);
                Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(j));
            }
        }

        public int GetNextIndex(int i)
        {
            return (i + 1) % (transform.childCount);
        }

        public Vector3 GetWayPoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}