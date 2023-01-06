using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static Main;

public class Node : MonoBehaviour
{

    public List<Vector3> neighbours = new List<Vector3>();
    public List<Orientations> orientation;
    public String pipeType;
    public int i;
    public int j;
    public int k;

    public Material waterMaterial;
    public Material whiteMaterial;
    public Material outputMaterial;
    public Material selectedMaterial;
    public Material transparentMaterial;

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

    public void setWaterMaterial()
    {
       transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Renderer>().material = waterMaterial;
    }

    public void setWhiteMaterial()
    {
       transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Renderer>().material = whiteMaterial;
    }

    public void setOutputMaterial()
    {
       transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Renderer>().material = outputMaterial;
    }

    public void setSelectedMaterial()
    {
        transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Renderer>().material = selectedMaterial;
    }

    public void setTransparentMaterial()
    {
        transform.gameObject.GetComponent<Renderer>().material = transparentMaterial;
    }
    public void select()
    {   
        this.isSelected = true;
        setSelectedMaterial();
    }

    public void unselect()
    {
        this.isSelected = false;
        setWhiteMaterial();
    }

    public List<Vector3> getNeighbours()
    {
        List<Collider> nA = transform.GetChild(1).gameObject.GetComponent<CylinderCollider>().GetColliders();
        List<Collider> nB = transform.GetChild(2).gameObject.GetComponent<CylinderCollider>().GetColliders();

        List<Vector3> neighbours = new List<Vector3>();
        foreach(Collider collider in nA)
        {
            Node parentNode = collider.transform.parent.gameObject.GetComponent<Node>();
            neighbours.Add(new Vector3(parentNode.i, parentNode.j, parentNode.k));
        }
        foreach(Collider collider in nB)
        {
            Node parentNode = collider.transform.parent.gameObject.GetComponent<Node>();
            neighbours.Add(new Vector3(parentNode.i, parentNode.j, parentNode.k));
        }

        return neighbours;
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

    public float getDistanceFromCamera()
    {
        return Vector3.Distance(Camera.main.transform.position, transform.position);
    }

   
}

