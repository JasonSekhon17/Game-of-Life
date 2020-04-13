using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSensor : MonoBehaviour
{
    public SphereCollider sphereCollider;

    HashSet<Plant> _targets = new HashSet<Plant>();

    void Start() {
        sphereCollider = GetComponent<SphereCollider>();
    }

    public HashSet<Plant> targets
    {
        get
        {
            _targets.RemoveWhere(IsNull);
            return _targets;
        }
    }

    static bool IsNull(Plant p)
    {
        return (p == null || p.Equals(null));
    }

    void TryToAdd(Component other)
    {
        Plant p = other.GetComponent<Plant>();
        if (p != null)
        {
            _targets.Add(p);
        }
    }

    void TryToRemove(Component other)
    {
        Plant p = other.GetComponent<Plant>();
        if (p != null)
        {
            _targets.Remove(p);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        TryToAdd(other);
    }

    void OnTriggerExit(Collider other)
    {
        TryToRemove(other);
    }
}
