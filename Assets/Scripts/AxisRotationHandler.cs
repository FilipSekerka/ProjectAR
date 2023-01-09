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
        Main.Instance.doCompute();
    }

    public void rotateY() 
    {
        Main.Instance.selectedNode.rotateY();
        Main.Instance.doCompute();
    }

    public void rotateZ() 
    {
        Main.Instance.selectedNode.rotateZ();
        Main.Instance.doCompute();
    }
}
