using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BoardManager;

public class TicTacToeGame : Singleton<TicTacToeGame>
{
    public enum PlayerType { Human, AI }
    public enum GameState { Playing, PlayerWin, AIWin, Tie }
    public enum GameMode { PVE, PVP } // 游戏模式

    // 游戏设置
    private PlayerType startingPlayer = PlayerType.Human;
    private GameMode gameMode = GameMode.PVE; // 游戏模式模式
    private float aiDelay = 0.5f;
    private bool randomFirstPlayer = true; // 随机先手选项
    private PlayerType currentPlayer;
    private GameState gameState = GameState.Playing;
    private int moveCount = 0;

    [Header("引用")]
    public BoardManager boardManager;
    public UIManager uiManager;
    public AIController aiController;
    public GameObject mainMenu;

    // 数据
    private int player1Score = 0; // 玩家1得分
    private int player2Score = 0; // 玩家2得分
    private int ties = 0; // 平局次数

    void Start()
    {
        mainMenu.SetActive(true);
        aiDelay = ConfigLoader.Instance.gameConfig.aiDelay;
        randomFirstPlayer = ConfigLoader.Instance.gameConfig.randomFirstPlayer;
    }

    public void InitializeGame()
    {
        // 随机决定先手玩家
        if (randomFirstPlayer)
        {
            startingPlayer = Random.Range(0, 2) == 0 ? PlayerType.Human : PlayerType.AI;
        }

        boardManager.InitializeBoard();
        currentPlayer = startingPlayer;
        gameState = GameState.Playing;
        moveCount = 0;

        // 更新状态文本
        if (gameMode == GameMode.PVP)
        {
            uiManager.UpdateStatusText($"玩家 {(currentPlayer == PlayerType.Human ? "1" : "2")} 回合（{(currentPlayer == PlayerType.Human ? "X" : "O")}）");
        }
        else
        {
            uiManager.UpdateStatusText($"{(currentPlayer == PlayerType.Human ? "到你了（X）" : "电脑回合")}");
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
                    uiManager.UpdateStatusText("电脑正在思考...");
                    Invoke("AIMove", aiDelay);
                }
                else // PVP模式
                {
                    currentPlayer = currentPlayer == PlayerType.Human ? PlayerType.AI : PlayerType.Human;
                    uiManager.UpdateStatusText($"玩家 {(currentPlayer == PlayerType.Human ? "1" : "2")} 回合（{(currentPlayer == PlayerType.Human ? "X" : "O")}）");
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
            uiManager.UpdateStatusText("到你了(X)");
        }
    }

    void CheckGameState()
    {
        CellState winner = boardManager.CheckWinner();

        if (winner == CellState.X)
        {
            gameState = GameState.PlayerWin;
            player1Score++;
            uiManager.ShowGameResult("玩家 1 赢了!");
        }
        else if (winner == CellState.O)
        {
            gameState = gameMode == GameMode.PVE ? GameState.AIWin : GameState.PlayerWin;

            if (gameMode == GameMode.PVE)
            {
                player2Score++;
                uiManager.ShowGameResult("电脑赢了!");
            }
            else
            {
                player2Score++;
                uiManager.ShowGameResult("玩家 2 赢了!");
            }
        }
        else if (moveCount == 9)
        {
            gameState = GameState.Tie;
            ties++;
            uiManager.ShowGameResult("平局!");
        }

        uiManager.UpdateScore(player1Score, player2Score, ties);
    }

    public void ShowMainMenu()
    {
        uiManager.ShowConfirmationDialog("返回主界面将重置得分，确定吗？", () => {
            player1Score = 0;
            player2Score = 0;
            ties = 0;
            mainMenu.SetActive(true);
        });
    }

    public void RestartGame()
    {
        uiManager.ShowConfirmationDialog("重新开始将重置当前游戏，且重置得分，确定吗？", () => {
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
