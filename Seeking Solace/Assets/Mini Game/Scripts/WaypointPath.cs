using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    public Transform getWaypoint(int waypointIndex){
        return transform.GetChild(waypointIndex);
    }

    public int getNextWaypointIndex(int currentWaypoint){
        int nextWaypoint = currentWaypoint + 1;
        if(nextWaypoint == transform.childCount){
            nextWaypoint = 0;
        }
        return nextWaypoint;
    }
}
