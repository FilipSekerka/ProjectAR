using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Main : MonoBehaviour
{

    public GameObject marker;
    public GameObject linearPipePrefab;
    public GameObject bendPipePrefab;

    public float cubeSize = 0.025f;
    // Start is called before the first frame update
    void Start()
    {
        string file = "Assets/Maps/map.txt";

        if (File.Exists(file))
        {
            Debug.Log("MFile exists.");
        }
        else {
            Debug.Log("File does not exist.");
        }

       // string[] map = File.ReadAllLines(file);

        string[] map = {"- - - - - - +",
                        "- + - + + - -",
                        "- - - + - - -",
                        "- - - - - - -",
                        "- + - - - + -",
                        "- - - - + - -",
                        "- - - + + - +"};


        for (var i = 0; i < map.Length; i++)
        {
            for (var j = 0; j < map[i].Split(" ").Length; j++) {
                GameObject newObject = Instantiate((map[i].Split(" ")[j] == "-") ? linearPipePrefab : bendPipePrefab, marker.transform);
                newObject.transform.position = new Vector3(cubeSize * i, cubeSize/2.0f, cubeSize * j);
            }
        }
    }


    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            bool wasHit = Physics.Raycast(ray.origin, ray.direction.normalized, out hit);

            if (wasHit)
            {
                GameObject hitGameObject = hit.transform.gameObject;
                if (hitGameObject.tag == "Pipes")
                {
                    hitGameObject.transform.Rotate(new Vector3(0, 90.0f, 0));
                }


            }
        }
    }
}
