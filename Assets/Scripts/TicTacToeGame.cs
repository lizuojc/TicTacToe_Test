using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BoardManager;

public class TicTacToeGame : Singleton<TicTacToeGame>
{
    public enum PlayerType { Human, AI }
    public enum GameState { Playing, PlayerWin, AIWin, Tie }
    public enum GameMode { PVE, PVP } // ��Ϸģʽ

    // ��Ϸ����
    private PlayerType startingPlayer = PlayerType.Human;
    private GameMode gameMode = GameMode.PVE; // ��Ϸģʽģʽ
    private float aiDelay = 0.5f;
    private bool randomFirstPlayer = true; // �������ѡ��
    private PlayerType currentPlayer;
    private GameState gameState = GameState.Playing;
    private int moveCount = 0;

    [Header("����")]
    public BoardManager boardManager;
    public UIManager uiManager;
    public AIController aiController;
    public GameObject mainMenu;

    // ����
    private int player1Score = 0; // ���1�÷�
    private int player2Score = 0; // ���2�÷�
    private int ties = 0; // ƽ�ִ���

    void Start()
    {
        mainMenu.SetActive(true);
        aiDelay = ConfigLoader.Instance.gameConfig.aiDelay;
        randomFirstPlayer = ConfigLoader.Instance.gameConfig.randomFirstPlayer;
    }

    public void InitializeGame()
    {
        // ��������������
        if (randomFirstPlayer)
        {
            startingPlayer = Random.Range(0, 2) == 0 ? PlayerType.Human : PlayerType.AI;
        }

        boardManager.InitializeBoard();
        currentPlayer = startingPlayer;
        gameState = GameState.Playing;
        moveCount = 0;

        // ����״̬�ı�
        if (gameMode == GameMode.PVP)
        {
            uiManager.UpdateStatusText($"��� {(currentPlayer == PlayerType.Human ? "1" : "2")} �غϣ�{(currentPlayer == PlayerType.Human ? "X" : "O")}��");
        }
        else
        {
            uiManager.UpdateStatusText($"{(currentPlayer == PlayerType.Human ? "�����ˣ�X��" : "���Իغ�")}");
        }

        uiManager.HideResultPanel();
        uiManager.InitialPlayerName(gameMode);
        uiManager.UpdateScore(player1Score, player2Score, ties);

        if (currentPlayer == PlayerType.AI && gameMode == GameMode.PVE)
        {
            Invoke("AIMove", aiDelay);
        }
    }

    public void OnCellClicked(int cellIndex)
    {
        if (gameState != GameState.Playing ||
            (currentPlayer != PlayerType.Human && gameMode == GameMode.PVE))
            return;

        if (boardManager.IsCellEmpty(cellIndex))
        {
            CellState state = (gameMode == GameMode.PVP && currentPlayer == PlayerType.AI) ? 
                CellState.O : CellState.X;
            
            boardManager.SetCell(cellIndex, state);
            moveCount++;
            CheckGameState();

            if (gameState == GameState.Playing)
            {
                if (gameMode == GameMode.PVE)
                {
                    currentPlayer = PlayerType.AI;
                    uiManager.UpdateStatusText("��������˼��...");
                    Invoke("AIMove", aiDelay);
                }
                else // PVPģʽ
                {
                    currentPlayer = currentPlayer == PlayerType.Human ? PlayerType.AI : PlayerType.Human;
                    uiManager.UpdateStatusText($"��� {(currentPlayer == PlayerType.Human ? "1" : "2")} �غϣ�{(currentPlayer == PlayerType.Human ? "X" : "O")}��");
                }
            }
        }
    }

    void AIMove()
    {
        if (gameState != GameState.Playing || currentPlayer != PlayerType.AI)
            return;

        int move = aiController.FindBestMove(boardManager.GetBoardState());
        boardManager.SetCell(move, CellState.O);
        moveCount++;
        CheckGameState();

        if (gameState == GameState.Playing)
        {
            currentPlayer = PlayerType.Human;
            uiManager.UpdateStatusText("������(X)");
        }
    }

    void CheckGameState()
    {
        CellState winner = boardManager.CheckWinner();

        if (winner == CellState.X)
        {
            gameState = GameState.PlayerWin;
            player1Score++;
            uiManager.ShowGameResult("��� 1 Ӯ��!");
        }
        else if (winner == CellState.O)
        {
            gameState = gameMode == GameMode.PVE ? GameState.AIWin : GameState.PlayerWin;

            if (gameMode == GameMode.PVE)
            {
                player2Score++;
                uiManager.ShowGameResult("����Ӯ��!");
            }
            else
            {
                player2Score++;
                uiManager.ShowGameResult("��� 2 Ӯ��!");
            }
        }
        else if (moveCount == 9)
        {
            gameState = GameState.Tie;
            ties++;
            uiManager.ShowGameResult("ƽ��!");
        }

        uiManager.UpdateScore(player1Score, player2Score, ties);
    }

    public void ShowMainMenu()
    {
        uiManager.ShowConfirmationDialog("���������潫���õ÷֣�ȷ����", () => {
            player1Score = 0;
            player2Score = 0;
            ties = 0;
            mainMenu.SetActive(true);
        });
    }

    public void RestartGame()
    {
        uiManager.ShowConfirmationDialog("���¿�ʼ�����õ�ǰ��Ϸ�������õ÷֣�ȷ����", () => {
            player1Score = 0;
            player2Score = 0;
            ties = 0;
            InitializeGame();
        });
    }

    public void OneMoreGame()
    {
        InitializeGame();
    }

    public void SetGameMode(GameMode gameMode)
    {
        this.gameMode = gameMode;
    }

    public GameMode GetCurrentGameMode()
    {
        return gameMode;
    }

    public PlayerType GetCurrentPlayer()
    {
        return currentPlayer;
    }
}
