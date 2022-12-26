using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    public static LevelSettings Instance { get; private set; }

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


    private static string[,,] map1 =
            {
                {
                    {"-","+"},
                    {"-","+"},
                },

        };

    private static string[,,] map2 =
         {
                {
                    {"-","+"},
                    {"-","+"},
                },
                 {
                    {"-","+"},
                    {"-","+"},
                },

        };

    private static string[,,] map3 =
         {
                {
                    {"-","+","-","+"},
                    {"-","+","-","+"},
                },
                 {
                    {"-","+","-","+"},
                    {"-","+","-","+"},
                },

        };


    private static Level actualLevel = Level.Level1;



    public enum Level
    {
        Level1,
        Level2,
        Level3
    }


    public void setLevel1()
    {
        actualLevel = Level.Level1;
    }

    public void setLevel2()
    {
        actualLevel = Level.Level2;
    }

    public void setLevel3()
    {
        actualLevel = Level.Level3;
    }

    public static string[,,] getMap()
    {
        switch (actualLevel)
        {
            case Level.Level1:
                return LevelSettings.map1;
            case Level.Level2:
                return LevelSettings.map2;
            case Level.Level3:
                return LevelSettings.map3;
            default:
                return LevelSettings.map1;

        }
    }
}
