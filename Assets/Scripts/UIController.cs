using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public void onToggleClicked()
    {
        Main.Instance.turnAroundY = !Main.Instance.turnAroundY;
    }
}
