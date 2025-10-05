using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI����")]
    public Button playButton;
    public Button quitButton;
    public GameObject modeSelectionPanel;
    public Button pveButton; // �˻���ս
    public Button pvpButton; // ˫�˶�ս

    void Start()
    {
        playButton.onClick.AddListener(ShowModeSelection);
        quitButton.onClick.AddListener(QuitGame);
        pveButton.onClick.AddListener(() => StartGame(true));
        pvpButton.onClick.AddListener(() => StartGame(false));

        modeSelectionPanel.SetActive(false);
    }

    void ShowModeSelection()
    {
        modeSelectionPanel.SetActive(true);
    }

    public void StartGame(bool isPVE)
    {
        TicTacToeGame game = TicTacToeGame.Instance;
        game.SetGameMode(isPVE ? TicTacToeGame.GameMode.PVE : TicTacToeGame.GameMode.PVP);
        game.InitializeGame();
        modeSelectionPanel.SetActive(false);
        gameObject.SetActive(false); // �������˵�
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }
}
