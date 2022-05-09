using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Board
{
    //  int array of length 64 to store board position.
    public int[] boardArray = new int[64];
    
    //  store number of moves played, start at 1, increment after Black's move
    public int fullMoveNumber;

    //  store number of moves since last capture or pawn advance, used for fifty move rule
    public int halfMoveClock;

    //  store whose turn it is
    public char activeColour;

    //  Queen and kingside castling rights for each side
    public bool whiteKingsideCastle;
    public bool whiteQueensideCastle;

    public bool blackKingsideCastle;
    public bool blackQueensideCastle;

    //  Square behind pawn that has just advanced 2 squares, signalling a possible "en passant" capture. -1 if no En Passant possible
    public int enPassantSquare;

    //  Playing, WhiteIsMated, BlackIsMated, StaleMate, ThreeFoldRepition, FiftyMoveRule, InsufficientMaterial
    public State gameState;

    //  List of fen strings from each position
    public List<string> gameHistory; //  Edit#########################
    public int repetitionLimiter; //  To reduce search size for threefold repetition


    public void SwitchActiveSides(int pieceSquareSelected, int pieceDestination, int promotionPiece = Pieces.None)
    {
        //  If the piece moved was a pawn and/or a piece was captured, reset half move clock. Otherwise, increment.
        if ((boardArray[pieceSquareSelected] & 7) == Pieces.Pawn || boardArray[pieceDestination] != Pieces.None)  
        {
            halfMoveClock = 0;
            repetitionLimiter = gameHistory.Count - 1; //  Only search from this position onwards for repetitions
        }
        else
        {
            halfMoveClock++;
        }

        //  Deal with En Passant, move needs to be added to pawn moves
        if ((boardArray[pieceSquareSelected] & 7) == Pieces.Pawn && Math.Abs(pieceDestination - pieceSquareSelected) == 16)
        {
            if (activeColour == 'w')
            {
                enPassantSquare = pieceDestination + 8;
            }
            else
            {
                enPassantSquare = pieceDestination - 8;
            }
            
        }
        else
        {
            enPassantSquare = -1;
        }
        

        //  If move selected was to castle, update rook position
        if (boardArray[pieceSquareSelected] == (Pieces.King | Pieces.White))
        {
            if (pieceSquareSelected == 60 && pieceDestination == 62)
            {
                boardArray[61] = Pieces.Rook | Pieces.White;
                boardArray[63] = Pieces.None;
            }
            else if (pieceSquareSelected == 60 && pieceDestination == 58)
            {
                boardArray[59] = Pieces.Rook | Pieces.White;
                boardArray[56] = Pieces.None;
            }
        }
        else if (boardArray[pieceSquareSelected] == (Pieces.King | Pieces.Black))
        {
            if (pieceSquareSelected == 4 && pieceDestination == 6)
            {
                boardArray[5] = Pieces.Rook | Pieces.Black;
                boardArray[7] = Pieces.None;
            }
            else if (pieceSquareSelected == 4 && pieceDestination == 2)
            {
                boardArray[3] = Pieces.Rook | Pieces.Black;
                boardArray[0] = Pieces.None;
            }
        }
        
        //  Update board array
        
        //  If En Passant, remove captured pawn
        if ((boardArray[pieceSquareSelected] & 7) == Pieces.Pawn && pieceSquareSelected % 8 != pieceDestination % 8 && boardArray[pieceDestination] == Pieces.None)
        {
            if (activeColour == 'w')
            {
                boardArray[pieceDestination + 8] = Pieces.None;
            }
            else
            {
                boardArray[pieceDestination - 8] = Pieces.None;
            }
        }

        //  Deal with pawn promotion
        if (promotionPiece == Pieces.None)
        {
            boardArray[pieceDestination] = boardArray[pieceSquareSelected];
            boardArray[pieceSquareSelected] = Pieces.None;
        }
        else
        {
            boardArray[pieceDestination] = promotionPiece;
            boardArray[pieceSquareSelected] = Pieces.None;
        }
        
        

        //  Deal with castle rights
        if (whiteKingsideCastle)
        {
            if (boardArray[63] != (Pieces.Rook | Pieces.White) || boardArray[60] != (Pieces.King | Pieces.White))
            {
                whiteKingsideCastle = false;
                repetitionLimiter = gameHistory.Count - 1; //  Only search from this position onwards for repetitions
            }
        }
        if (whiteQueensideCastle)
        {
            if (boardArray[56] != (Pieces.Rook | Pieces.White) || boardArray[60] != (Pieces.King | Pieces.White))
            {
                whiteQueensideCastle = false;
                repetitionLimiter = gameHistory.Count - 1; //  Only search from this position onwards for repetitions
            }
        }
        if (blackKingsideCastle)
        {
            if (boardArray[7] != (Pieces.Rook | Pieces.Black) || boardArray[4] != (Pieces.King | Pieces.Black))
            {
                blackKingsideCastle = false;
                repetitionLimiter = gameHistory.Count - 1; //  Only search from this position onwards for repetitions
            }
        }
        if (blackQueensideCastle)
        {
            if (boardArray[0] != (Pieces.Rook | Pieces.Black) || boardArray[4] != (Pieces.King | Pieces.Black))
            {
                blackQueensideCastle = false;
                repetitionLimiter = gameHistory.Count - 1; //  Only search from this position onwards for repetitions
            }
        }

        //  Switch active side and increment full move count if black just made a move
        if (activeColour == 'w')
        {
            activeColour = 'b';
        }
        else
        {
            fullMoveNumber++;  //  Black just made a move, hence increment full move count
            activeColour = 'w';
        }

        //  Deal with history
        gameHistory.Add(FenString.BoardToFenString(this));

        //  Deal with Game State / End Conditions
        if (EndConditions.IsCheckMate(this))
        {
            if (activeColour == 'w')
            {
                gameState = State.WhiteIsMated;
            }
            else
            {
                gameState = State.BlackIsMated;
            }
        }
        if (EndConditions.IsStaleMate(this))
        {
            gameState = State.StaleMate;
        }
        if (EndConditions.IsInsufficientMaterial(this))
        {
            gameState = State.InsufficientMaterial;
        }
        if (EndConditions.IsFiftyMoveRule(this))
        {
            gameState = State.FiftyMoveRule;
        }
        if (gameHistory.Count - repetitionLimiter >= 9)
        {
            if (EndConditions.IsThreefoldRepetition(this))
            {
                gameState = State.ThreeFoldRepetition;
            }  
        }
        
    }

    public enum State { Playing, WhiteIsMated, BlackIsMated, StaleMate, ThreeFoldRepetition, FiftyMoveRule, InsufficientMaterial };

    public int GetActiveColourAsInt ()
    {
        if (activeColour == 'w')
        {
            return Pieces.White;
        }
        else
        {
            return Pieces.Black;
        }
    }
}
