using JetBrains.Annotations;
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
    public bool primerMovimiento = false;
    [SerializeField] int puntos = 0;
    bool puedeDestruir = false;

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
        boardCheckPieces();
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
        if (startTile != null && endTile != null && isCloseTo(startTile, endTile))
        {

           
          
                
                swapTiles();
            
        }
    }
    //mover piezas
    private void swapTiles()
    {
        var startPiece = pieces[startTile.x, startTile.y];
        var endPiece = pieces[endTile.x, endTile.y];

        startPiece.Move(endTile.x, endTile.y);
        endPiece.Move(startTile.x, startTile.y);
        pieces[startTile.x, startTile.y] = endPiece;
        pieces[endTile.x, endTile.y] = startPiece;

        primerMovimiento = true;



    }
    //devolver pieza
    public piece GetPiece(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return null;
        }
        return pieces[x, y];
    }


    public bool isCloseTo(Tile start, Tile end)
    {
        if (Math.Abs((start.x - end.x)) == 1 && start.y == end.y)
        {
            return true;
        }
        if (Math.Abs((start.y - end.y)) == 1 && start.x == end.x)
        {
            return true;
        }
        return false;
    }

    void boardCheckPieces()
    {
        FallDown();
        spawnPiece();
    }

    //caida de piezas
    private void FallDown()
    {
        //comprueba cada fila y columna para detectar fichas nulas, en caso de fichas nulas mueve las superiores para abajo
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (pieces[x, y] == null)
                {
                    for (int pos = y + 1; pos < height; pos++)
                    {
                        if (pieces[x, pos] != null)
                        {
                            pieces[x, pos].Move(x, y);
                            pieces[x, y] = pieces[x, pos];
                            pieces[x, pos] = null;
                            break;
                        }
                    }
                }
            }
        }
    }

    //generacion de piezas
    private void spawnPiece()
    {
        // comprueba la ultima columna en busca de huecos vacios
        for (int i = 0; i < width; i++)
        {
            if (GetPiece(i, height - 1) == null)
            {
                // En caso de encontrar un hueco vacio se genera en la posicion una pieza aleatoria y se añade al array
                var selectedPiece = avalaiblePieces[UnityEngine.Random.Range(0, avalaiblePieces.Length)];
                var o = Instantiate(selectedPiece, new Vector3(i, height - 1, -5), Quaternion.identity);
                o.transform.parent = transform;
                pieces[i, height - 1] = o.GetComponent<piece>();
                pieces[i, height - 1]?.Setup(i, height - 1, this);
            }
        }
    }

    public void addPuntos(int puntos)
    {
        this.puntos = puntos + this.puntos;
    }

    //obsoleto
    /*
    public void checkMatch(int x, int y)
    {
        switch (x)
        {
            case 0:
                //comprueba dos a la derecha
                dosDerecha(x, y);
                break;
            case 1:
                //comprueba izquierda, centro, derecha
                if (centro(x, y) == true) {}
                else
                {
                    //comprueba comprueba x inicial y dos a la derecha
                    dosDerecha(x, y);
                }


                break;
            case 2:
            case 3:
                //conmprueba izquierda, centro inicial y derecha
                if (centro(x, y) == true) { }
                //comprueba dos a la derecha
                else if (dosDerecha(x, y) == true)
                { }
                //comprueba dos a la izquierda
                else if (dosIzquierda(x, y) == true)
                { }




                break;
            case 4:
                //conmprueba izquierda, centro inicial y derecha
                if (centro(x, y) == true) 
                {}
                //comprueba dos a la derecha
                else if (dosIzquierda(x, y) == true)
                {}

                break;
            case 5:
                // comprueba dos a la izquierda
                dosIzquierda(x, y);


                break;


        }
    }

    public bool dosIzquierda(int x, int y)
    {
        Debug.Log("Comprobando dos a la izquierda");
        bool eliminado = false;
        if (pieces[x - 1, y].GetComponent<piece>().pieceType == pieces[x, y].GetComponent<piece>().pieceType && pieces[x - 2, y].GetComponent<piece>().pieceType == pieces[x, y].GetComponent<piece>().pieceType)
        {
            Debug.Log("Son iguales");
            var a = Instantiate(emptyPiece, new Vector3(x, y, -5), Quaternion.identity);
            a.transform.parent = transform;
            Destroy(pieces[x, y].gameObject);
            pieces[x, y] = a.GetComponent<piece>();

            pieces[x, y]?.Setup(x, y, this);

            var b = Instantiate(emptyPiece, new Vector3(x, y, -5), Quaternion.identity);
            b.transform.parent = transform;
            Destroy(pieces[x - 1, y].gameObject);
            pieces[x - 1, y] = b.GetComponent<piece>();

            pieces[x - 1, y]?.Setup(x, y, this);

            var c = Instantiate(emptyPiece, new Vector3(x, y, -5), Quaternion.identity);
            c.transform.parent = transform;
            Destroy(pieces[x - 2, y].gameObject);
            pieces[x - 2, y] = c.GetComponent<piece>();

            pieces[x - 2, y]?.Setup(x, y, this);

            eliminado = true;
        }
        return eliminado;
    }

    public bool dosDerecha(int x, int y)
    {
        Debug.Log("Comprobando dos a la derecha");

        bool eliminado = false;

        if (pieces[x + 1, y].GetComponent<piece>().pieceType == pieces[x, y].GetComponent<piece>().pieceType && pieces[x + 2, y].GetComponent<piece>().pieceType == pieces[x, y].GetComponent<piece>().pieceType)
        {
            Debug.Log("Son iguales");

            var a = Instantiate(emptyPiece, new Vector3(x, y, -5), Quaternion.identity);
            a.transform.parent = transform;
            Destroy(pieces[x + 1, y].gameObject);
            pieces[x + 1, y] = a.GetComponent<piece>();
            pieces[x + 1, y]?.Setup(x, y, this);
            //
            var b = Instantiate(emptyPiece, new Vector3(x, y, -5), Quaternion.identity);
            b.transform.parent = transform;
            Destroy(pieces[x, y].gameObject);
            pieces[x, y] = b.GetComponent<piece>();
            pieces[x, y]?.Setup(x, y, this);
            //
            var c = Instantiate(emptyPiece, new Vector3(x, y, -5), Quaternion.identity);
            c.transform.parent = transform;
            Destroy(pieces[x + 2, y].gameObject);
            pieces[x + 2, y] = c.GetComponent<piece>();
            pieces[x + 2, y]?.Setup(x, y, this);
            eliminado = true;
        }
        return eliminado;
    }

    public bool centro(int x, int y)
    {
        Debug.Log("Comprobando a los lados");

        bool eliminado = false;

        if (pieces[x - 1, y].GetComponent<piece>().pieceType == pieces[x, y].GetComponent<piece>().pieceType && pieces[x + 1, y].GetComponent<piece>().pieceType == pieces[x, y].GetComponent<piece>().pieceType)
        {
            Debug.Log("Son iguales");

            var a = Instantiate(emptyPiece, new Vector3(x, y, -5), Quaternion.identity);
            a.transform.parent = transform;
            Destroy(pieces[x - 1, y].gameObject);
            pieces[x - 1, y] = a.GetComponent<piece>();

            pieces[x - 1, y]?.Setup(x, y, this);
            //
            var b = Instantiate(emptyPiece, new Vector3(x, y, -5), Quaternion.identity);
            b.transform.parent = transform;
            Destroy(pieces[x, y].gameObject);
            pieces[x, y] = b.GetComponent<piece>();

            pieces[x, y]?.Setup(x, y, this);

            //
            var c = Instantiate(emptyPiece, new Vector3(x, y, -5), Quaternion.identity);
            c.transform.parent = transform;
            Destroy(pieces[x + 1, y].gameObject);
            pieces[x + 1, y] = c.GetComponent<piece>();

            pieces[x + 1, y]?.Setup(x, y, this);
            eliminado = true;
        }
        return eliminado;
    }*/

}
