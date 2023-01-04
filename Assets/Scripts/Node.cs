using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static Main;

public class Node : MonoBehaviour
{

    public HashSet<Vector3> neighbours = new HashSet<Vector3>();
    public List<Orientations> orientation;
    public String pipeType;
    public int i;
    public int j;
    public int k;

    public Material blueMaterial;
    public Material whiteMaterial;
    public Material outputMaterial;

    private Color pipeColor;

    public bool isSelected;

    void Update()
    {

        float distanceFromCamera = getDistanceFromCamera();
        pipeColor = transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color;

        if (distanceFromCamera < 0.4f)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        } else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            pipeColor.a = Mathf.Pow(distanceFromCamera, 2.0f);
        }
        transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = pipeColor;
    }

    public void setBlueMaterial()
    {
        print("coloring pipes");
       transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Renderer>().material = blueMaterial;
    }

    public void setWhiteMaterial()
    {
       transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Renderer>().material = whiteMaterial;
    }

    public void setOutputMaterial()
    {
       transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Renderer>().material = outputMaterial;
    }

    public void select()
    {   
        this.isSelected = true;
        transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Renderer>().material = outputMaterial;
    }

    public void unselect()
    {
        this.isSelected = false;
        transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Renderer>().material = whiteMaterial;
    }

    public Orientations getNewDirection(Orientations originOrientation, bool turnAroundYAxis)
    {
        if (turnAroundYAxis)
        {
            switch (originOrientation)
            {
                case Orientations.North:
                    return Orientations.East;
                case Orientations.East:
                    return Orientations.South;
                case Orientations.South:
                    return Orientations.West;
                case Orientations.West:
                    return Orientations.North;
                case Orientations.Up:
                    return Orientations.Up;
                case Orientations.Down:
                    return Orientations.Down;
                default:
                    Debug.Log("Unknown orientation");
                    return Orientations.North;
            }
        }
        else
        {
            switch (originOrientation)
            {
                case Orientations.North:
                    return Orientations.Up;
                case Orientations.Up:
                    return Orientations.South;
                case Orientations.South:
                    return Orientations.Down;
                case Orientations.Down:
                    return Orientations.North;
                case Orientations.East:
                    return Orientations.East;
                case Orientations.West:
                    return Orientations.West;
                default:
                    Debug.Log("Unknown orientation");
                    return Orientations.North;
            }
        }
    }

    public void rotateX()
    { 
        transform.Rotate(new Vector3(90.0f, 0, 0));
    }

    public void rotateY()
    { 
        transform.Rotate(new Vector3(0, 90.0f, 0));
    }

    public void rotateZ()
    { 
        transform.Rotate(new Vector3(0, 0, 90.0f));

    }

    public void turnAroundYAxis()
    {
        // Debug.Log("old orientation: " + this.orientation[0] + ", " + this.orientation[1]);
        this.orientation = new List<Orientations>() {
                        getNewDirection(this.orientation[0],true),
                        getNewDirection(this.orientation[1],true),
                        };
        Debug.Log("new orientation: " + this.orientation[0] + ", " + this.orientation[1]);
    }

    public void turnAroundXAxis()
    {
        this.orientation = new List<Orientations>() {
                        getNewDirection(this.orientation[0],false),
                        getNewDirection(this.orientation[1],false),
                        };
        Debug.Log("new orientation: " + this.orientation[0] + ", " + this.orientation[1]);
    }

    public float getDistanceFromCamera()
    {
        return Vector3.Distance(Camera.main.transform.position, transform.position);
    }

   
}

