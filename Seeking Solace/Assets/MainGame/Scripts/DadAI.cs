using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DadAI : MonoBehaviour
{
    #region Setup
    private enum Mode { Wandering, Adventuring, Alerted };

    private Mode behaviorMode;
    private Rigidbody rigidbody;
    private Ray[] vision;
    private NavMeshAgent Agent;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        Agent = GetComponent<NavMeshAgent>();

        behaviorMode = Mode.Wandering;

        vision = new Ray[9];
        updateRays();
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
        Debug.DrawRay(transform.position + Vector3.up, newRay.direction * 20, Color.blue);

        newRay.direction = transform.TransformDirection(forward + left/4 + up/4);   
        vision[1] = newRay;
        Debug.DrawRay(transform.position + Vector3.up, newRay.direction * 20, Color.blue);

        newRay.direction = transform.TransformDirection(forward + up / 4);
        vision[2] = newRay;
        Debug.DrawRay(transform.position + Vector3.up, newRay.direction * 20, Color.blue);

        newRay.direction = transform.TransformDirection(forward + right / 4 + up / 4);
        vision[3] = newRay;
        Debug.DrawRay(transform.position + Vector3.up, newRay.direction * 20, Color.blue);

        newRay.direction = transform.TransformDirection(forward + left / 4);
        vision[4] = newRay;
        Debug.DrawRay(transform.position + Vector3.up, newRay.direction * 20, Color.blue);

        newRay.direction = transform.TransformDirection(forward + right / 4);
        vision[5] = newRay;
        Debug.DrawRay(transform.position + Vector3.up, newRay.direction * 20, Color.blue);

        newRay.direction = transform.TransformDirection(forward + left/4 + down/4);
        vision[6] = newRay;
        Debug.DrawRay(transform.position + Vector3.up, newRay.direction * 20, Color.blue);

        newRay.direction = transform.TransformDirection(forward + down / 4);
        vision[7] = newRay;
        Debug.DrawRay(transform.position + Vector3.up, newRay.direction * 20, Color.blue);

        newRay.direction = transform.TransformDirection(forward + right / 4 + down / 4);
        vision[8] = newRay;
        Debug.DrawRay(transform.position + Vector3.up, newRay.direction * 20, Color.blue);
    }
    #endregion

    private void Update()
    {
        updateRays();
        if (visionCheck())
            Debug.Log("Child Spotted");
        if (Input.GetKeyDown("e"))
            wander(FindObjectOfType<CharacterController>().transform.position);
    }

    private void wander(Vector3 destination)
    {
        //Ray movePosition = Camera.main.ScreenPointToRay()
        //if (Physics.Raycast(movePosition, out var hitInfo))
        //    Agent.SetDestination(hitInfo.point);
        Agent.SetDestination(destination);
    }

    private void Adventure()
    {

    }

    private void goToAlert()
    {

    }

    private void moveToPoint(Vector3 point)
    {
        
    }

    private bool visionCheck()
    {
        updateRays();

        foreach (Ray ray in vision)
        {
            if (Physics.Raycast(ray, 20, 1 << 7))   //Only picks up collisions in layer 2
                return true;
        }

        return false;
    }
}
