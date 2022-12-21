using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static Main;

public class Node : MonoBehaviour
{

    public List<Orientations> orientation;
    public String pipeType;
    public int i;
    public int j;
    public int k;

    public Material blueMaterial;
    public Material whiteMaterial;
    public Material outputMaterial;

    private Color pipeColor;

    //void Update()
    //{
    //    float distanceFromCamera = getDistanceFromCamera();
    //    pipeColor = transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color;
    //    pipeColor.a = Mathf.Pow(distanceFromCamera, 1.2f);
    //    transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = pipeColor;
    //}

    //public void setBlueMaterial()
    //{
    //    transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Renderer>().material = blueMaterial;
    //}

    //public void setWhiteMaterial()
    //{
    //    transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Renderer>().material = whiteMaterial;
    //}

    //public void setOutputMaterial()
    //{
    //    transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Renderer>().material = outputMaterial;
    //}


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

    public void turnAroundYAxis()
    {
        // Debug.Log("old orientation: " + this.orientation[0] + ", " + this.orientation[1]);
        this.orientation = new List<Orientations>() {
                        getNewDirection(this.orientation[0],true),
                        getNewDirection(this.orientation[1],true),
                        };
        // Debug.Log("new orientation: " + this.orientation[0] + ", " + this.orientation[1]);
    }

    public void turnAroundXAxis()
    {
        this.orientation = new List<Orientations>() {
                        getNewDirection(this.orientation[0],false),
                        getNewDirection(this.orientation[1],false),
                        };
    }

    public float getDistanceFromCamera()
    {
        return Vector3.Distance(Camera.main.transform.position, transform.position);
    }
}

