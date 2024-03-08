using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class piece : MonoBehaviour
{
    public int x;
    public int y;
    public board board;

    public enum type
    {
        elephant,
        giraffe,
        hipoo,
        monkey,
        panda,
        parrot,
        penguin,
        pig,
        rabbit,
        snake
    }
    public type pieceType;

    public void Setup(int x_, int y_, board board_)
    {
        this.x = x_;
        this.y = y_;
        this.board = board_;

    }
    public void Move(int desX, int desY)
    {
        transform.DOMove(new Vector3(desX, desY, -5), 0.25f).SetEase(Ease.InOutCubic).onComplete = () =>
        {
            x = desX;
            y = desY;
        };
    }
    [ContextMenu("TextMenu")]
    public void MoveTest()
    {
        Move(0, 0);
    }
    void Start()
    {

    }



    void Update()
    {
       checkBoard();


    }
    void checkBoard()
    {
        // check x
        checkX();
        // check y
        checkY();
    }
    void checkX()
    {
        //comprueba si la pieza no es nula y si es del mismo tipo
        if ( board.GetPiece(x + 1, y) != null && board.GetPiece(x - 1, y) != null &&   board.GetPiece(x + 1, y).pieceType == pieceType && board.GetPiece(x - 1, y).pieceType == pieceType)
        {
            // en caso de que las piezas coinciden se destruyen
            Destroy(board.GetPiece(x + 1, y).gameObject);
            Destroy(board.GetPiece(x - 1, y).gameObject);
            Destroy(gameObject);
        }
    }
    void checkY()
    //comprueba si la pieza no es nula y si es del mismo tipo
    {
        if (board.GetPiece(x, y + 1) != null && board.GetPiece(x, y - 1) != null && board.GetPiece(x, y + 1).pieceType == pieceType && board.GetPiece(x, y - 1).pieceType == pieceType)
        {
            // en caso de que las piezas coinciden se destruyen
            Destroy(board.GetPiece(x, y + 1).gameObject);
            Destroy(board.GetPiece(x, y - 1).gameObject);
            Destroy(gameObject);
        }
    }
}
