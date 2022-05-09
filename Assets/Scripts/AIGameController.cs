using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIGameController : MonoBehaviour
{
    int AIColour;
    public static Board board;
    bool activeSquareSelected;
    int activeSquareHighlighted;
    int promotionSquare;
    GameObject canvasObject;
    GameObject textObject;
    Text endConditionText;
    RectTransform rectTransform;

    private void Awake()
    {
        canvasObject = GameObject.Find("Canvas");
        textObject = new GameObject("TextObject");
        textObject.transform.parent = canvasObject.transform;
        endConditionText = textObject.AddComponent<Text>();
        rectTransform = textObject.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 230);
        rectTransform.localScale = Vector3.one;
        endConditionText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        endConditionText.fontSize = 40;
        endConditionText.horizontalOverflow = HorizontalWrapMode.Overflow;
        endConditionText.verticalOverflow = VerticalWrapMode.Overflow;
        endConditionText.alignment = TextAnchor.MiddleCenter;
    }

    private void OnEnable()
    {
        SquareInputManager.OnSquareClick += OnSquareClickManageBoard;
        PromotionInputManager.OnPromotionSquareClick += OnPawnPromotionManageBoard;
    }

    private void OnDisable()
    {
        SquareInputManager.OnSquareClick -= OnSquareClickManageBoard;
        PromotionInputManager.OnPromotionSquareClick -= OnPawnPromotionManageBoard;
    }

    void Start()
    {
        board = FenString.FenStringToBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        activeSquareSelected = false;
        DrawPieces.DrawPiecesFromArray(board.boardArray);
    }

    private void Update()
    {
        
    }

    void OnSquareClickManageBoard(int i)
    {
        if (board.gameState == Board.State.Playing)
        {
            if (!activeSquareSelected)  //  If there is no selected active square
            {
                if (ActivePieces.GetActivePieceSquares(board).Contains(i))  //  If the clicked square is an active piece
                {
                    Debug.Log("Active piece clicked");
                    //  Active piece is now selected
                    activeSquareSelected = true;
                    activeSquareHighlighted = i;

                    //  Highlight square and show moves
                    HighlightBoard.HighlightActivePieceSquare(i);
                    HighlightBoard.HighlightLegalMoves(board, i);
                }
            }
            else  //  If there is a selected active square
            {
                if (LegalMoves.GetLegalMovesForSquare(board, activeSquareHighlighted).Contains(i))  //  If they have selected a legal move
                {
                    if ((board.boardArray[activeSquareHighlighted] & 7) == Pieces.Pawn && (i / 8 == 7 || i / 8 == 0))  //  Handle pawn promotion
                    {
                        //  Disable usual input, enable promotion menu input
                        GetComponent<SquareInputManager>().enabled = false;
                        GetComponent<PromotionInputManager>().enabled = true;
                        Promotion.ActivatePromotionMenu(i);
                        promotionSquare = i;
                        activeSquareSelected = false;
                        HighlightBoard.UnhighlightActivePieceSquare(activeSquareHighlighted);
                        HighlightBoard.UnHighlightLegalMoves(board, activeSquareHighlighted);
                        return;
                    }
                    //  Deselect square
                    activeSquareSelected = false;

                    //  Unhighlight square and legal moves
                    HighlightBoard.UnhighlightActivePieceSquare(activeSquareHighlighted);
                    HighlightBoard.UnHighlightLegalMoves(board, activeSquareHighlighted);

                    //  Update board, change sides, increment clocks etc.
                    board.SwitchActiveSides(activeSquareHighlighted, i);
                    DrawPieces.ClearBoard();
                    DrawPieces.DrawPiecesFromArray(board.boardArray);

                    //  React if end conditions have been reached
                    OnEndCondition();

                    if (board.gameState == Board.State.Playing)
                    {
                        (int squareOfPieceToMove, int squareToMoveTo, int promotionPiece) = RandomAI.GenerateMove(board);
                        Debug.Log(squareOfPieceToMove + " " + squareToMoveTo);
                        board.SwitchActiveSides(squareOfPieceToMove, squareToMoveTo, promotionPiece);
                        DrawPieces.ClearBoard();
                        DrawPieces.DrawPiecesFromArray(board.boardArray);

                        //  React if end conditions have been reached
                        OnEndCondition();
                    }
                }
                else  //  They have clicked a square that isn't a legal move
                {
                    //  Deselect square
                    activeSquareSelected = false;

                    //  Unhighlight square and moves
                    HighlightBoard.UnhighlightActivePieceSquare(activeSquareHighlighted);
                    HighlightBoard.UnHighlightLegalMoves(board, activeSquareHighlighted);

                    if (ActivePieces.GetActivePieceSquares(board).Contains(i))  //  They have selected a different active piece
                    {
                        //  Active square selected
                        activeSquareSelected = true;
                        activeSquareHighlighted = i;

                        //  Highlight square and show legal moves
                        HighlightBoard.HighlightActivePieceSquare(i);
                        HighlightBoard.HighlightLegalMoves(board, i);
                    }
                }
            }
        }
    }

    private void OnPawnPromotionManageBoard(int promotionPieceChoice)
    {
        board.SwitchActiveSides(activeSquareHighlighted, promotionSquare, promotionPieceChoice);

        DrawPieces.ClearBoard();
        DrawPieces.DrawPiecesFromArray(board.boardArray);

        //  React if end conditions have been reached
        OnEndCondition();

        Promotion.DestroyPromotionMenu();
        GetComponent<SquareInputManager>().enabled = true;
        GetComponent<PromotionInputManager>().enabled = false;
    }

    private void OnEndCondition()
    {
        if (board.gameState == Board.State.WhiteIsMated)
        {
            endConditionText.text = "Black wins by checkmate";
        }
        else if (board.gameState == Board.State.BlackIsMated)
        {
            endConditionText.text = "White wins by checkmate";
        }
        else if (board.gameState == Board.State.StaleMate)
        {
            endConditionText.text = "Draw by Stalemate";
        }
        else if (board.gameState == Board.State.ThreeFoldRepetition)
        {
            endConditionText.text = "Draw by threefold repition";
        }
        else if (board.gameState == Board.State.FiftyMoveRule)
        {
            endConditionText.text = "Draw by fifty move rule";
        }
        else if (board.gameState == Board.State.InsufficientMaterial)
        {
            endConditionText.text = "Draw by insufficient material";
        }
    }

}
