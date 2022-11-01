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

    public int mapSize;
    public Node[,] nodes;


    public enum Orientations
    {
        North,
        South,
        East,
        West,
        Up,
        Down
    }


    void Start()
    {
        /* string file = "Assets/Maps/map.txt";

         if (File.Exists(file))
         {
             Debug.Log("MFile exists.");
         }
         else
         {
             Debug.Log("File does not exist.");
         }

         string[] map = File.ReadAllLines(file);
        */

        //string[] map = {"- - - - - - +",
        //                "- + - + + - -",
        //                "- - - + - - -",
        //                "- - - - - - -",
        //                "- + - - - + -",
        //                "- - - - + - -",
        //                "- - - + + - +"};

        string[] map = {"- + - +",
                        "- - - -",
                        "- - - -",
                        "- + - -"};

        float offset = (map.Length * cubeSize) / 2.0f;

        this.mapSize = map.Length;

        this.nodes = new Node[map.Length, map.Length];


        for (var i = 0; i < map.Length; i++)
        {
            for (var j = 0; j < map[i].Split(" ").Length; j++)
            {
                var pipeType = map[i].Split(" ")[j];
                GameObject newObject = Instantiate((pipeType == "-") ? linearPipePrefab : bendPipePrefab, marker.transform);
                newObject.transform.position = new Vector3((cubeSize * j), cubeSize / 2.0f, -(cubeSize * i));


                if (pipeType == "-")
                {
                    newObject.GetComponent<Node>().orientation = new List<Orientations>() { Orientations.West, Orientations.East };
                    newObject.GetComponent<Node>().pipeType = "-";
                }
                else if (pipeType == "+")
                {
                    newObject.GetComponent<Node>().orientation = new List<Orientations>() { Orientations.East, Orientations.South };
                    newObject.GetComponent<Node>().pipeType = "+";
                }

                newObject.GetComponent<Node>().i = i;
                newObject.GetComponent<Node>().j = j;
                nodes[i, j] = newObject.GetComponent<Node>();
            }
        }
    }


    void Update()
    {

        //otacanie
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            bool wasHit = Physics.Raycast(ray.origin, ray.direction.normalized, out hit);

            


            if (wasHit)
            {
                GameObject hitGameObject = hit.transform.gameObject;

                if (hitGameObject.tag == "UI")
                {
                    hitGameObject.transform.Rotate(new Vector3(0, 90.0f, 0));
                    checkPath();
                }

                if (hitGameObject.tag == "Pipes")
                {
                    hitGameObject.transform.Rotate(new Vector3(0, 90.0f, 0));
                    hitGameObject.GetComponent<Node>().turnAroundYAxis();
                    //Debug.Log("suradnice pipe-y: " + hitGameObject.GetComponent<Node>().i + ", " + hitGameObject.GetComponent<Node>().j);
                }


            }
        }
    }


    void checkPath()
    {

        bool[,] visited = new bool[this.mapSize, this.mapSize];

        bool result = DFSUtil(0, 0, visited);

        Debug.Log("vysledok prehladavania: " + result);

        for (var i = 0; i < this.mapSize; i++)
        {
            for (var j = 0; j < this.mapSize; j++)
            {
                if (visited[i, j])
                {
                    this.nodes[i, j].setBlueMaterial();
                }
            }
        }


    }

    bool DFSUtil(int i, int j, bool[,] visited)
    {

        visited[i, j] = true;
        Debug.Log("visited: (" + i + ", " + j + ")");

        if ((i == (mapSize - 1)) && (j == (mapSize - 1)))
        {
            return true;
        }

        

        List<List<int>> neighbours = getNeighbours(i, j);

        foreach (var n in neighbours)
        {
            int neighbour_i = n[0];
            int neighbour_j = n[1];

            if (visited[neighbour_i, neighbour_j] == false)
            {
                bool result = DFSUtil(neighbour_i, neighbour_j, visited);
                if (result == true)
                {
                    return true;
                }
            }
        }



        return false;
    }

    Orientations getAdjacentOrientation(Orientations originOrientantion)
    {
        switch (originOrientantion)
        {
            case Orientations.West:
                return Orientations.East;
            case Orientations.East:
                return Orientations.West;
            case Orientations.North:
                return Orientations.South;
            case Orientations.South:
                return Orientations.North;
            case Orientations.Up:
                return Orientations.Down;
            case Orientations.Down:
                return Orientations.Up;
            default:
                Debug.Log("Unknown orientation");
                return Orientations.North;
        }
    }

    List<List<int>> getNeighbours(int i, int j)
    {
        List<List<int>> neighbours = new List<List<int>>();


        Node actualNode = this.nodes[i, j];

        if (j - 1 >= 0)
        {
            Node westNeighbour = this.nodes[i, j - 1];
            if (actualNode.orientation.Contains(Orientations.West) && westNeighbour.orientation.Contains(getAdjacentOrientation(Orientations.West)))
            {
                neighbours.Add(new List<int> { i, j - 1 });
            }
        }


        if (j + 1 < this.mapSize)
        {
            Node eastNeighbour = this.nodes[i, j + 1];
            if (actualNode.orientation.Contains(Orientations.East) && eastNeighbour.orientation.Contains(getAdjacentOrientation(Orientations.East)))
            {
                neighbours.Add(new List<int> { i, j + 1 });
            }
        }

        if (i + 1 < this.mapSize)
        {
            Node southNeighbour = this.nodes[i + 1, j];
            if (actualNode.orientation.Contains(Orientations.South) && southNeighbour.orientation.Contains(getAdjacentOrientation(Orientations.South)))
            {
                neighbours.Add(new List<int> { i + 1, j });
            }
        }

        if (i - 1 >= 0)
        {
            Node northNeighbour = this.nodes[i - 1, j];
            if (actualNode.orientation.Contains(Orientations.North) && northNeighbour.orientation.Contains(getAdjacentOrientation(Orientations.North)))
            {
                neighbours.Add(new List<int> { i - 1, j });
            }
        }

        return neighbours;
    }

}
