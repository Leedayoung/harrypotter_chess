﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceManager : MonoBehaviour
{
    [HideInInspector]
    public bool mIsKingAlive = true;
    public bool mHuman = true;
    public bool isBlackTurn = false;
    public static bool timer = false;

    public GameObject mPiecePrefab;

    private static List<BasePiece> mWhitePieces = null;
    private static List<BasePiece> mBlackPieces = null;
    private List<BasePiece> mPromotedPieces = new List<BasePiece>();

    public Vector2Int white_cam;
    public Vector2Int black_cam;
    public bool flag1 = false;
    public bool flag2 = false;

    public GameObject gameOverPanel;
    public Text gameOverText;

    public static List<BasePiece> ShareWhitePieces
    {
        get
        {
            return mWhitePieces;
        }
    }
    public static List<BasePiece> ShareBlackPieces
    {
        get
        {
            return mBlackPieces;
        }
    }

    public static void SetTimer(bool setter)
    {
        timer = setter;
    }

    private string[] mPieceOrder = new string[16]
    {
        "P", "P", "P", "P", "P", "P", "P", "P",
        "R", "KN", "B", "Q", "K", "B", "KN", "R"
    };

    private Dictionary<string, Type> mPieceLibrary = new Dictionary<string, Type>()
    {
        {"P",  typeof(Pawn)},
        {"R",  typeof(Rook)},
        {"KN", typeof(Knight)},
        {"B",  typeof(Bishop)},
        {"K",  typeof(King)},
        {"Q",  typeof(Queen)}
    };

    public void Setup(Board board)
    {
        mWhitePieces = CreatePieces(Color.white, new Color32(0, 99, 2, 255), board);
        mBlackPieces = CreatePieces(Color.black, new Color32(174, 0, 1, 255), board);

        PlacePieces(1, 0, mWhitePieces, board);
        PlacePieces(6, 7, mBlackPieces, board);
        SwitchSides(Color.black);
    }

    private List<BasePiece> CreatePieces(Color teamColor, Color32 spriteColor, Board board)
    {
        List<BasePiece> newPieces = new List<BasePiece>();

        for(int i =0; i< mPieceOrder.Length; i++)
        {
            GameObject newPieceObject = Instantiate(mPiecePrefab);
            newPieceObject.transform.SetParent(transform);

            newPieceObject.transform.localScale = new Vector3(0.4375f, 0.4375f, 0.4375f);
            newPieceObject.transform.localRotation = Quaternion.identity;

            string key = mPieceOrder[i];
            Type pieceType = mPieceLibrary[key];

            BasePiece newPiece = (BasePiece)newPieceObject.AddComponent(pieceType);
            newPieces.Add(newPiece);

            newPiece.Setup(teamColor, spriteColor, this);
        }
        return newPieces;
    }

    private void PlacePieces(int pawnRow, int royaltyRow, List<BasePiece> pieces, Board board)
    {
        for (int i = 0; i < 8; i++)
        {
            pieces[i].Place(board.mAllCells[i, pawnRow]);

            pieces[i+8].Place(board.mAllCells[i, royaltyRow]);
        }
    }

    private void SetInteractive(List<BasePiece> allPieces, bool value)
    {
        foreach (BasePiece piece in allPieces)
            piece.enabled = value;
    }

    public void SwitchSides(Color color)
    {
        if(!mIsKingAlive || !mHuman)
        {
            gameObject.SetActive(false);
            gameOverPanel.SetActive(true);
            if(color == Color.white) gameOverText.text = "Slytherine Wins!";
            else gameOverText.text = "Gryffindor Wins!";
            /*
            ResetPieces();
            mIsKingAlive = true;
            color = Color.black;
            */
        }
        isBlackTurn = color == Color.white ? true : false;
        SetInteractive(mWhitePieces, !isBlackTurn);
        SetInteractive(mBlackPieces, isBlackTurn);
    }

    public void ResetPieces()
    {
        foreach (BasePiece piece in mWhitePieces)
            piece.Reset();
        foreach (BasePiece piece in mBlackPieces)
            piece.Reset();
    }



    public void PromotePiece(Pawn pawn, Cell cell, Color teamColor, Color spriteColor)
    {

    }
}
