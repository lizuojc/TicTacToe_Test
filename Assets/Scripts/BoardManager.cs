using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public enum CellState { Empty, X, O }

    [Header("引用")]
    public Transform boardParent;
    public GameObject cellPrefab;

    private CellController[] cells = new CellController[9];
    private CellState[] boardState = new CellState[9];

    public void InitializeBoard()
    {
        // 清除现有棋盘
        foreach (Transform child in boardParent)
        {
            Destroy(child.gameObject);
        }

        // 创建新棋盘
        for (int i = 0; i < 9; i++)
        {
            GameObject cellObj = Instantiate(cellPrefab, boardParent);
            cellObj.name = "Cell_" + i;
            CellController cell = cellObj.GetComponent<CellController>();
            cell.Initialize(i);
            cells[i] = cell;
            boardState[i] = CellState.Empty;
        }
    }

    public bool IsCellEmpty(int index)
    {
        return boardState[index] == CellState.Empty;
    }

    public void SetCell(int index, CellState state)
    {
        boardState[index] = state;
        cells[index].SetState(state);
    }

    public CellState[] GetBoardState()
    {
        return boardState;
    }

    public CellState CheckWinner()
    {
        // 检查所有可能的获胜组合
        int[,] winCombinations = {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // 行
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // 列
            {0, 4, 8}, {2, 4, 6}             // 对角线
        };

        for (int i = 0; i < winCombinations.GetLength(0); i++)
        {
            int a = winCombinations[i, 0];
            int b = winCombinations[i, 1];
            int c = winCombinations[i, 2];

            if (boardState[a] != CellState.Empty &&
                boardState[a] == boardState[b] &&
                boardState[b] == boardState[c])
            {
                HighlightWinningCells(a, b, c);
                return boardState[a];
            }
        }

        return CellState.Empty;
    }

    private void HighlightWinningCells(int a, int b, int c)
    {
        cells[a].Highlight();
        cells[b].Highlight();
        cells[c].Highlight();
    }
}
