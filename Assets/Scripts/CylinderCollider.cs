using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderCollider : MonoBehaviour
{
     private List<Collider> colliders = new List<Collider>();
     public List<Collider> GetColliders () { return colliders; }

     void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Stupel")
        {
            if (!colliders.Contains(collider)) { colliders.Add(collider); }
        }
    }


    void OnTriggerExit(Collider collider)
    {
        colliders.Remove(collider);
    }



}
