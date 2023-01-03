using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class AxisRotationHandler : MonoBehaviour
{
    
    public void rotateX() 
    {
        Main.Instance.selectedNode.rotateX();
    }

    public void rotateY() 
    {
        Main.Instance.selectedNode.rotateY();
    }

    public void rotateZ() 
    {
        Main.Instance.selectedNode.rotateZ();
       
    }

    public void handle(Dropdown change) {
        return;
    }
}
