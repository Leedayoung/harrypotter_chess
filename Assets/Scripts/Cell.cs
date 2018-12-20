﻿using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public Image mOutlineImage;

    [HideInInspector]
    public Vector2Int mBoardPosition = Vector2Int.zero;
    [HideInInspector]
    public Board mBoard = null;
    [HideInInspector]
    public RectTransform mRectTransform = null;
    [HideInInspector]
    public BasePiece mCurrentPiece = null;
    public BasePiece mPastPiece = null;

    #region Setup
    public void Setup(Vector2Int newBoardPosition, Board newBoard)
    {
        mBoardPosition = newBoardPosition;
        mBoard = newBoard;

        mRectTransform = GetComponent<RectTransform>();
    }
    #endregion
    

    public void RemovePiece()
    {
        if (mCurrentPiece != null)
        {
            mPastPiece = mCurrentPiece;
            mCurrentPiece.Kill();
        }
    }
}
