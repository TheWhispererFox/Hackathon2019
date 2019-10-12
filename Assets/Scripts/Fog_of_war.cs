using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FOV : MonoBehaviour
{

    [HideInInspector] public GameObject fovPanel;
    public GameObject fovTile; //That's your previously created Fog of War tile - assign in editor!

    void Awake()
    {
        fovPanel = GameObject.Find("FOVPanel");
    }

    // max revealed tile depth (from start)
    public const int MAX_RANGE = 5;

    /*
     level - how far we've already gone with our raycasting
     x - current tile x position
     y - current tile y position
     directionX - make sure it's -1, 0 or 1 (0 if directionY is not 0)
     directionY - make sure it's -1, 0 or 1 (0 if directionX is not 0)
    */
    public void RayCasting(int level,
                           int x,
                           int y,
                           int directionX,
                           int directionY,
                           bool leftCorner,
                           bool rightCorner)
    {
        // that's the board script: it handles all the board logics
        BoardManager board = GameManager.instance.boardScript;
        // continue only if we have not surpassed the max distance
        if (level < MAX_RANGE)
        {
            // if we are out of bounds
            if (x < 0 || y < 0 || x >= board.maxX || y >= board.maxY)
            {
                return;
            }
            else
            {
                // ok, we're here, mark the tile as revealed
                board.revealedGrid[x, y] = true;
                // but does the tile blocks visibility? if visibilityGrid[x, y] is true, then player can see through the tile
                // what's behind it
                if (board.visibilityGrid[x, y])
                {
                    // our ray expands gradually. therefore if we are located on the corner tiles
                    // of the current ray level - we need to expand further on the next level
                    if (leftCorner)
                    {
                        if (directionX != 0)
                        {
                            // if we are moving horizontally, expand ray below or above (that's why we adjust y by directionX)
                            // the left corner tile ray would go something like that
                            // level
                            // 0 1 2
                            // . . *
                            // . * *
                            // * * .
                            RayCasting(level + 1, x + directionX, y + directionX, directionX, directionY, leftCorner, false);
                        }
                        else
                        {
                            // else
                            RayCasting(level + 1, x + directionY, y + directionY, directionX, directionY, leftCorner, false);
                        }
                    }

                    // same as with left corner
                    if (rightCorner)
                    {
                        if (directionX != 0)
                        {
                            RayCasting(level + 1, x + directionX, y - directionX, directionX, directionY, rightCorner, false);
                        }
                        else
                        {
                            RayCasting(level + 1, x - directionY, y + directionY, directionX, directionY, rightCorner, false);
                        }
                    }

                    // we raycast forward in any case
                    // level
                    // 0 1 2
                    // . . .
                    // * * *
                    // . . .
                    RayCasting(level + 1, x + directionX, y + directionY, directionX, directionY, leftCorner, false);
                }
            }
        }
        else
        {
            return;
        }
    }

    public void Recalculate(int x, int y)
    {
        BoardManager board = GameManager.instance.boardScript;
        // the player moves here
        board.revealedGrid[x, y] = true;

        // raycast to all directions
        RayCasting(0, x, y, 0, 1, true, true);
        RayCasting(0, x, y, 0, -1, true, true);
        RayCasting(0, x, y, 1, 0, true, true);
        RayCasting(0, x, y, -1, 0, true, true);
    }


    protected const int MAX_X_LOOKUP = 20;
    protected const int MAX_Y_LOOKUP = 10;

    // redraw based on position of camera 
    // (currentX and currentY most often is the center of the screen, where player stands)
    public void Redraw(int currentX, int currentY)
    {
        // redraw the Fog of War tiles
        List<GameObject> children = new List<GameObject>();
        // first we gather all children of FOVPanel
        foreach (Transform child in fovPanel.transform)
        {
            children.Add(child.gameObject);
        }

        // then we delete them
        foreach (GameObject child in children)
        {
            Destroy(child);
        }

        BoardManager board = GameManager.instance.boardScript;

        // then we draw new Fog of War tiles
        for (int x = -MAX_X_LOOKUP + currentX; x < MAX_X_LOOKUP + currentX + 1; x++)
        {
            for (int y = -MAX_Y_LOOKUP + currentY; y < MAX_Y_LOOKUP + currentY; y++)
            {
                if (x < 0 || y < 0 || x > board.maxX - 1 || y > board.maxY - 1)
                {
                    continue;
                }


                if (board.revealedGrid[x, y] == false)
                {
                    Vector3 pos = new Vector2(x, y);
                    GameObject obj = Instantiate(fovTile, pos, Quaternion.identity) as GameObject;
                    obj.transform.parent = fovPanel.transform;
                }
            }
        }
    }
}