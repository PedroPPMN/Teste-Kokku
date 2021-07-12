using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moviment : MonoBehaviour
{
    bool canMove = true;
    [HideInInspector]
    public bool stored = false;
    float timer = 0f;
    GameController gc;
    AudioManager am;

    [HideInInspector]
    public GameObject block;

    void Start()
    {
        gc = FindObjectOfType<GameController>();
        am = FindObjectOfType<AudioManager>();
        block = this.gameObject;
    }

    private bool CheckPiece()
    {
        foreach (Transform piece in block.transform)
        {
            if ((piece.transform.position.x < 0) ||
                (piece.transform.position.y < 0) ||
                (piece.transform.position.x) >= (GameController.boardWidth))
            {
                return false;
            }
            if (piece.position.y < GameController.boardHeight && gc.grid[Mathf.FloorToInt(piece.position.x), Mathf.FloorToInt(piece.position.y)] != null)
            {
                return false;
            }
        }
        return true;
    }

    void AddBlockToMatrix()
    {
        foreach (Transform piece in block.transform)
        {
            if (gc.grid[Mathf.FloorToInt(piece.position.x), Mathf.FloorToInt(piece.position.y)] == null)
            {
                gc.grid[Mathf.FloorToInt(piece.position.x), Mathf.FloorToInt(piece.position.y)] = piece;
            }
            else
            {
                gc.GameOver();
            }

        }
    }

    public void RemoveFromStorage(Vector3 position)
    {
        this.transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        if (stored)
        {
            this.transform.position = new Vector3(23, 12, 0);
        }

        if (canMove && !stored)
        {
            timer += gc.multi * Time.deltaTime;

            if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && timer > GameController.sprintVelocity)
            {
                am.PlayMovePiece();
                gameObject.transform.position -= new Vector3(0, 1, 0);
                timer = 0;
                if (!CheckPiece())
                {
                    am.PlayMovePiece();
                    canMove = false;
                    gameObject.transform.position += new Vector3(0, 1, 0);
                    AddBlockToMatrix();
                    gc.CheckBoard();
                    gc.SpawnPiece();
                }
            }
            else if (timer > GameController.velocity)
            {
                gameObject.transform.position -= new Vector3(0, 1, 0);
                timer = 0;
                if (!CheckPiece())
                {
                    canMove = false;
                    gameObject.transform.position += new Vector3(0, 1, 0);
                    AddBlockToMatrix();
                    gc.CheckBoard();
                    gc.SpawnPiece();
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                am.PlayMovePiece();
                gameObject.transform.position -= new Vector3(1, 0, 0);
                if (!CheckPiece())
                {
                    am.PlayMovePiece();
                    gameObject.transform.position += new Vector3(1, 0, 0);
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                am.PlayMovePiece();
                gameObject.transform.position += new Vector3(1, 0, 0);
                if (!CheckPiece())
                {
                    am.PlayMovePiece();
                    gameObject.transform.position -= new Vector3(1, 0, 0);
                }
            }
            else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
            {
                am.PlayMovePiece();
                block.transform.eulerAngles -= new Vector3(0, 0, 90);
                if (!CheckPiece())
                {
                    am.PlayMovePiece();
                    block.transform.eulerAngles += new Vector3(0, 0, 90);
                }
            }
        }
    }
}