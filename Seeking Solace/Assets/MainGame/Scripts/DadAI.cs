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

    #region Variables
    private Mode behaviorMode;
    private Ray[] vision;
    private NavMeshAgent Agent;
    private float timeInSpotlight;
    private IEnumerator activeSchedule;
    private Animator animator;
    private int currFloor;
    private Vector3 currRoom;
    [SerializeField] private Transform targetPos;
    private bool atCurrDest;
    private bool pathingUp;
    private bool pathingDown;

    private int aggression;

    private int areaMask = 0;

    private Vector3 currTarget = Vector3.zero;
    #endregion

    public static event Action<Vector3> DadAlert;

    private void OnEnable()
    {
        DadAlert += goToAlert;
        GameManager.nextNight += newNight;
    }
    private void OnDisable()
    {
        DadAlert -= goToAlert;
        GameManager.nextNight -= newNight;
    }
    private void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();
        Agent = GetComponent<NavMeshAgent>();
        animator = gameObject.transform.GetChild(0).GetComponent<Animator>();

        behaviorMode = Mode.Wandering;

        vision = new Ray[9];
        areaMask = 1;
        currFloor = 1;
        currRoom = transform.position;



        activeSchedule = schedule();
        StartCoroutine(activeSchedule);
    }
    private void newNight(int nightNum)
    {
        Debug.Log("Night " + nightNum);
        if (nightNum == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        else
            aggression = 1 * nightNum;
        Agent.speed = Agent.speed * (Mathf.Log10(aggression) * 3 + 1);
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

        if (Input.GetKeyDown(KeyCode.V)) goToAlert(GameObject.Find("Player").transform.position);
        if (Input.GetKeyDown(KeyCode.B)) pathUpstairs();

        animator.SetBool("Walking", Agent.velocity.magnitude >= 1);

        if ((Agent.remainingDistance <= 0.1f) != atCurrDest)
        {
            Debug.Log("Arrived: " + (Agent.remainingDistance <= 0.1f));
            Debug.Log("Pathing Upstairs: " + pathingUp + ", Pathing Downstairs: " + pathingDown);
            if (pathingUp) pathUpstairs2();
            if (pathingDown) tpFloor1();
        }
        atCurrDest = Agent.remainingDistance <= 0.1f;

        //Debug.Log(Agent.destination + ", " + Vector3.Distance(transform.position, Agent.destination));
    }

    private void pathUpstairs()
    {
        pathingUp = true;
        StopAllCoroutines();
        Debug.Log("Pathing upstairs");
        Vector3 destination = GameObject.Find("Room Centers Floor 1").transform.GetChild(7).position;

        Agent.SetDestination(destination);
        targetPos.position = destination;
    }
    private void pathUpstairs2()
    {
        transform.rotation = Quaternion.identity;
        Debug.Log("Climbing");
        animator.SetTrigger("Climb");
        pathingUp = false;
    }
    private void pathDownstairs()
    {
        StopAllCoroutines();
        Vector3 destination = GameObject.Find("Room Centers Floor 2").transform.GetChild(7).position;

        Agent.SetDestination(destination);
        targetPos.position = destination;
    }

    public void tpFloor2()  //Gets triggered from the climbing animation
    {
        Debug.Log("TP Floor 2");
        //Animation only makes it look like he's climbing, so this would teleport him up once his animation is done
        gameObject.transform.position = FindObjectOfType<HouseNavManager>().getNearbyRooms(2).transform.GetChild(7).position;
        Agent.areaMask = 1 << 3;
        currFloor = 2;

        currRoom = transform.position;
        StartCoroutine(activeSchedule);
    }
    public void tpFloor1()
    {
        gameObject.transform.position = FindObjectOfType<HouseNavManager>().getNearbyRooms(1).transform.GetChild(7).position;
        Agent.areaMask = 1 << 0;
        currFloor = 1;

        currRoom = transform.position;
        StartCoroutine(activeSchedule);
    }

    private void Wander()   //Pick a random point in the current room and go to it
    {
        Vector3 destination = randomNavmeshPoint();
        if (destination != Vector3.zero)
        {
            Agent.SetDestination(destination);
            targetPos.position = destination;
        }
    }

    private void Adventure()    //Pick a random point in a nearby room and go to it
    {
        currRoom = FindObjectOfType<HouseNavManager>().getRandomRoomCenter(currFloor);

        Vector3 destination = randomNavmeshPoint();
        if (destination != Vector3.zero)
        {
            Agent.SetDestination(destination);
            targetPos.position = destination;
        }
    }

    private IEnumerator schedule()  //Wander to a random spot in the room every 5-10 seconds, then change rooms every minute
    {
        float loopTime;
        if (aggression != 0)
            loopTime = 15f / aggression;
        else
            loopTime = 15;
        float startTime = Time.time;

        while (startTime + loopTime > Time.time)
        {
            Wander();
            yield return new WaitForSeconds(Random.Range(3, 6));
            Debug.Log("Time remaining: " + (startTime - Time.time + loopTime));
            yield return new WaitForEndOfFrame();
        }
        
        Debug.Log("Adventuring");
        Adventure();

        while (!Agent.pathPending)
        {
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("Restarting Schedule");
                    activeSchedule = schedule();
                    StartCoroutine(activeSchedule);
                    break;
                }
            }
        }
    }

    private void goToAlert(Vector3 alertLocation)
    {
        Debug.Log("Chasing");
        animator.SetBool("Chasing", true);
        NavMeshHit hit;
        Debug.Log(alertLocation + ", " + NavMesh.SamplePosition(alertLocation, out hit, Agent.height * 2, NavMesh.AllAreas));
        //Stop Schedule
        StopAllCoroutines();
        //Increase speed
        Agent.speed *= 2;

        //Set destination to alert location
        NavMesh.SamplePosition(alertLocation, out hit, Agent.height * 2, NavMesh.AllAreas);
        Agent.SetDestination(hit.position);
        //Start playing angry monster noises
        Debug.Log(Agent.path.status);
        Agent.speed /= 2;
        animator.SetBool("Chasing", false);

        //Deal with the alert

        while (!Agent.pathPending)
        {
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("Restarting Schedule");
                    activeSchedule = schedule();
                    StartCoroutine(activeSchedule);
                    break;
                }
            }
        }
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

    private Vector3 getRoomCenter()
    {
        Debug.Log("Current room: " + currRoom);
        return currRoom;
    }

    private IEnumerator tempOpenDoor(GameObject door)
    {
        door.SendMessage("Toggle");
        //door.GetComponent<MeshCollider>().enabled = false;

        yield return new WaitForSeconds(1);

        //door.GetComponent<MeshCollider>().enabled = true;
        door.SendMessage("Toggle");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Enter");
        if (collision.gameObject.CompareTag("Door"))
        {
            StartCoroutine(tempOpenDoor(collision.gameObject));
        }
    }
}
