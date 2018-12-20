﻿using ChessDotNet; 
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;

public class Testing : MonoBehaviour {

    public static ChessGame game;
    public static Dictionary<string, int[]> boardCoords = new Dictionary<string, int[]>()
    {
        { "A1", new[]{-14, 0, -14} },
        { "B1", new[]{-10, 0, -14} },
        { "C1", new[]{-6, 0, -14} },
        { "D1", new[]{-2, 0, -14} },
        { "E1", new[]{2, 0, -14} },
        { "F1", new[]{6, 0, -14} },
        { "G1", new[]{10, 0, -14} },
        { "H1", new[]{14, 0, -14} },
        { "A2", new[]{-14, 0, -10} },
        { "B2", new[]{-10, 0, -10} },
        { "C2", new[]{-6, 0, -10} },
        { "D2", new[]{-2, 0, -10} },
        { "E2", new[]{2, 0, -10} },
        { "F2", new[]{6, 0, -10} },
        { "G2", new[]{10, 0, -10} },
        { "H2", new[]{14, 0, -10} },
        { "A3", new[]{-14, 0, -6} },
        { "B3", new[]{-10, 0, -6} },
        { "C3", new[]{-6, 0, -6} },
        { "D3", new[]{-2, 0, -6} },
        { "E3", new[]{2, 0, -6} },
        { "F3", new[]{6, 0, -6} },
        { "G3", new[]{10, 0, -6} },
        { "H3", new[]{14, 0, -6} },
        { "A4", new[]{-14, 0, -2} },
        { "B4", new[]{-10, 0, -2} },
        { "C4", new[]{-6, 0, -2} },
        { "D4", new[]{-2, 0, -2} },
        { "E4", new[]{2, 0, -2} },
        { "F4", new[]{6, 0, -2} },
        { "G4", new[]{10, 0, -2} },
        { "H4", new[]{14, 0, -2} },
        { "A5", new[]{-14, 0, 2} },
        { "B5", new[]{-10, 0, 2} },
        { "C5", new[]{-6, 0, 2} },
        { "D5", new[]{-2, 0, 2} },
        { "E5", new[]{2, 0, 2} },
        { "F5", new[]{6, 0, 2} },
        { "G5", new[]{10, 0, 2} },
        { "H5", new[]{14, 0, 2} },
        { "A6", new[]{-14, 0, 6} },
        { "B6", new[]{-10, 0, 6} },
        { "C6", new[]{-6, 0, 6} },
        { "D6", new[]{-2, 0, 6} },
        { "E6", new[]{2, 0, 6} },
        { "F6", new[]{6, 0, 6} },
        { "G6", new[]{10, 0, 6} },
        { "H6", new[]{14, 0, 6} },
        { "A7", new[]{-14, 0, 10} },
        { "B7", new[]{-10, 0, 10} },
        { "C7", new[]{-6, 0, 10} },
        { "D7", new[]{-2, 0, 10} },
        { "E7", new[]{2, 0, 10} },
        { "F7", new[]{6, 0, 10} },
        { "G7", new[]{10, 0, 10} },
        { "H7", new[]{14, 0, 10} },
        { "A8", new[]{-14, 0, 14} },
        { "B8", new[]{-10, 0, 14} },
        { "C8", new[]{-6, 0, 14} },
        { "D8", new[]{-2, 0, 14} },
        { "E8", new[]{2, 0, 14} },
        { "F8", new[]{6, 0, 14} },
        { "G8", new[]{10, 0, 14} },
        { "H8", new[]{14, 0, 14} },
    };


    private Color32 copper = new Color32(223, 141, 56, 255);
    public int modifier = 1;

    private string command;
    private string selected;
    protected GameObject highlightPiece = null;


    static GameObject getSumireko(string square)
    {
        GameObject violetDetector = GameObject.Find(square);
        Collider[] hitColliders = Physics.OverlapSphere(violetDetector.transform.position, 1);


        for (int i = 0; i < hitColliders.Count(); i++)
        {
            if ((hitColliders[i].gameObject.transform.parent.name == "WhitePieces") || (hitColliders[i].gameObject.transform.parent.name == "BlackPieces"))
            {
                return hitColliders[i].gameObject;
            }
        }

        return null; 
    }


    public void movePiece(string current, string target) // Moves piece from current to target
    {
        Player currentPlayer = game.WhoseTurn;
        Move move = new Move(current, target, currentPlayer);
        bool isValid = game.IsValidMove(move);

        if (isValid) // If move if valid, detect piece on current square and send it to target square
        {
            MoveType type = game.ApplyMove(move, true);

            if ((type & MoveType.Capture) == MoveType.Capture) // If movement kills another unit, send it to the shadow realm
            {
                GameObject capturedPiece = getSumireko(target);
                capturedPiece.transform.Translate(Vector3.forward * 100);
            }

            GameObject movingPiece = getSumireko(current);
            movingPiece.transform.Translate(Vector3.up * (boardCoords[target][2] - movingPiece.transform.position.z) * modifier);
            movingPiece.transform.Translate(Vector3.right * (boardCoords[target][0] - movingPiece.transform.position.x) * modifier);
            Debug.Log($"{type} movement by {currentPlayer} from {current} to {target} was successful.");
        }
        else
        {
            Debug.Log($"Movement by {currentPlayer} from {current} to {target} was not possible.");
        }

        

        Piece[][] currentBoard = game.GetBoard();
        int i = 0;
        int j = 0;

        if (game.IsInCheck(game.WhoseTurn) == true)
        {}

        bool gameOver = false;
        if (game.IsCheckmated(game.WhoseTurn) == true)
        {
            gameOver = true;
        }

        if (game.IsDraw() == true)
        {
            gameOver = true;
        }

        if (gameOver == true)
        {}
    }

	// Use this for initialization
	void Start () {
        game = new ChessGame();

        this.movePiece("A2", "A3"); // W1
        this.movePiece("C7", "C5"); // B1
        this.movePiece("F2", "F3"); // W2
        this.movePiece("G7", "G5"); // B2
        this.movePiece("D2", "D4"); // W3
        this.movePiece("F7", "F6"); // B3
        this.movePiece("B2", "B3"); // W4
        this.movePiece("G8", "H6"); // B4
        this.movePiece("C1", "G5"); // W5

        command = "";
       
     

     /*   Piece pieceAtA1 = game.GetPieceAt(new Position("A1")); 
        Console.WriteLine("What piece is there at A1? {0}", pieceAtA1.GetFenCharacter());
        
        Move e2e4 = new Move("E2", "E4", Player.White);
        bool isValid = game.IsValidMove(e2e4);

        
        MoveType type = game.ApplyMove(e2e4, true);
       
        Debug.Log(game.GetPieceAt(File.E, 4));
        moveWPawn5.MoveYoAss();
        */
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.anyKeyDown) // Gathers an input command from keyboard ~ possibly more efficient method available
        {
            command += Input.inputString;
            command = command.ToUpper();
            Debug.Log(command);
        }
        

        if ((command.Length == 2) && (Regex.IsMatch(command, @"^[A-H][1-8]$"))) // Activates violetDetector if command if valid
        {
            if (highlightPiece != null) // On second valid input, attempts movement to target square and de-highlights piece
            {
                this.movePiece(selected, command);

                Renderer rend = highlightPiece.GetComponent<Renderer>();

                if (highlightPiece.name.Contains("White"))
                {
                    rend.material.color = copper;
                }
                else if (highlightPiece.name.Contains("Black"))
                {
                    rend.material.color = Color.black;
                }
                highlightPiece = null;
                selected = "";
                command = "";
            }
            else
            {
                highlightPiece = getSumireko(command);
                if (highlightPiece != null) // Highlights target piece, if selected violetDetector detects a piece
                {
                    Renderer rend = highlightPiece.GetComponent<Renderer>();

                    if (highlightPiece.name.Contains("White"))
                    {
                        rend.material.color = Color.green;
                    }
                    else if (highlightPiece.name.Contains("Black"))
                    {
                        rend.material.color = Color.red;
                    }
                }
                selected = command;
                command = "";
            }

            
        }
        else if (command.Length >= 2)
        {
            command = command.Remove(0, 1);
        }

    }
}
