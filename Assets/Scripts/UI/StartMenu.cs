using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Button btnNewGame;
    public Button btnContinueGame;
    public Button btnQuitGame;

    private void Start()
    {
        btnNewGame.onClick.AddListener(NewGame);
        btnContinueGame.onClick.AddListener(ContinueGame);
        btnQuitGame.onClick.AddListener(QuitGame);
    }

    private void Update()
    {
        btnContinueGame.interactable = GameManager.Instance.isExitSaveData;
    }

    public void NewGame()
    {
        GameManager.Instance.LoadGame(GameManager.Instance.defaultSavePath);
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        GameManager.Instance.gameState = GameState.Playing;
    }

    public void ContinueGame()
    {
        //TODO 加载保存进度
        GameManager.Instance.LoadGame(GameManager.Instance.filePath);
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        GameManager.Instance.gameState = GameState.Playing;
    }

    public void QuitGame()
    {
        // 在编辑器中检查是否在播放模式中
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            // 在编辑器中停止播放模式
            EditorApplication.isPlaying = false;
        }
#endif

        // 在构建后的应用程序中调用Application.Quit()来退出游戏
        Application.Quit();
    }
}
