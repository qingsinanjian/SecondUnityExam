using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum GameState
{
    GameStart,
    Playing,
    GamePause,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState gameState;
    public PlayerManager playerManager;
    public GameObject enemyPrefab;
    public List<EnemyManager> enemyManagers = new List<EnemyManager>();
    public string defaultSavePath = Application.streamingAssetsPath + "/DefaultSave.json";
    //保存路径
    public string filePath = Application.streamingAssetsPath + "/Save.json";
    //是否存在游戏保存记录
    public bool isExitSaveData;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameState = GameState.GameStart;
        Time.timeScale = 0;
        isExitSaveData = File.Exists(filePath);
    }

    /// <summary>
    /// 保存游戏
    /// </summary>
    public void SaveGame()
    {
        Save save = new Save();
        //玩家位置及状态
        Vector3 playerPos = playerManager.transform.position;
        Quaternion playerRot = playerManager.transform.rotation;
        save.playerXPos = playerPos.x;
        save.playerYPos = playerPos.y;
        save.playerZPos = playerPos.z;
        save.playerXRot = playerRot.x;
        save.playerYRot = playerRot.y;
        save.playerZRot = playerRot.z;
        save.playerWRot = playerRot.w;
        save.currentHP = playerManager.currentHP;
        save.MaxHP = playerManager.MaxHP;
        save.currentMP = playerManager.currentMP;
        save.MaxMP = playerManager.MaxMP;
        //敌人位置
        save.enemyCount = enemyManagers.Count;
        for (int i = 0; i < enemyManagers.Count; i++)
        {
            Vector3 pos = enemyManagers[i].transform.position;
            Quaternion rot = enemyManagers[i].transform.rotation;
            save.enemyXPosList.Add(pos.x);
            save.enemyYPosList.Add(pos.y);
            save.enemyZPosList.Add(pos.z);
            save.enemyXRotList.Add(rot.x);
            save.enemyYRotList.Add(rot.y);
            save.enemyZRotList.Add(rot.z);
            save.enemyWRotList.Add(rot.w);
        }
        string data = JsonConvert.SerializeObject(save);
        File.WriteAllText(filePath, data);
        isExitSaveData = true;
    }

    /// <summary>
    /// 加载游戏
    /// </summary>
    public void LoadGame(string path)
    {
        if(!File.Exists(path))
        {
            return;
        }
        
        string data = File.ReadAllText(path);
        Save save = JsonConvert.DeserializeObject<Save>(data);
        Vector3 pos = new Vector3(save.playerXPos, save.playerYPos, save.playerZPos);
        Quaternion rot = new Quaternion(save.playerXRot, save.playerYRot, save.playerZRot, save.playerWRot);
        playerManager.Init(pos, rot, save.currentHP, save.MaxHP, save.currentMP, save.MaxMP);

        for (int i = 0; i < save.enemyCount; i++)
        {
            GameObject go = Instantiate(enemyPrefab);
            Vector3 enemyPos = new Vector3(save.enemyXPosList[i], save.enemyYPosList[i], save.enemyZPosList[i]);
            Quaternion enemyRot = new Quaternion(save.enemyXRotList[i], save.enemyYRotList[i], save.enemyZRotList[i], save.enemyWRotList[i]);
            go.transform.position = enemyPos;
            go.transform.rotation = enemyRot;
            enemyManagers.Add(go.GetComponent<EnemyManager>());
        }
    }

    /// <summary>
    /// 清楚所有敌人
    /// </summary>
    public void DestroyAllEnemy()
    {
        for (int i = 0;i < enemyManagers.Count; i++)
        {
            Destroy(enemyManagers[i].gameObject);
        }
        enemyManagers.Clear();
    }
}
