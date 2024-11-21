using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Random = UnityEngine.Random;

public class DadAI : MonoBehaviour
{
    #region Setup
    private enum Mode { Wandering, Adventuring, Alerted };

    private Mode behaviorMode;
    private Ray[] vision;
    private NavMeshAgent Agent;
    private float timeInSpotlight;
    private IEnumerator activeSchedule;

    private int areaMask = 0;

    private Vector3 currTarget = Vector3.zero;

    public static event Action<Vector3> DadAlert;

    private void OnEnable()
    {
        DadAlert += goToAlert;
    }
    private void OnDisable()
    {
        DadAlert -= goToAlert;
    }

    private void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();
        Agent = GetComponent<NavMeshAgent>();

        behaviorMode = Mode.Wandering;

        vision = new Ray[9];
        areaMask = -1;
        pickCurrentRoom();

        activeSchedule = schedule();
        StartCoroutine(activeSchedule);
    }

    private void updateRays()
    {
        Vector3 forward = Vector3.forward;
        Vector3 left = Vector3.left;
        Vector3 right = Vector3.right;
        Vector3 up = Vector3.up;
        Vector3 down = Vector3.down;
        /*
         Updates rays to be arranged like this

        [   1   2   3   ]
        [   4   0   5   ]
        [   6   7   8   ]

         */
        Ray newRay = new Ray();
        newRay.origin = transform.position + Vector3.up;

        newRay.direction = transform.TransformDirection(forward);
        vision[0] = newRay;

        newRay.direction = transform.TransformDirection(forward + left/4 + up/4);   
        vision[1] = newRay;

        newRay.direction = transform.TransformDirection(forward + up / 4);
        vision[2] = newRay;

        newRay.direction = transform.TransformDirection(forward + right / 4 + up / 4);
        vision[3] = newRay;

        newRay.direction = transform.TransformDirection(forward + left / 4);
        vision[4] = newRay;

        newRay.direction = transform.TransformDirection(forward + right / 4);
        vision[5] = newRay;

        newRay.direction = transform.TransformDirection(forward + left/4 + down/4);
        vision[6] = newRay;

        newRay.direction = transform.TransformDirection(forward + down / 4);
        vision[7] = newRay;

        newRay.direction = transform.TransformDirection(forward + right / 4 + down / 4);
        vision[8] = newRay;
    }

    private void traceRays()
    {
        Vector3 forward = Vector3.forward;
        Vector3 left = Vector3.left;
        Vector3 right = Vector3.right;
        Vector3 up = Vector3.up;
        Vector3 down = Vector3.down;

        Debug.DrawRay(transform.position + Vector3.up, transform.TransformDirection(forward) * 20,                          Color.blue);
        Debug.DrawRay(transform.position + Vector3.up, transform.TransformDirection(forward + left / 4 + up / 4) * 20,      Color.blue);
        Debug.DrawRay(transform.position + Vector3.up, transform.TransformDirection(forward + up / 4) * 20,                 Color.blue);
        Debug.DrawRay(transform.position + Vector3.up, transform.TransformDirection(forward + right / 4 + up / 4) * 20,     Color.blue);
        Debug.DrawRay(transform.position + Vector3.up, transform.TransformDirection(forward + left / 4) * 20,               Color.blue);
        Debug.DrawRay(transform.position + Vector3.up, transform.TransformDirection(forward + right / 4) * 20,              Color.blue);
        Debug.DrawRay(transform.position + Vector3.up, transform.TransformDirection(forward + left / 4 + down / 4) * 20,    Color.blue);
        Debug.DrawRay(transform.position + Vector3.up, transform.TransformDirection(forward + down / 4) * 20,               Color.blue);
        Debug.DrawRay(transform.position + Vector3.up, transform.TransformDirection(forward + right / 4 + down / 4) * 20,   Color.blue);
    }
    #endregion

    private void Update()
    {
        updateRays();
        traceRays();

        if (visionCheck())      //Triggers events only if he sees you for a full second
            timeInSpotlight += Time.deltaTime;
        else
            timeInSpotlight = 0;

        if (timeInSpotlight >= 1f)
        {
            Debug.Log("Child Spotted");
            timeInSpotlight = 0;
        }

        if (Input.GetKeyDown("e")) goToAlert(FindObjectOfType<CharacterController>().transform.position);

        //Debug.Log(Agent.destination + ", " + Vector3.Distance(transform.position, Agent.destination));
    }

    private void Wander()   //Pick a random point in the current room and go to it
    {
        Vector3 destination = randomNavmeshPoint();
        if (destination != Vector3.zero)
            Agent.SetDestination(destination);
    }

    private void Adventure()    //Pick a random point in a nearby room and go to it
    {
        pickNearbyRoom();

        Vector3 destination = randomNavmeshPoint();
        if (destination != Vector3.zero)
            Agent.SetDestination(destination);
    }

    private IEnumerator schedule()  //Wander to a random spot in the room every 5-10 seconds, then change rooms every minute
    {
        float loopTime = 60;
        float startTime = Time.time;

        while (startTime + loopTime > Time.time)
        {
            Wander();
            yield return new WaitForSeconds(Random.Range(5, 10));
            Debug.Log("Time remaining: " + (startTime - Time.time + loopTime));
            yield return new WaitForEndOfFrame();
        }
        
        Debug.Log("Adventuring");
        Adventure();
        while (Agent.path.status != NavMeshPathStatus.PathComplete) 
            yield return new WaitForEndOfFrame();
        Debug.Log("Restarting Schedule");
        activeSchedule = schedule();
        StartCoroutine(activeSchedule);
    }

    private void goToAlert(Vector3 alertLocation)
    {
        NavMeshHit hit;
        Debug.Log(alertLocation + ", " + NavMesh.SamplePosition(alertLocation, out hit, Agent.height * 2, NavMesh.AllAreas));
        //Stop Schedule
        StopCoroutine(activeSchedule);
        //Increase speed
        Agent.speed *= 2;

        //Set destination to alert location
        NavMesh.SamplePosition(alertLocation, out hit, Agent.height * 2, NavMesh.AllAreas);
        Agent.SetDestination(hit.position);
        //Start playing angry monster noises
        Debug.Log(Agent.path.status);
        while (Agent.path.status != NavMeshPathStatus.PathComplete && Vector3.Distance(transform.position, alertLocation) > 1f)
        {
            Debug.Log("Angry noises >:(");
        }
        Agent.speed /= 2;

        //Deal with the alert

        //Resume schedule
        pickCurrentRoom();
        StartCoroutine(activeSchedule);
    }

    private bool visionCheck()  //Checks if the player is overlapping any of the rays cast from the head
    {
        updateRays();

        foreach (Ray ray in vision)
        {
            if (Physics.Raycast(ray, 20, 1 << 7))   //Only picks up collisions in player layer
                return true;
        }

        return false;
    }

    private Vector3 randomNavmeshPoint()
    {
        Vector3 randomPoint;
        NavMeshHit hit;

        for (int i = 0; i < 30; i++)
        {
            randomPoint = getRoomCenter() + Random.insideUnitSphere * Random.Range(0, 20f);
            if (NavMesh.SamplePosition(randomPoint, out hit, Agent.height * 2, areaMask))
                return hit.position;
        }
        Debug.Log("Failed to pick a point");
        return Vector3.zero;
    }

    private int pickNearbyRoom()    //Sets areaMask to a nearby room
    {
        List<int> nearbyRooms = FindObjectOfType<HouseNavManager>().getNearbyRooms((int) Mathf.Log(areaMask, 2) - 3);

        int choice = nearbyRooms[Random.Range(0, nearbyRooms.Count)] + 3;
        areaMask = 1 << choice;
        Debug.Log("Choice: " + choice + ", AreaMask = " + areaMask);

        return choice;
    }

    private void pickCurrentRoom()
    {
        Vector3 randomPoint;
        NavMeshHit hit;

        for (int i = 0; i < 30; i++)
        {
            randomPoint = transform.position + Random.insideUnitSphere * Random.Range(0, 10f);
            if (NavMesh.SamplePosition(randomPoint, out hit, Agent.height * 2, NavMesh.AllAreas))
            {
                areaMask = hit.mask;
                return;
            }
        }
    }

    private Vector3 getRoomCenter()
    {
        return FindObjectOfType<HouseNavManager>().getRoomCenter(areaMask);
    }
}
