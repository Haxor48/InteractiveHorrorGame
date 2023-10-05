using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private AudioHandler audioHandler;
    public bool isRunning;
    private void Awake()
    {
        isRunning = true;
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioHandler>();
    }

    private void Update()
    {
        if (isRunning)
        {
            int[] enemyPos = { (int)gameObject.transform.position.x, (int)gameObject.transform.position.x };
            int[] desination = audioHandler.getMaxVolPos(enemyPos);
            navMeshAgent.destination = new Vector3(desination[0], gameObject.transform.position.y, desination[1]);
        }
            }
}
