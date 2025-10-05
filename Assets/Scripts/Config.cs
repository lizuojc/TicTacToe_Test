using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configuration/Game Config")]
public class Config : ScriptableObject
{
    [Header("AI设置")]
    [Range(0f, 1f)]
    [Tooltip("聪明程度控制参数（0=完全随机，1=最优策略）")]
    public float smartness = 0.5f;

    [Tooltip("AI计算棋盘格子权重时，立即获胜的权重")]
    public float winWeight = 100;
    [Tooltip("AI计算棋盘格子权重时，阻止玩家获胜的权重")]
    public float defendWeight = 50;
    [Tooltip("AI计算棋盘格子权重时，中心位置的权重")]
    public float centerLocWeight = 20;
    [Tooltip("AI计算棋盘格子权重时，角落位置的权重")]
    public float cornerLocWeight = 10;
    [Tooltip("AI计算棋盘格子权重时，边缘位置的权重")]
    public float edgeLocWeight = 5;


    [Header("棋子设置")]
    [Tooltip("X棋子的颜色")]
    public Color xColor = Color.white;
    [Tooltip("O棋子的颜色")]
    public Color oColor = Color.white;
    [Tooltip("获胜棋子的颜色")]
    public Color highlightColor = Color.green;

    [Tooltip("X棋子的样式")]
    public Sprite xSprite;
    [Tooltip("O棋子的样式")]
    public Sprite oSprite;


    [Header("游戏设置")]
    [Tooltip("AI回合时等待的时间，用于模拟AI思考")]
    public float aiDelay = 0.5f;
    [Tooltip("游戏开始时，是否随机先手")]
    public bool randomFirstPlayer = true; // 随机先手选项

}
