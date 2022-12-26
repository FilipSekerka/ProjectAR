using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;
using UnityEngine.EventSystems;

public class Main : MonoBehaviour
{


    public static Main Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
    }


    public GameObject marker;
    public GameObject linearPipePrefab;
    public GameObject bendPipePrefab;

    public float cubeSize = 0.025f;

    public Node[,,] nodes;

    public bool turnAroundY = true;

    public void onToggleClicked() { }


    public enum Orientations
    {
        North,
        South,
        East,
        West,
        Up,
        Down
    }

    private bool isValveOpen = false;
    private string[,,] map;
    private bool clickingIsLocked = false;
    private bool[,] lastVisited = new bool[1, 1];
    private bool colorThePipesInNextFrame = false;

    private int mapHeight = 0;
    private int mapRows = 0;
    private int mapCols = 0;

    void Start()
    {

        this.map = LevelSettings.getMap();

        this.mapHeight = this.map.GetLength(0);
        this.mapRows = this.map.GetLength(1);
        this.mapCols = this.map.GetLength(2);

        this.nodes = new Node[this.mapHeight, this.mapRows, this.mapCols];


        for (int i = 0; i < this.mapHeight; i++)
        {
            for (int j = 0; j < this.mapRows; j++)
            {
                for (int k = 0; k < this.mapCols; k++)
                {
                    string pipeType = map[i, j, k];

                    GameObject newObject = Instantiate((pipeType == "-") ? linearPipePrefab : bendPipePrefab, marker.transform);
                    newObject.transform.position = new Vector3((cubeSize * j), (cubeSize * i) + (cubeSize / 2.0f), -(cubeSize * k));

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
                    newObject.GetComponent<Node>().k = k;
                    nodes[i, j, k] = newObject.GetComponent<Node>();
                }
            }

        }

        //resetWaterFlow();
    }

    Vector3 calibrateEulerAngles(Vector3 angles)
    {
        Vector3 newAngles = Vector3.zero;

        for (int i = 0; i < 3; i++)
        {
            if (angles[i] > 80.0 && angles[i] < 100.0f)
            {
                newAngles[i] = 90.0f;
            }
            else if (angles[i] > 170.0f && angles[i] < 190.0f)
            {
                newAngles[i] = 180.0f;
            }
            else if (angles[i] > 260.0f && angles[i] < 280.0f)
            {
                newAngles[i] = 270.0f;
            }
            else if (angles[i] > 350.0f && angles[i] <= 360.0f)
            {
                newAngles[i] = 0.0f;
            }
            else if (angles[i] >= 0.0f && angles[i] < 10.0f)
            {
                newAngles[i] = 0.0f;
            }

        }

        return newAngles;
    }


    void Update()
    {

        if (this.colorThePipesInNextFrame)
        {
            //ColorThePipes();
            this.colorThePipesInNextFrame = false;
            this.clickingIsLocked = false;
        }

        if (Input.GetMouseButtonDown(0))
        {

            if (this.clickingIsLocked)
            {
                Debug.Log("Background computation in progress");
                return;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("kliknutie na GUI");
                return;
            }


            RaycastHit hit;
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            bool wasHit = Physics.Raycast(ray.origin, ray.direction.normalized, out hit);

            if (wasHit)
            {
                GameObject hitGameObject = hit.transform.gameObject;

                //resetWaterFlow();
                if (hitGameObject.tag == "UI") //Tento UI tag premenovat, lebo nie je to UI
                {
                    hitGameObject.transform.Rotate(new Vector3(0, 90.0f, 0));
                    isValveOpen = !isValveOpen;

                    //this.clickingIsLocked = true;
                    //Thread t = new Thread(compute);
                    //t.Start();
                }

                if (hitGameObject.tag == "Pipes")
                {
                    //Debug.Log("parent rotation: " + hitGameObject.transform.localEulerAngles);
                    //Debug.Log("container rotation: " + hitGameObject.transform.GetChild(0).localEulerAngles);
                    //Debug.Log("pipe rotation: " + hitGameObject.transform.GetChild(0).GetChild(0).localEulerAngles);

                    //Debug.Log("container rotaion before calibration: " + hitGameObject.transform.GetChild(0).localEulerAngles);
                    //hitGameObject.transform.GetChild(0).localEulerAngles = calibrateEulerAngles(hitGameObject.transform.GetChild(0).localEulerAngles);
                    //Debug.Log("container rotaion after calibration: " + hitGameObject.transform.GetChild(0).localEulerAngles);


                    if (this.turnAroundY)
                    {
                        Vector3 parentPosition = hitGameObject.transform.position;
                        hitGameObject.transform.GetChild(0).RotateAround(parentPosition, new Vector3(0, 1, 0), 90.0f);
                        hitGameObject.GetComponent<Node>().turnAroundYAxis();
                    }
                    else
                    {
                        Vector3 parentPosition = hitGameObject.transform.position;
                        hitGameObject.transform.GetChild(0).RotateAround(parentPosition, new Vector3(1, 0, 0), 90.0f);
                        hitGameObject.GetComponent<Node>().turnAroundXAxis();
                    }


                    hitGameObject.transform.GetChild(0).localEulerAngles = calibrateEulerAngles(hitGameObject.transform.GetChild(0).localEulerAngles);


                    if (isValveOpen)
                    {

                        this.clickingIsLocked = true;
                        //Thread t = new Thread(compute);
                        //t.Start();
                    }
                }


            }
        }
    }

    //private void compute()
    //{
    //    Debug.Log("Computation started.");
    //    checkPath();
    //    Debug.Log("Computation finished.");

    //}


    //void checkPath()
    //{

    //    bool[,] visited = new bool[this.mapSize, this.mapSize];

    //    bool result = DFSUtil(0, 0, visited);

    //    this.lastVisited = visited;
    //    this.colorThePipesInNextFrame = true;

    //    Debug.Log("vysledok prehladavania: " + result);


    //}

    //void ColorThePipes()
    //{
    //    for (var i = 0; i < this.lastVisited.GetLength(0); i++)
    //    {
    //        for (var j = 0; j < this.lastVisited.GetLength(1); j++)
    //        {
    //            if (this.lastVisited[i, j])
    //            {
    //                if (this.isValveOpen)
    //                {
    //                    this.nodes[i, j].setBlueMaterial();
    //                }
    //            }
    //        }
    //    }
    //}

    //bool DFSUtil(int i, int j, bool[,] visited)
    //{

    //    visited[i, j] = true;
    //    //Debug.Log("visited: (" + i + ", " + j + ")");

    //    if ((i == (mapSize - 1)) && (j == (mapSize - 1)))
    //    {
    //        return true;
    //    }



    //    List<List<int>> neighbours = getNeighbours(i, j);

    //    foreach (var n in neighbours)
    //    {
    //        int neighbour_i = n[0];
    //        int neighbour_j = n[1];

    //        if (visited[neighbour_i, neighbour_j] == false)
    //        {
    //            bool result = DFSUtil(neighbour_i, neighbour_j, visited);
    //            if (result == true)
    //            {
    //                return true;
    //            }
    //        }
    //    }



    //    return false;
    //}

    //Orientations getAdjacentOrientation(Orientations originOrientantion)
    //{
    //    switch (originOrientantion)
    //    {
    //        case Orientations.West:
    //            return Orientations.East;
    //        case Orientations.East:
    //            return Orientations.West;
    //        case Orientations.North:
    //            return Orientations.South;
    //        case Orientations.South:
    //            return Orientations.North;
    //        case Orientations.Up:
    //            return Orientations.Down;
    //        case Orientations.Down:
    //            return Orientations.Up;
    //        default:
    //            Debug.Log("Unknown orientation");
    //            return Orientations.North;
    //    }
    //}

    //List<List<int>> getNeighbours(int i, int j)
    //{
    //    List<List<int>> neighbours = new List<List<int>>();


    //    Node actualNode = this.nodes[i, j];

    //    if (j - 1 >= 0)
    //    {
    //        Node westNeighbour = this.nodes[i, j - 1];
    //        if (actualNode.orientation.Contains(Orientations.West) && westNeighbour.orientation.Contains(getAdjacentOrientation(Orientations.West)))
    //        {
    //            neighbours.Add(new List<int> { i, j - 1 });
    //        }
    //    }


    //    if (j + 1 < this.mapSize)
    //    {
    //        Node eastNeighbour = this.nodes[i, j + 1];
    //        if (actualNode.orientation.Contains(Orientations.East) && eastNeighbour.orientation.Contains(getAdjacentOrientation(Orientations.East)))
    //        {
    //            neighbours.Add(new List<int> { i, j + 1 });
    //        }
    //    }

    //    if (i + 1 < this.mapSize)
    //    {
    //        Node southNeighbour = this.nodes[i + 1, j];
    //        if (actualNode.orientation.Contains(Orientations.South) && southNeighbour.orientation.Contains(getAdjacentOrientation(Orientations.South)))
    //        {
    //            neighbours.Add(new List<int> { i + 1, j });
    //        }
    //    }

    //    if (i - 1 >= 0)
    //    {
    //        Node northNeighbour = this.nodes[i - 1, j];
    //        if (actualNode.orientation.Contains(Orientations.North) && northNeighbour.orientation.Contains(getAdjacentOrientation(Orientations.North)))
    //        {
    //            neighbours.Add(new List<int> { i - 1, j });
    //        }
    //    }

    //    return neighbours;
    //}

    //void resetWaterFlow()
    //{
    //    for (int i = 0; i < this.map.Length; i++)
    //    {
    //        var rowLength = this.map[i].Split(" ").Length;
    //        for (int j = 0; j < rowLength; j++)
    //        {

    //            if (i == 0 && j == 0)
    //            {
    //                this.nodes[i, j].GetComponent<Node>().setBlueMaterial();
    //            }
    //            else if (i == this.map.Length - 1 && j == rowLength - 1)
    //            {
    //                this.nodes[i, j].GetComponent<Node>().setOutputMaterial();
    //            }
    //            else
    //            {
    //                this.nodes[i, j].GetComponent<Node>().setWhiteMaterial();
    //            }
    //        }
    //    }
    //}

}
