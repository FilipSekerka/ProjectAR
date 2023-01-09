using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;
using UnityEngine.EventSystems;
using System.Linq;

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

    public Node selectedNode = null;

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
    private List<List<string[]>> map;
    private bool clickingIsLocked = false;
    private bool[,,] lastVisited = new bool[1, 1, 1];
    private bool colorThePipesInNextFrame = false;

    private int mapHeight = 0;
    private int mapRows = 0;
    private int mapCols = 0;
    private bool isGameover = false;

    void Start()
    {
        this.map = LevelSettings.Instance.getMap();
        this.mapHeight = this.map.Count;
        this.mapRows = this.map[0].Count;
        this.mapCols = this.map[0][0].Length;
        this.nodes = new Node[this.mapHeight, this.mapRows, this.mapCols];


        for (int i = 0; i < this.mapHeight; i++)
        {
            for (int j = 0; j < this.mapRows; j++)
            {
                for (int k = 0; k < this.mapCols; k++)
                {
                    string pipeType = map[i][j][k];
                    
                    GameObject newObject = null;
                    if (pipeType == "-")
                    {
                        newObject = Instantiate(linearPipePrefab, marker.transform);
                        newObject.transform.position = new Vector3((cubeSize * k), (cubeSize * i) + (cubeSize / 2.0f), -(cubeSize * j));
                    }
                    else if (pipeType == "+")
                    {
                        newObject = Instantiate(bendPipePrefab, marker.transform);
                        newObject.transform.position = new Vector3((cubeSize * k), (cubeSize * i) + (cubeSize / 2.0f), -(cubeSize * j));
                    } else if (pipeType == "?") 
                    {
                        continue;
                    }

                    newObject.GetComponent<Node>().i = i;
                    newObject.GetComponent<Node>().j = j;
                    newObject.GetComponent<Node>().k = k;
                    nodes[i, j, k] = newObject.GetComponent<Node>();
                }
            }

        }

        resetWaterFlow();
    }


    void Update()
    {

        if (this.colorThePipesInNextFrame)
        {
            ColorThePipes();
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

            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return;
            }


            RaycastHit[] hits;
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            hits = Physics.RaycastAll(ray.origin, ray.direction.normalized, 100.0F);

            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject.tag ==  "UI")
                {
                    return;
                }
            }
            
            RaycastHit[] activeHits = Array.FindAll(hits, hit => hit.transform.GetChild(0).gameObject.activeSelf);
            activeHits.OrderByDescending(hit => hit.transform.gameObject.GetComponent<Node>().getDistanceFromCamera());
            if (activeHits.Length > 0)
            {

               
                RaycastHit hit = activeHits.First();
                GameObject hitGameObject = hit.transform.gameObject;

                if (hitGameObject.tag == "Valve")
                {
                    hitGameObject.transform.Rotate(new Vector3(0, 90.0f, 0));
                    isValveOpen = !isValveOpen;

                    if (isValveOpen) {
                        doCompute();
                    }
                    else {
                        resetWaterFlow();
                    }
                }

                if (hitGameObject.tag == "Pipes")
                {
                    if (isValveOpen) {
                        return;
                    }
                    if (this.selectedNode != null)
                    {
                        this.selectedNode.unselect();
                    }

                    Node clickedNode = hitGameObject.GetComponent<Node>();
                    if (!clickedNode.isSelected)
                    {
                        clickedNode.select();
                        this.selectedNode = clickedNode;
                    }

                }

            }

        }
    }

    public void doCompute() 
    {
        if (this.clickingIsLocked)
        {
            return;
        }
        resetWaterFlow();

        if (isValveOpen)
        {
            setAllNeighbours();
            this.clickingIsLocked = true;
            Thread t = new Thread(compute);
            t.Start();
        }
        
    }

    private void compute()
    {
        checkPath();

    }


    void checkPath()
    {

        bool[,,] visited = new bool[this.mapHeight, this.mapRows, this.mapCols];

        bool result = DFSUtil(0, 0, 0,visited);

        this.isGameover = result;
        this.lastVisited = visited;
        this.colorThePipesInNextFrame = true;
    }

    void ColorThePipes()
    {
        for (var i = 0; i < this.mapHeight; i++)
        {
            for (var j = 0; j < this.mapRows; j++)
            {
                for (var k = 0; k < this.mapCols; k++)
                {
                    if (nodes[i, j, k] == null)
                    {
                        continue;
                    }
                    if (this.lastVisited[i, j, k])
                    {
                        if (this.isValveOpen)
                        {
                            this.nodes[i, j, k].setWaterMaterial();
                        }
                    } else if (this.isGameover)
                    {
                        this.nodes[i,j,k].setGravity();
                    }
                }

            }
        }
    }

    bool DFSUtil(int i, int j, int k, bool[,,] visited)
    {

        visited[i, j, k] = true;

        if ((i == (mapHeight - 1)) && (j == (mapRows - 1)) && (k == (mapCols - 1)))
        {
            return true;
        }

        List<Vector3> neighbours = nodes[i, j, k].neighbours;

        foreach (var n in neighbours)
        {
            int neighbour_i = Convert.ToInt32(n[0]);
            int neighbour_j = Convert.ToInt32(n[1]);
            int neighbour_k = Convert.ToInt32(n[2]);

            if (visited[neighbour_i, neighbour_j, neighbour_k] == false)
            {
                bool result = DFSUtil(neighbour_i, neighbour_j, neighbour_k, visited);
                if (result == true)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void resetWaterFlow()
    {
        for (int i = 0; i < this.mapHeight; i++)
        {
            for (int j = 0; j < this.mapRows; j++)
            {
                for (int k = 0; k < this.mapCols; k++)
                {
                    if (nodes[i, j, k] == null)
                    {
                        continue;
                    }
                    if (this.selectedNode != null && i == this.selectedNode.i && j == this.selectedNode.j && k == this.selectedNode.k)
                    {
                        this.nodes[i, j, k].GetComponent<Node>().setSelectedMaterial();
                    }
                    else if (i == this.mapHeight - 1 && j == this.mapRows - 1 && k == this.mapCols - 1)
                    {
                        this.nodes[i, j, k].GetComponent<Node>().setOutputMaterial();
                    }
                    else
                    {
                        this.nodes[i, j, k].GetComponent<Node>().setWhiteMaterial();
                    }
                }


            }
        }
    }

    private void setAllNeighbours()
    {
        for (int i = 0; i < this.mapHeight; i++)
        {
            for (int j = 0; j < this.mapRows; j++)
            {
                for (int k = 0; k < this.mapCols; k++)
                {
                    if (nodes[i,j,k] != null) 
                    {
                        List<Vector3> nodeNeighbours = nodes[i, j, k].getNeighbours();
                        nodes[i, j, k].neighbours = nodeNeighbours;
                    }
                   
                }
            }
        }
    }

}
