using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public Button btnContinueGame;
    public Button btnSaveAndBack;
    public Button btnQuitGame;

    private void Start()
    {
        btnContinueGame.onClick.AddListener(ContinueGame);
        btnSaveAndBack.onClick.AddListener(SaveAndBack);
        btnQuitGame.onClick.AddListener(QuitGame);
    }

    public void ContinueGame()
    {
        GameManager.Instance.gameState = GameState.Playing;
        this.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void SaveAndBack()
    {
        //TODO 保存游戏
        GameManager.Instance.SaveGame();
        GameManager.Instance.DestroyAllEnemy();
        //返回主菜单
        GameManager.Instance.gameState = GameState.GameStart;
        UIManager.Instance.startMenu.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
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
