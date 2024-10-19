using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    //Pre-initialized Variables some of them are visible for real-time monitoring
    [SerializeField]
    private Vector3 destination;
    [SerializeField]
    private UnityEngine.AI.NavMeshAgent agent;
    [SerializeField]
    private int idleTime;
    [SerializeField]
    private GameObject signPrefab;

    private GameObject signInstance;
    public bool isTalking = false;

    void Start()
    {
        //Get agent from npc
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        SetRandomDestination();
    }

    void Update()
    {
        //check's if the agent have destination and it is reachable and calls SetRandomDestination if needed
        if (!agent.hasPath)
        {
            SetRandomDestination();
        }
    }

    //calls when npc collision intersects with another collision(possible to check tag of collision object for future use)
    void OnCollisionEnter(Collision collision)
    {
        //check if collision object is talking or free
        bool prep = collision.gameObject.GetComponent<AIController>().isTalking;
        //10% chance to activate talk event main npc and collision npc must be free
        if (Random.Range(0, 10) == 0 && !isTalking && !prep)
        {
            //starts talk event for main and collision object
            Talk();
            collision.gameObject.GetComponent<AIController>().Talk();
        }
    }

    public void Talk()
    {
        //sets talk status
        isTalking = true;
        //stopping navMeshAgent
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        //creating and positioning talk sign and placing it as a child object so it will be attached to npc
        Vector3 signPosition = new Vector3(transform.position.x, transform.position.y + 0.45f, transform.position.z);
        signInstance = Instantiate(signPrefab, signPosition, transform.rotation);
        signInstance.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        signInstance.transform.parent = transform;
        //starts coroutine and sets new random point
        StartCoroutine(TalkEvent(idleTime));
        SetRandomDestination();
    }

    private void SetRandomDestination()
    {
        //random point in radius
        Vector3 randomPoint = new Vector3(
            Random.Range(-20f, 20f),
            0, 
            Random.Range(-20f, 20f)
        );

        //checks if closest point on navMesh is valid if else checks for another random point
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
        {
            destination = hit.position;
            agent.destination = destination;
        }
        else
        {
            SetRandomDestination();
        }
    }

    //this method waits for idleTime turns on navMeshAgent
    IEnumerator TalkEvent(int idleTime)
    {
        //Log
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //Wait time
        yield return new WaitForSeconds(idleTime);

        //turning on npc and removing talk sign
        agent.isStopped = false;
        DestroyImmediate(signInstance);
    }
}
