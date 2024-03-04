using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;
    public board board;
    public void Setup(int x_, int y_, board board_)
    {
        this.x = x_;
        this.y = y_;
        this.board = board_;

    }
    private void OnMouseDown()
    {
        board.tileDown(this);
    }
    private void OnMouseEnter()
    {
        board.tileOver(this);
    }
    private void OnMouseUp()
    {
        board.tileUp(this);
    }
}
