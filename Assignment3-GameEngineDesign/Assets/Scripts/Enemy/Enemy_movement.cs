using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_movement : MonoBehaviour
{
    public float lookRadius = 10f;

    Transform target;
    NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        target = Player_Manager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);
        }
        FaceTarget();
    }
    // This is so that the enemy will try to face the direction the player is in
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion LookRot = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, LookRot, Time.deltaTime * 5);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

    }
}
