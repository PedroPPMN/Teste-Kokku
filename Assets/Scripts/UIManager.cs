using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static bool GameIsPause = false;

    GameObject pauseMenu;
    GameObject gameOverMenu;
    GameObject ingameMenu;
    GameObject points;
    GameObject hudScore;

    void Start()
    {
        pauseMenu = GameObject.Find("Canvas/PauseMenu");
        gameOverMenu = GameObject.Find("Canvas/GameOverMenu");
        points = GameObject.Find("Canvas/GameOverMenu/Points/Points_Text");
        ingameMenu = GameObject.Find("Canvas/IngameMenu/SideMenu/");
        hudScore = GameObject.Find("Canvas/IngameMenu/SideMenu/ScorePoints/Text");

        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GameOver(int value)
    {
        Time.timeScale = 0f;
        points.GetComponent<TMPro.TextMeshProUGUI>().text = value.ToString();
        pauseMenu.SetActive(false);
        ingameMenu.SetActive(false);
        gameOverMenu.SetActive(true);
    }

    public void UpdatePointsIngame(int score)
    {
        hudScore.GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();
    }

}
