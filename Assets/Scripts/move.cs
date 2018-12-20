﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{

    enum DIR { UP, DOWN, LEFT, RIGHT, LUP, RUP, LDOWN, RDOWN };
    public float smoothTime = 1.0F;
    private float scale = 6.5F;
    private Vector3 velocity = Vector3.zero;

    int randomNum = 0;
    BasePiece pieceToRemove;
    Vector3 targetPosition;
    Vector2Int boardPosition = Vector2Int.zero;

    public bool on_process = false;

    private Dictionary<string, int> pieceName2idx = new Dictionary<string, int>()
    {
        {"Pawn_1", 0}, {"Pawn_2", 1}, {"Pawn_3", 2}, {"Pawn_4", 3},
        {"Pawn_5", 4}, {"Pawn_6", 5}, {"Pawn_7", 6}, {"Pawn_8",  7},
        {"Rook_l", 8}, {"Knight_l", 9}, {"Bishop_l", 10}, {"Queen", 11},
        {"King", 12}, {"Bishop_r", 13}, {"Knight_r", 14}, {"Rook_r", 15}
    };

    protected virtual void Start()
    {
        targetPosition = transform.position;
        boardPosition.x -= 1;
        boardPosition.y -= 1;
    }

    void destroy_chess()
    {
        Destroy(gameObject);
    }

    void destroy_chess_process()
    {
        Mesh mesh = GetComponentInChildren<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        if (randomNum % 2 == 0)
            for (var i = 0; i < vertices.Length; i++)
                vertices[i] += vertices[i] * Random.Range(0f, 0.3f); //* Mathf.Sin(Time.time);

        else
            for (var i = 0; i < vertices.Length; i++)
                vertices[i] = vertices[i] * Random.Range(0.9f, 1.0f); //* Mathf.Sin(Time.time);

        mesh.vertices = vertices;
        GetComponentInChildren<MeshFilter>().mesh = mesh;
    }
    // Update is called once per frame
    void Update()
    {
        string objectName = gameObject.name;
        List<BasePiece> pieces = null;

        // Get location from 2D board
        if (objectName.Substring(0, 2).Equals("my"))
        {
            pieces = PieceManager.ShareWhitePieces;
        }
        else if(objectName.Substring(0, 2).Equals("op"))
        {
            pieces = PieceManager.ShareBlackPieces;
        }
        else
        {
            print("Naming incorrect!");
        }

        string key = objectName.Substring(2, objectName.Length - 2);
        int idx = pieceName2idx[key];
        pieces[idx].pieceName = objectName;
        Vector2Int newBoardPosition = pieces[idx].mCurrentCell.mBoardPosition;

        if(boardPosition.x < 0)
        {
            boardPosition = newBoardPosition;
            return;
        }

        Vector2Int diff = newBoardPosition - boardPosition;
        if (diff != new Vector2Int(0,0))
        {
            Vector3 targetPosition_temp = gameObject.transform.position;
            targetPosition_temp.x -= diff.x * scale;
            targetPosition_temp.z -= diff.y * scale;
            targetPosition = targetPosition_temp;
            pieceToRemove = pieces[idx].mCurrentCell.mPastPiece;
            on_process = true;
            PieceManager.timer = on_process;
        }
        boardPosition = newBoardPosition;

        // Random destroy effect
        if (randomNum == 0)
            randomNum = Random.Range(1, 100);

        // Move 3D object
        if (on_process)
        {
            if ((int)transform.position.x== (int)targetPosition.x && (int)transform.position.z == (int)targetPosition.z)
            {
                //Debug.Log("Free");
                if (pieceToRemove) GameObject.Find(pieceToRemove.pieceName).GetComponent<move>().destroy_chess();
                on_process = false;
                PieceManager.timer = false;
                return;
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
                if (pieceToRemove)
                {
                    GameObject.Find(pieceToRemove.pieceName).GetComponent<move>().destroy_chess_process();
                }
                return;
                //Debug.Log(PieceManager.timer);
                //destroy_chess();
                //transform.position = targetPosition;
            }
        }

    }
}
