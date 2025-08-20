using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class AgentCtrl : MonoBehaviourPun
{
    private NavMeshAgent agent;
    private Animator anim;

    [SerializeField] private float wanderRadius = 30f;

    private float minWaitTime = 1f;
    private float maxWaitTime = 5f;

    [SerializeField] private float turnSpeed = 10f;

    private bool isDead = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agent.updateRotation = false;
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        while (!isDead)
        {

            var randomDir = Random.insideUnitSphere * wanderRadius;
            randomDir += transform.position;

            photonView.RPC(nameof(SetRandomDestination), RpcTarget.AllBuffered, randomDir);


            float moveType = Random.Range(0, 2) == 0 ? 0.5f : 1f;
            anim.SetFloat("Speed", moveType);
            agent.speed = moveType * 4f;

            yield return new WaitUntil(() => !isDead && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);

            anim.SetFloat("Speed", 0f);
            float idleTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(idleTime);
        }
    }

    void Update()
    {
        Vector3 dir = agent.desiredVelocity;
        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed *  Time.deltaTime);
        }
    }

    [PunRPC]
    private void SetRandomDestination(Vector3 dir)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(dir, out hit, wanderRadius, NavMesh.AllAreas))
        {
            if (!isDead)
                agent.SetDestination(hit.position);
        }
    }

    public void GetHit()
    {
        photonView.RPC(nameof(Dead), RpcTarget.AllBuffered);
    }

    [PunRPC] 
    private void Dead()
    {
        isDead = true;
        GetComponent<Collider>().enabled = false;
        anim.SetTrigger("death");
        agent.updatePosition = false;
        agent.isStopped = true;
    }
}
