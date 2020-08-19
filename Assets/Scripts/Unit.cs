using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum UnitState
{
    Idle,
    Move,
    MoveToResource,
    Gather
}

public class Unit : MonoBehaviour
{
    [Header("Stats")]
    public UnitState state;

    public int gatherAmount;
    public float gatherRate;
    private float lastGatherTime;

    private ResourceSource curResourceSource;

    [Header("Components")]
    public GameObject selectionVisual;
    private NavMeshAgent navAgent;

    public Player player;

    // events
    public class StateChangeEvent : UnityEvent<UnitState> { }
    public StateChangeEvent onStateChange;

    void Awake()
    {
        // get the components
        navAgent = GetComponent<NavMeshAgent>();
    }

    void SetState(UnitState toState)
    {
        state = toState;

        // calling the event
        if (onStateChange != null)
            onStateChange.Invoke(state);

        if (toState == UnitState.Idle)
        {
            navAgent.isStopped = true;
            navAgent.ResetPath();
        }
    }

    void Update()
    {
        switch (state)
        {
            case UnitState.Move:
            {
                MoveUpdate();
                break;
            }
            case UnitState.MoveToResource:
            {
                MoveToResourceUpdate();
                break;
            }
            case UnitState.Gather:
            {
                GatherUpdate();
                break;
            }
            default:
                break;
        }
    }

    // called every frame the 'Move' state is active
    void MoveUpdate()
    {
        if (Vector3.Distance(transform.position, navAgent.destination) == 0.0f)
            SetState(UnitState.Idle);
    }

    // called every frame the 'MoveToResource' state is active
    void MoveToResourceUpdate()
    {
        if (curResourceSource == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        if (Vector3.Distance(transform.position, navAgent.destination) == 0.0f)
            SetState(UnitState.Gather);
    }

    // called every frame the 'Gather' state is active
    void GatherUpdate()
    {
        if (curResourceSource == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        LookAt(curResourceSource.transform.position);

        if (Time.time - lastGatherTime > gatherRate)
        {
            lastGatherTime = Time.time;
            curResourceSource.GatherResource(gatherAmount, player);
        }
    }

    // moves the unit to a specific position
    public void MoveToPosition(Vector3 pos)
    {
        SetState(UnitState.Move);

        navAgent.isStopped = false;
        navAgent.SetDestination(pos);
    }

    // move to a resource and begin to gather it
    public void GatherResource(ResourceSource resource, Vector3 pos)
    {
        curResourceSource = resource;
        SetState(UnitState.MoveToResource);

        navAgent.isStopped = false;
        navAgent.SetDestination(pos);
    }

    // move to an enemy unit and attack it
    public void AttackUnit(Unit target)
    {

    }

    // toggles the selection ring around our feet
    public void ToggleSelectionVisual(bool selected)
    {
        selectionVisual.SetActive(selected);
    }

    // rotate to face the given position
    void LookAt(Vector3 pos)
    {
        Vector3 dir = (pos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
