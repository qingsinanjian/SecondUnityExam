using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Save
{
    //ÕÊº“ Ù–‘
    public float playerXPos;
    public float playerYPos;
    public float playerZPos;

    public float playerXRot;
    public float playerYRot;
    public float playerZRot;
    public float playerWRot;
    public int currentHP;
    public int MaxHP;
    public int currentMP;
    public int MaxMP;

    //µ–»À
    public int enemyCount;
    public List<float> enemyXPosList = new List<float>();
    public List<float> enemyYPosList = new List<float>();
    public List<float> enemyZPosList = new List<float>();
    public List<float> enemyXRotList = new List<float>();
    public List<float> enemyYRotList = new List<float>();
    public List<float> enemyZRotList = new List<float>();
    public List<float> enemyWRotList = new List<float>();
}
