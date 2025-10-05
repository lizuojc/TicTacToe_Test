using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TicTacToeGame;

public class UIManager : MonoBehaviour
{
    [Header("UI引用")]
    public TextMeshProUGUI statusText;
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;

    
    public Button backToMainButton;
    public Button restartButton;

    public Button backToMainButton1; 
    public Button oneMoreButton;

    [Header("得分显示")]
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;
    public TextMeshProUGUI player2Name;
    public TextMeshProUGUI tiesText;

    [Header("确认对话框")]
    public GameObject confirmationDialog;
    public TextMeshProUGUI dialogMessage;
    public Button confirmButton;
    public Button cancelButton;

    void Start()
    {
        TicTacToeGame game = TicTacToeGame.Instance;
        restartButton.onClick.AddListener(game.RestartGame);
        oneMoreButton.onClick.AddListener(game.OneMoreGame);

        backToMainButton.onClick.AddListener(game.ShowMainMenu);
        backToMainButton1.onClick.AddListener(game.ShowMainMenu);

        confirmationDialog.SetActive(false);
        resultPanel.SetActive(false);
    }

    public void UpdateStatusText(string message)
    {
        statusText.text = message;
    }

    public void ShowGameResult(string result)
    {
        resultText.text = result;
        resultPanel.SetActive(true);
    }

    public void HideResultPanel()
    {
        resultPanel.SetActive(false);
    }

    public void InitialPlayerName(GameMode gameMode)
    {
        player2Name.text = gameMode == GameMode.PVE ? "电脑" : "玩家 2";
    }

    public void UpdateScore(int player1, int player2, int ties)
    {
        player1ScoreText.text = player1.ToString();
        player2ScoreText.text = player2.ToString();
        tiesText.text = $"平局数: {ties}";
    }

    public void ShowConfirmationDialog(string message, Action onConfirm)
    {
        confirmationDialog.SetActive(true);
        dialogMessage.text = message;

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() => {
            confirmationDialog.SetActive(false);
            onConfirm?.Invoke();
        });

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => {
            confirmationDialog.SetActive(false);
        });
    }
}
