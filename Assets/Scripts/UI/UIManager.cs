using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public StartMenu startMenu;
    public GameMenu gameMenu;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameManager.Instance.gameState == GameState.Playing)
            {
                OpenGameMenu();
            }
            else if(GameManager.Instance.gameState == GameState.GamePause)
            {
                CloseGameMenu();
            }
        }
    }

    private void OpenGameMenu()
    {
        if (!gameMenu.gameObject.activeInHierarchy)
        {
            Time.timeScale = 0f;
            GameManager.Instance.gameState = GameState.GamePause;
            gameMenu.gameObject.SetActive(true);
        }
    }

    private void CloseGameMenu()
    {
        if (gameMenu.gameObject.activeInHierarchy)
        {
            GameManager.Instance.gameState = GameState.Playing;
            gameMenu.ContinueGame();
        }
    }
}
