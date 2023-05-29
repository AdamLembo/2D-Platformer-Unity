using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject controlsPanel;
    public GameObject winPanel;
    public bool isPaused;
    public int scoreCounter = 0;
    public bool canWin = false;
    private GameMaster GM;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        controlsPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);

        if (isPaused)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1.0f;
            isPaused = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0.0f;
            isPaused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }



    public void OnClickControls()
    {
        controlsPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void OnClickBackControls()
    {
        controlsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void CheckScore()
    {
        if(scoreCounter >= 30)
        {
            canWin = true;
        }
    }

    public void WinGame()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnClickPlayAgain()
    {
        GM.lastCheckpointPosition = new Vector2(-10.9499998f, 5.82999992f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        winPanel.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
