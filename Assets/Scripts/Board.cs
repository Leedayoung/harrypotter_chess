using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// New
public enum CellState
{
    None,
    Friendly,
    Enemy,
    Free,
    OutOfBounds
}

public class Board : MonoBehaviour
{
    public GameObject mCellPrefab;
    public bool view_point1 = true;
    public bool view_point2 = true;

    [HideInInspector]
    public Cell[,] mAllCells = new Cell[8, 8];

    float scale= 0.4375f;


    public void Create()
    {
        for (int y = 0; y < 8; ++y)
        {
            for(int x = 0; x <8; ++x)
            {
                //Create the cell
                GameObject newCell = Instantiate(mCellPrefab, transform);

                //Position
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 100*scale) + 50 * scale, (y * 100 * scale) + 50 * scale);

                //Setup
                mAllCells[x,y] = newCell.GetComponent<Cell>();
                //mAllCells[x, y].GetComponent<Image>().color = new Color32(238,186,48, 255);
                mAllCells[x,y].Setup(new Vector2Int(x, y), this);
            }
        }
        //Color
        for(int x= 0; x < 8; x += 2)
        {
            for(int y = 0; y < 8; y++)
            {
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalX = x + offset;
                mAllCells[finalX,y].GetComponent<Image>().color = new Color32(76, 76, 76, 255);
            }
        }
        view_point1 = true;
        view_point2 = true;
    }

    public CellState ValidateCell(int targetX, int targetY, BasePiece checkingPiece)
    {
        //Bound Check
        if (targetX < 0 || targetX > 7)
            return CellState.OutOfBounds;
        if (targetY < 0 || targetY > 7)
            return CellState.OutOfBounds;

        Cell targetCell = mAllCells[targetX, targetY];

        if (targetCell.mCurrentPiece != null){
            if (checkingPiece.mColor == targetCell.mCurrentPiece.mColor)
                return CellState.Friendly;
            else
                return CellState.Enemy;
        }


        return CellState.Free;
    }
}
