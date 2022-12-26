using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    public void onToggleClicked()
    {
        Main.Instance.turnAroundY = !Main.Instance.turnAroundY;
    }

    public void goToLevel1()
    {
        LevelSettings.Instance.setLevel1();
        SceneManager.LoadScene(1);
    }

    public void goToLevel2()
    {
        LevelSettings.Instance.setLevel2();
        SceneManager.LoadScene(1);
    }

    public void goToLevel3()
    {
        LevelSettings.Instance.setLevel3();
        SceneManager.LoadScene(1);
    }

    public void goBackToMenu()
    {
        SceneManager.LoadScene(0);
        
    }
}
