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
        //TODO ���ر������
        GameManager.Instance.LoadGame(GameManager.Instance.filePath);
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        GameManager.Instance.gameState = GameState.Playing;
    }

    public void QuitGame()
    {
        // �ڱ༭���м���Ƿ��ڲ���ģʽ��
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            // �ڱ༭����ֹͣ����ģʽ
            EditorApplication.isPlaying = false;
        }
#endif

        // �ڹ������Ӧ�ó����е���Application.Quit()���˳���Ϸ
        Application.Quit();
    }
}
