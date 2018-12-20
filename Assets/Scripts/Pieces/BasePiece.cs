using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public abstract class BasePiece : EventTrigger
{
    [HideInInspector]
    public Color mColor = Color.clear;
    public string pieceName = null;
    //public bool mIsFirstMove = true;

    protected Cell mOriginalCell = null;
    public Cell mCurrentCell = null;

    protected RectTransform mRectTransform = null;
    protected PieceManager mPieceManager;

    protected Cell mTargetCell = null;

    protected Vector3Int mMovement = Vector3Int.one;
    protected List<Cell> mHighlightedCells = new List<Cell>();

    protected bool isHuman = false;

    public virtual void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        mPieceManager = newPieceManager;

        mColor = newTeamColor;
        GetComponent<Image>().color = newSpriteColor;
        mRectTransform = GetComponent<RectTransform>();

    }

    public virtual void Place(Cell newCell)
    {
        mCurrentCell = newCell;
        mOriginalCell = newCell;
        mCurrentCell.mCurrentPiece = this;

        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        Kill();

        Place(mOriginalCell);
    }

    public virtual void Kill()
    {
       
        //Clear Current Cell
        mCurrentCell.mCurrentPiece = null;

        //Remove Pieces
        gameObject.SetActive(false);

        if (isHuman) mPieceManager.mHuman = false;
    }

    #region Movement
    private void CreateCellPath(int xDirection, int yDirection, int movement)
    {
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        for (int i=1;i<=movement; i++)
        {
            currentX += xDirection;
            currentY += yDirection;

            CellState cellState = CellState.None;
            cellState = mCurrentCell.mBoard.ValidateCell(currentX, currentY, this);

            if(cellState == CellState.Enemy)
            {
                mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
                break;
            }

            if(cellState != CellState.Free)
            {
                break;
            }

            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
        }

    }

    protected virtual void CheckPathing()
    {
        CreateCellPath(1, 0, mMovement.x);
        CreateCellPath(-1, 0, mMovement.x);

        CreateCellPath(0, 1, mMovement.y);
        CreateCellPath(0, -1, mMovement.y);

        CreateCellPath(1, 1, mMovement.z);
        CreateCellPath(-1, 1, mMovement.z);

        CreateCellPath(-1, -1, mMovement.z);
        CreateCellPath(1, -1, mMovement.z);

    }

    protected void ShowCells()
    {
        foreach (Cell cell in mHighlightedCells)
            cell.mOutlineImage.enabled = true;
    }

    protected void ClearCells()
    {
        foreach (Cell cell in mHighlightedCells)
            cell.mOutlineImage.enabled = false;
        mHighlightedCells.Clear();
    }

    protected virtual void Move()
    {
        mTargetCell.RemovePiece();
        mCurrentCell.mCurrentPiece = null;
        mCurrentCell.mPastPiece = null;

        //switch cells
        mCurrentCell = mTargetCell;
        mCurrentCell.mCurrentPiece = this;

        transform.position = mCurrentCell.transform.position;
        mTargetCell = null;

    }
    #endregion

    #region Events

    public override void OnPointerClick(PointerEventData data)
    {
        //if (mCurrentCell.mCurrentPiece ==  ) return;       
        bool determ1 = mCurrentCell.mBoard.view_point1;
        bool determ2 = mCurrentCell.mBoard.view_point2;
        //Debug.Log(determ1);
        if (!determ1 && !determ2) return;
        else if (!determ1)
        {
            isHuman = true;
            Debug.Log("Second was called.");
            mCurrentCell.mBoard.view_point2 = false;
            mPieceManager.SwitchSides(mColor);
            GetComponent<Image>().color = new Color32(153, 0, 102, 255);
            return;
        }
        isHuman = true;
        Debug.Log("First was called.");
        mCurrentCell.mBoard.view_point1 = false;
        GetComponent<Image>().color = new Color32(0, 47, 1, 255);
        mPieceManager.SwitchSides(mColor);
        return;

    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        CheckPathing();
        ShowCells();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        transform.position += (Vector3)eventData.delta;

        foreach (Cell cell in mHighlightedCells)
        {
            if(RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Input.mousePosition))
            {
                mTargetCell = cell;
                break;
            }

            mTargetCell = null;
        }
        print(mCurrentCell.mBoardPosition);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        //Hide
        ClearCells();

        //Return to original position
        if (!mTargetCell)
        {
            transform.position = mCurrentCell.gameObject.transform.position;
            return;
        }
        
        //Move to New Cell
        Move();

        Debug.Log(mCurrentCell.mBoardPosition);
        //End turn
        mPieceManager.SwitchSides(mColor);
    }
    #endregion
}
