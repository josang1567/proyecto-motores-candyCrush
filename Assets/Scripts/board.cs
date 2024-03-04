using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tileObject;
    public float cameraSizeOffset;
    public float cameraVerticalOffset;
    public GameObject[] avalaiblePieces;
    Tile[,] tiles;
    piece[,] pieces;
    Tile startTile;
    Tile endTile;
    void Start()
    {
        tiles = new Tile[width, height];
        pieces = new piece[width, height];
        setupBoard();
        setupPieces();
        positionCamera();
    }

    void Update()
    {

    }
    void setupBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var o = Instantiate(tileObject, new Vector3(x, y, -5), Quaternion.identity);
                o.transform.parent = transform;
                tiles[x, y] = o.GetComponent<Tile>();
                tiles[x, y]?.Setup(x, y, this);
            }
        }
    }
    void setupPieces()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var selectedPiece = avalaiblePieces[UnityEngine.Random.Range(0, avalaiblePieces.Length)];
                var o = Instantiate(selectedPiece, new Vector3(x, y, -5), Quaternion.identity);
                o.transform.parent = transform;
                pieces[x, y] = o.GetComponent<piece>();

                pieces[x, y]?.Setup(x, y, this);
            }
        }
    }
    void positionCamera()
    {
        float newPosY = (float)width / 2f;
        float newPosX = (float)height / 2f;
        Camera.main.transform.position = new Vector3(newPosY - 0.5f, newPosX - 0.5f, -10f);

        float horizontal = width + 1;
        float vertical = (height / 2) + 1;
        Camera.main.orthographicSize = horizontal > vertical ? horizontal + cameraSizeOffset : vertical;
    }


    public void tileDown(Tile tile_)
    {
        startTile = tile_;
    }
    public void tileOver(Tile tile_)
    {
        endTile = tile_;
    }
    public void tileUp(Tile tile_)
    {
        if (startTile != null && endTile != null && isCloseTo(startTile,endTile))
        {
            swapTiles();
        }
    }
    private void swapTiles()
    {
        var startPiece = pieces[startTile.x, startTile.y];
        var endPiece = pieces[endTile.x, endTile.y];

        startPiece.Move(endTile.x, endTile.y);
        endPiece.Move(startTile.x, startTile.y);
        pieces[startTile.x, startTile.y] = endPiece;
        pieces[endTile.x, endTile.y] = startPiece;
    }

    public bool isCloseTo(Tile start, Tile end)
    {
        if (Math.Abs((start.x - end.x)) == 1 && start.y == end.y)
        {
            return true;
        }
        if (Math.Abs((start.y - end.y)) == 1 && start.x == end.x    )
        {
            return true;
        }
        return false;
    }
}
