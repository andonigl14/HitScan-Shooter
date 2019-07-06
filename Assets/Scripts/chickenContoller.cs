using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class chickenContoller : MonoBehaviour {

    private NavMeshAgent navAgent;
    public GameObject target;  

    private void OnEnable()
    {
        navAgent = this.GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Finish");
        navAgent.SetDestination(target.transform.position);
    }
   
}
