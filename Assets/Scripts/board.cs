using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tileObject;
    public float cameraSizeOffset;
    public float cameraVerticalOffset;

    void Start()
    {
        setupBoard();
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
                var o = Instantiate(tileObject, new Vector3(x,y,-5),Quaternion.identity);
                o.transform.parent = transform;
            }
        }
    }
    void positionCamera()
    {
        float newPosY = (float)width / 2f;
        float newPosX = (float)height / 2f;
        Camera.main.transform.position = new Vector3(newPosY-0.5f, newPosX-0.5f, -10f);

        float horizontal = width + 1;
        float vertical = (height/2) + 1;
        Camera.main.orthographicSize=horizontal>vertical?horizontal+cameraSizeOffset:vertical;
    }
}
