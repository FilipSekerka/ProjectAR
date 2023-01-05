using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderCollider : MonoBehaviour
{
     void OnTriggerEnter(Collider collider)
    {

        Node parent = transform.parent.gameObject.GetComponent<Node>();
        Node otherParent = collider.transform.parent.gameObject.GetComponent<Node>();

        if (collider.gameObject.tag == "Stupel")
        {
            print($"trigger enter {parent.i}, {parent.j}, {parent.k}");

            parent.neighbours.Add(new Vector3(otherParent.i, otherParent.j, otherParent.k));
        } 
    }


    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Stupel")
        {

            Node parent = transform.parent.gameObject.GetComponent<Node>();
            Node otherParent = collider.transform.parent.gameObject.GetComponent<Node>();
            print($"trigger exit {parent.i}, {parent.j}, {parent.k}");
            

            // parent.neighbours.Dequeue();
        }
    }

    // void OnTriggerStay(Collider collider)
    // {
    //     if (collider.gameObject.tag == "Stupel")
    //     {

    //     }
    // }

}
