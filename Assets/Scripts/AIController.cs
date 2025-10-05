using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BoardManager;

public class AIController : MonoBehaviour
{
    private float smartness = 1f; // 聪明程度控制参数（0=完全随机，1=最优策略）
    private float winWeight;
    private float defendWeight;
    private float centerLocWeight;
    private float cornerLocWeight;
    private float edgeLocWeight;

    private void Start()
    {
        smartness = ConfigLoader.Instance.gameConfig.smartness;
        winWeight = ConfigLoader.Instance.gameConfig.winWeight;
        defendWeight = ConfigLoader.Instance.gameConfig.defendWeight;
        centerLocWeight = ConfigLoader.Instance.gameConfig.centerLocWeight;
        cornerLocWeight = ConfigLoader.Instance.gameConfig.cornerLocWeight;
        edgeLocWeight = ConfigLoader.Instance.gameConfig.edgeLocWeight;
    }

    public int FindBestMove(CellState[] board)
    {
        List<int> emptyCells = new List<int>();
        for (int i = 0; i < 9; i++)
        {
            if (board[i] == CellState.Empty)
            {
                emptyCells.Add(i);
            }
        }

        // 计算每个空位置的权重
        float[] weights = CalculateWeights(board, emptyCells);

        // 根据聪明程度调整权重分布
        AdjustWeightsBySmartness(weights, smartness);

        // 根据聪明程度，选择是否使用调整后的权重
        float[] uniformWeights = new float[emptyCells.Count];
        float lowestWeight = float.MaxValue;
        for (int i = 0; i < weights.Length; i++)
        {
            if (weights[i] < lowestWeight) lowestWeight = weights[i];
        }
        for (int i = 0; i < uniformWeights.Length; i++)
        {
            uniformWeights[i] = lowestWeight;
        }
        MixWeights(weights, uniformWeights, smartness);

        string log = null;
        for (int i = 0; i < weights.Length; i++)
        {
            log += $"{weights[i]}, ";
        }
        print(log);

        // 加权随机选择（轮盘赌）
        int selectedIndex = WeightedRandomChoice(emptyCells, weights);
        return emptyCells[selectedIndex];
    }

    private float[] CalculateWeights(CellState[] board, List<int> emptyCells)
    {
        float[] weights = new float[emptyCells.Count];
        float maxWeight = 0f;

        for (int i = 0; i < emptyCells.Count; i++)
        {
            int pos = emptyCells[i];
            float weight = 1f; // 基础权重

            // 是否立即获胜
            board[pos] = CellState.O;
            if (CheckWinner(board) == CellState.O)
            {
                weight += winWeight; // 最高优先级
            }
            board[pos] = CellState.Empty;

            // 是否阻止玩家获胜
            board[pos] = CellState.X;
            if (CheckWinner(board) == CellState.X)
            {
                weight += defendWeight; // 次高优先级
            }
            board[pos] = CellState.Empty;

            // 位置类型加成
            if (pos == 4) weight += centerLocWeight;      // 中心位置
            else if (IsCorner(pos)) weight += cornerLocWeight; // 角落位置
            else weight += edgeLocWeight;                // 边缘位置

            weights[i] = weight;
            if (weight > maxWeight) maxWeight = weight;
        }

        // 归一化权重
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = weights[i] / maxWeight;
        }

        return weights;
    }

    private void AdjustWeightsBySmartness(float[] weights, float smartness)
    {
        float exponent = 1f + 3f * smartness; // 指数范围1-4

        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = Mathf.Pow(weights[i], exponent);
        }
    }


    private void MixWeights(float[] adjustedWeights, float[] uniformWeights, float smartness)
    {
        for (int i = 0; i < adjustedWeights.Length; i++)
        {
            // smartness=0 → 完全均匀权重；smartness=1 → 完全策略权重
            adjustedWeights[i] = Mathf.Lerp(uniformWeights[i], adjustedWeights[i], smartness);
        }
    }


    // 轮盘赌
    private int WeightedRandomChoice(List<int> options, float[] weights)
    {
        float total = 0f;
        foreach (float w in weights) total += w;

        float randomPoint = Random.value * total;
        float cumulative = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i];
            if (randomPoint <= cumulative)
            {
                return i;
            }
        }

        return weights.Length - 1;
    }

    private bool IsCorner(int pos)
    {
        return pos == 0 || pos == 2 || pos == 6 || pos == 8;
    }

    private CellState CheckWinner(CellState[] board)
    {
        int[,] winCombinations = {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8},
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8},
            {0, 4, 8}, {2, 4, 6}
        };

        for (int i = 0; i < winCombinations.GetLength(0); i++)
        {
            int a = winCombinations[i, 0];
            int b = winCombinations[i, 1];
            int c = winCombinations[i, 2];

            if (board[a] != CellState.Empty &&
                board[a] == board[b] &&
                board[b] == board[c])
            {
                return board[a];
            }
        }

        return CellState.Empty;
    }
}
