using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour 
    {
        const float waypointGizmoRadius = 0.3f;        

        private void OnDrawGizmos() {

            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);  // Wrap around to the first waypoint if we reach the end.

                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }

        }

        public int GetNextIndex(int i)
        {
            return (i + 1) % transform.childCount;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
    
}