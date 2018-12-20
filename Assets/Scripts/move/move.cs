﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{

    enum DIR { UP, DOWN, LEFT, RIGHT, LUP, RUP, LDOWN, RDOWN };
    public float smoothTime = 1.0F;
    private float scale = 6.0F;
    private Vector3 velocity = Vector3.zero;

    Vector3 targetPosition;
    Vector2Int boardPosition = Vector2Int.zero;
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
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i] += normals[i] * Mathf.Sin(Time.time);
        }

        mesh.vertices = vertices;
        GetComponent<MeshFilter>().mesh = mesh;

        //Destroy(gameObject);
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
        Vector2Int newBoardPosition = pieces[idx].mCurrentCell.mBoardPosition;

        if(boardPosition.x < 0)
        {
            boardPosition = newBoardPosition;
            return;
        }

        Vector2Int diff = newBoardPosition - boardPosition;
        boardPosition = newBoardPosition;


        Vector3 targetPosition = gameObject.transform.position;
        targetPosition.x -= diff.x * scale;
        targetPosition.z -= diff.y * scale;
        
        // Move 3D object
        if (transform.position != targetPosition)
        {
            transform.position = targetPosition;
            return;
        }

    }
}
