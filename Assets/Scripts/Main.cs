using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    public GameObject planePrefab;
    public GameObject linearPipePrefab;
    public GameObject bendPipePrefab;

    public float cubeSize;
    // Start is called before the first frame update
    void Start()
    {
        // string[] map = System.IO.File.ReadAllLines(@"C:\Users\Public\projects\map.txt");

        string[] map = {"- - - - - - +",
                        "- + - + + - -",
                        "- - - + - - -",
                        "- - - - - - -",
                        "- + - - - + -",
                        "- - - - + - -",
                        "- - - + + - +"};

        int rows = map.Length;
        int cols = map[0].Length;

        float planeWidth = rows * cubeSize;
        float planeHeight = cols * cubeSize;

        // planePrefab.transform.localScale = new Vector3(planeWidth, planeWidth, planeWidth);
        // RectTransform rt = (RectTransform)planePrefab.transform;

        // float planeWidth = rt.rect.width;
        // float planeHeight = rt.rect.height;

        float startX = planePrefab.transform.position.x - planeWidth;
        float startZ = planePrefab.transform.position.z - planeHeight;

        for (var i = 0; i < map.Length; i++)
        {
            for (var j = 0; j < map[i].Split(" ").Length; j++) {
                GameObject newObject = Instantiate((map[i].Split(" ")[j] == "-") ? linearPipePrefab : bendPipePrefab, planePrefab.transform);
                newObject.transform.position = new Vector3(0.025f * i, 0.0125f, 0.025f * j);
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
                    //TODO zmeni sa orientacia
                    hitGameObject.GetComponent<HorizontalPipe>().inputOrientation = 5;
                }


            }
        }
    }
}
