using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] List<Target> targets = new List<Target>();

    private void OnTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent<Target>(out Target target)) { return; }
        targets.Add(target);
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.TryGetComponent<Target>(out Target target)) { return; }
        targets.Remove(target);
    }
}
