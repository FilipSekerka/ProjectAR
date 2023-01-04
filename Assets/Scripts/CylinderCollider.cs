using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderCollider : MonoBehaviour
{
     void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Stupel")
        {
            Node parent = transform.parent.gameObject.GetComponent<Node>();
            Node otherParent = collider.transform.parent.gameObject.GetComponent<Node>();

            // nastavis zoznam susedov
            parent.neighbours.Add(new Vector3(otherParent.i, otherParent.j, otherParent.k));
        }
    }


    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Stupel")
        {
            Node parent = transform.parent.gameObject.GetComponent<Node>();
            Node otherParent = collider.transform.parent.gameObject.GetComponent<Node>();
            
            // parent.neighbours.Remove
            parent.neighbours.RemoveWhere(n => n[0] == otherParent.i && n[1] == otherParent.j && n[2] == otherParent.k);
        }
    }
}
