using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class UIController : MonoBehaviour
{
    public static UIController Instance {get; private set;}
    public GameObject levelButtonPrefab;
    public GameObject buttonPanel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        
        }
        else
        {
            Destroy(gameObject);
        }
        

    }
    void Start()
    {
        var maps = LevelSettings.Instance.loadMaps();
        for (var i = 0; i < maps.Count; i++)
        {
            GameObject button = Instantiate(levelButtonPrefab);

            button.transform.SetParent(buttonPanel.transform);

            // prepata kopia
            var iCopy = i;
            button.GetComponent<Button>().onClick.AddListener(delegate{goToLevel(iCopy);});

            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Level {i}";
        }
    }
    public void onToggleClicked()
    {
    }

    public void goToLevel(int level)
    {
        LevelSettings.Instance.setLevel(level);
        SceneManager.LoadScene(1);
    }


    public void goBackToMenu()
    {
        SceneManager.LoadScene(0);
        
    }
}
