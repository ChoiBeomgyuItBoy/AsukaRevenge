using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;
    private List<Target> targets = new List<Target>();

    public Target CurrentTarget { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent<Target>(out Target target)) { return; }

        targets.Add(target);

        target.OnDestroyed += RemoveTarget;
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.TryGetComponent<Target>(out Target target)) { return; }

        targets.Remove(target);

        RemoveTarget(target);
    }

    public bool SelectTarget()
    {
        if(targets.Count == 0) { return false; }

        CurrentTarget = targets[0];

        cinemachineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

        return true;
    }

    private void RemoveTarget(Target target)
    {
        if(CurrentTarget == target)
        {
            cinemachineTargetGroup.RemoveMember(CurrentTarget.transform); 
            CurrentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;

        targets.Remove(target);
    }

    public void Cancel()
    {
        if(CurrentTarget == null) { return; }

        cinemachineTargetGroup.RemoveMember(CurrentTarget.transform);

        CurrentTarget = null;
    }
}
