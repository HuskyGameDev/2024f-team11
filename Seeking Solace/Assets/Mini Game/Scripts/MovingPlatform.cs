using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private WaypointPath path;
    [SerializeField] float speed;
    [SerializeField] bool resets;

    private int targetWaypointIndex;
    private Transform previousWaypoint;
    private Transform targetWaypoint;
    private float timeToWaypoint;
    private float elapsedTime;

    void Start()
    {
        targetWaypointIndex = 0;
        targetNextWaypoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        float elapsedPercentage = elapsedTime / timeToWaypoint;
        if(!resets){
            elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
        }
        transform.position = Vector3.Lerp(previousWaypoint.position, targetWaypoint.position, elapsedPercentage);
        if(previousWaypoint.rotation.z > targetWaypoint.rotation.z){
            if(elapsedPercentage >= .7){
                transform.rotation = Quaternion.Lerp(previousWaypoint.rotation, targetWaypoint.rotation, elapsedPercentage);
            } else {
                transform.rotation = previousWaypoint.rotation;
            }
        } else {
            if(elapsedPercentage <= .3){
                transform.rotation = Quaternion.Lerp(previousWaypoint.rotation, targetWaypoint.rotation, elapsedPercentage);
            } else {
                transform.rotation = targetWaypoint.rotation;
            }
        }

        if(elapsedPercentage >= 1){
            targetNextWaypoint();
        }
        
        if(targetWaypointIndex == 0 && resets){
            reset();
        }
    }

    private void targetNextWaypoint(){
        previousWaypoint = path.getWaypoint(targetWaypointIndex);
        targetWaypointIndex = path.getNextWaypointIndex(targetWaypointIndex);
        targetWaypoint = path.getWaypoint(targetWaypointIndex);

        elapsedTime = 0;

        float distanceToNext = Vector3.Distance(previousWaypoint.position, targetWaypoint.position);
        timeToWaypoint = distanceToNext/speed;
    }

    private void reset(){
        transform.position = targetWaypoint.position;
        targetNextWaypoint();
    }

    private void OnTriggerEnter(Collider other){
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other){
        other.transform.SetParent(null);
    }
}
