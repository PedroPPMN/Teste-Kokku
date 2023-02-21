using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    #region prefabs
    GameObject defaultcube;
    GameObject iFormation;
    GameObject lFormation;
    GameObject oFormation;
    GameObject tFormation;
    GameObject zFormation;
    #endregion

    #region game object references
    private Transform board;
    private Transform pieces;
    private Transform boundaries;
    #endregion

    #region tetris configuration
    public static float velocity = 1f;
    public static float sprintVelocity = 0.1f;
    public static int boardWidth = 15, boardHeight = 30;

    [HideInInspector]
    public GameObject[] blocks;
    [HideInInspector]
    public Transform[,] grid = new Transform[boardWidth, boardHeight];

    Dictionary<int, GameObject> dicPieces = new Dictionary<int, GameObject>();

    GameObject storage;
    GameObject actual;
    #endregion

    #region points
    private int points = 0;
    private int lineValue = 100;
    [HideInInspector]
    public float multi = 1.0f;
    #endregion

    AudioManager am;
    UIManager ui;

    private GameObject storageReferance;

    public GameObject StorageReferance { get => storageReferance; set => storageReferance = value; }

    void Start()
    {
        iFormation = Resources.Load<GameObject>("Prefabs/I");
        dicPieces.Add(0, iFormation);
        lFormation = Resources.Load<GameObject>("Prefabs/L");
        dicPieces.Add(1, lFormation);
        oFormation = Resources.Load<GameObject>("Prefabs/O");
        dicPieces.Add(2, oFormation);
        tFormation = Resources.Load<GameObject>("Prefabs/T");
        dicPieces.Add(3, tFormation);
        zFormation = Resources.Load<GameObject>("Prefabs/Z");
        dicPieces.Add(4, zFormation);

        StorageReferance = GameObject.Find("Pieces/Storage");

        board = GameObject.Find("Tetris_Board").transform;
        pieces = GameObject.Find("Pieces").transform;

        am = FindObjectOfType<AudioManager>();
        ui = FindObjectOfType<UIManager>();

        SpawnPiece();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ui.PauseGame();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangePiece();
        }
    }

    public void SpawnPiece()
    {
        int index = Random.Range(0, dicPieces.Count);
        actual = Instantiate(dicPieces[index], pieces);
    }

    public void ChangePiece()
    {
        if (storage == null)
        {
            actual.GetComponent<Moviment>().stored = true;
            storage = actual;
            actual.transform.position = new Vector3(18, -17, 0);
            SpawnPiece();
        }
        else
        {
            var aux = storage;
            storage.GetComponent<Moviment>().RemoveFromStorage(actual.transform.position);
            storage = actual;
            actual = aux;
            actual.GetComponent<Moviment>().stored = false;
            storage.GetComponent<Moviment>().stored = true;
        }
    }

    public void CheckBoard()
    {
        for (int y = boardHeight - 1; y >= 0; y--)
        {
            if (CheckLine(y))
            {
                DestroyLine(y);
                DropLines(y);
            }
        }
    }

    bool CheckLine(int y)
    {
        for (int x = 0; x < boardWidth; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    public void DestroyLine(int y)
    {
        for (int x = 0; x < boardWidth; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
        UpdatePoints();
    }

    public void DropLines(int y)
    {
        for (int i = y; i < boardHeight; i++)
        {
            for (int x = 0; x < boardWidth; x++)
            {
                if (grid[x, i] != null)
                {
                    grid[x, i - 1] = grid[x, i];
                    grid[x, i] = null;
                    grid[x, i - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    public void UpdatePoints()
    {
        multi += 1;
        points += ScoreCalculation(lineValue, multi);
        ui.UpdatePointsIngame(points);
        am.ChangeMusicVelocity((float)(1.0f + (multi / 10)));
    }

    public int ScoreCalculation(int score, float multi)
    {
        return ((int)(score * multi));
    }

    public void GameOver()
    {
        ui.GameOver(points);
    }
}
