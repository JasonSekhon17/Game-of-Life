using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SightSensor : MonoBehaviour
{
    public float radius;

    public SphereCollider sphereCollider;

    HashSet<GameObject> _targets = new HashSet<GameObject>();

    void Start() {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = radius;
    }

    public HashSet<GameObject> targets
    {
        get
        {
            _targets.RemoveWhere(IsNull);
            return _targets;
        }
    }

    static bool IsNull(GameObject g)
    {
        return (g == null || g.Equals(null));
    }

    void TryToAdd(GameObject other)
    {
        if(other.GetComponent<Organism>() || other.GetComponent<WaterNode>())
            _targets.Add(other);
    }

    void TryToRemove(GameObject other)
    {
        if(other.GetComponent<Organism>() || other.GetComponent<WaterNode>())
            _targets.Remove(other);
    }

    void OnTriggerEnter(Collider other)
    {
        TryToAdd(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        TryToRemove(other.gameObject);
    }
}
