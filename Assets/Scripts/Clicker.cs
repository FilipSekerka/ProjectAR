using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{

    public List<GameObject> objects;

    public GameObject rock;

    private int min_objects = 2;
    private int max_objects = 7;



    void Update()
    {
        //0 je lave tlacidlo na myske, pre telefon to chyta dotyk na obrazovku
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {

            RaycastHit hit; //toto je ten vystup, dostaneme to cez vstupno-vystupny parameter
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition); //ten luc je v nasom pripade urceny vlastne tou hlavnou kamerou - dalo by sa to aj takto GameObject.FindGameObjectsWithTag("")

            bool wasHit = Physics.Raycast(ray.origin, ray.direction.normalized, out hit);

            if (wasHit)
            {

                if (Input.GetMouseButtonDown(0))
                {
                    int object_id = Random.Range(0, objects.Count);
                    GameObject new_object = Instantiate(objects[object_id], transform.parent);
                    new_object.transform.position = hit.point;
                    Debug.DrawLine(ray.origin, hit.point, Color.red, 100.0f);
                }

                if (Input.GetMouseButtonDown(1))
                {
                    int number = Random.Range(min_objects, max_objects);
                    for (int i = 0; i < number; i++)
                    {
                        GameObject new_go = Instantiate(rock, transform.parent);
                        new_go.transform.position = hit.point + new Vector3(Random.Range(-0.05f, 0.05f), 0.3f + Random.Range(-0.1f, 0.1f), Random.Range(-0.05f, 0.05f));
                    }
                }



            }




            //Instantiate(tree, transform.parent); //chceme, aby tree bol potomkom toho trackera
            //Debug.Log("Instantiate Called");

        }
    }
}
