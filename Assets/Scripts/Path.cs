using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Path : MonoBehaviour
{
    public Transform[] waypoints;

    private void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        Handles.color = Color.blue;

        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Handles.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
        #endif
    }
}
