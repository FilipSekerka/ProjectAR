using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelSettings : MonoBehaviour
{
    public static LevelSettings Instance { get; private set; }
    private static List<List<List<string[]>>> maps = new List<List<List<string[]>>>();

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


    private static int actualLevel = 0;

    public void setLevel(int level) 
    {
        actualLevel = level;
    }

    public List<List<string[]>> getMap()
    {
        return LevelSettings.maps[actualLevel];
    }

    public List<List<List<string[]>>> loadMaps()
    {
        maps = new List<List<List<string[]>>>();
        var fileEntries = Resources.LoadAll("Maps");
        
        foreach(var fileEntry in fileEntries) 
        {
            TextAsset mapContents = Resources.Load<TextAsset>($"Maps/{fileEntry.name}");
            string[] mapLines = mapContents.text.Split("\n");

            List<List<string[]>> map = new List<List<string[]>>();
            List<string[]> floor = new List<string[]>();

            foreach (string line in mapLines)
            {  
                string[] row = line.Split(" ");
                
                if (row.GetValue(0).ToString() != ";")
                {
                    floor.Add(row);
                } else {
                    map.Add(floor);
                    floor = new List<string[]>();
                }
            }  

            maps.Add(map);
        }
        return maps;
    }
}
