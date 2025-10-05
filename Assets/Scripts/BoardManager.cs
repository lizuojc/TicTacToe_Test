using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public enum CellState { Empty, X, O }

    [Header("����")]
    public Transform boardParent;
    public GameObject cellPrefab;

    private CellController[] cells = new CellController[9];
    private CellState[] boardState = new CellState[9];

    public void InitializeBoard()
    {
        // �����������
        foreach (Transform child in boardParent)
        {
            Destroy(child.gameObject);
        }

        // ����������
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
        // ������п��ܵĻ�ʤ���
        int[,] winCombinations = {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // ��
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // ��
            {0, 4, 8}, {2, 4, 6}             // �Խ���
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
