using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static TicTacToeGame;
using static BoardManager;

public class CellController : MonoBehaviour, IPointerClickHandler
{
    [Header("引用")]
    public Image symbolImage;

    private Sprite xSprite;
    private Sprite oSprite;

    private Color xColor;
    private Color oColor;
    private Color highlightColor;

    private int cellIndex;
    private TicTacToeGame gameController;

    public void Initialize(int index)
    {
        cellIndex = index;
        gameController = TicTacToeGame.Instance;
        symbolImage.enabled = false;

        xColor = ConfigLoader.Instance.gameConfig.xColor;
        oColor = ConfigLoader.Instance.gameConfig.oColor;
        highlightColor = ConfigLoader.Instance.gameConfig.highlightColor;

        xSprite = ConfigLoader.Instance.gameConfig.xSprite;
        oSprite = ConfigLoader.Instance.gameConfig.oSprite;
    }

    public void SetState(CellState state)
    {
        symbolImage.enabled = true;

        switch (state)
        {
            case CellState.X:
                symbolImage.sprite = xSprite;
                symbolImage.color = xColor;
                break;
            case CellState.O:
                symbolImage.sprite = oSprite;
                symbolImage.color = oColor;
                break;
            default:
                symbolImage.enabled = false;
                break;
        }
    }

    public void Highlight()
    {
        symbolImage.color = highlightColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameMode gameMode = gameController.GetCurrentGameMode();
        // 在PVP模式下，任何时候都可以点击
        // 在PVE模式下，只有人类玩家回合才能点击
        if (gameMode == GameMode.PVP ||
            (gameMode == GameMode.PVE && gameController.GetCurrentPlayer() == PlayerType.Human))
        {
            gameController.OnCellClicked(cellIndex);
        }
    }
}
