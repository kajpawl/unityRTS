using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [Header("Components")]
    public GameObject selectionVisual;
    private NavMeshAgent navAgent;

    void Awake()
    {
        // get the components
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void MoveToPosition(Vector3 pos)
    {
        navAgent.isStopped = false;
        navAgent.SetDestination(pos);
    }

    public void ToggleSelectionVisual(bool selected)
    {
        selectionVisual.SetActive(selected);
    }
}
