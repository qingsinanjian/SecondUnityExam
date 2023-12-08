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
        //TODO ������Ϸ
        GameManager.Instance.SaveGame();
        GameManager.Instance.DestroyAllEnemy();
        //�������˵�
        GameManager.Instance.gameState = GameState.GameStart;
        UIManager.Instance.startMenu.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
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
