using UnityEngine.UI;
using System;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    public TMP_Text congText = null;
    public TMP_Text graphicQualityText = null;
    public TMP_Text scoreText = null;
    public TMP_Text endingScoreText = null;
    public TMP_Text highScoreText = null;
    public TMP_Text enemiesText = null;
    public TMP_Text timerText = null;
    public Canvas inGameCanvas = null;
    public static GameManager instance;
    public GameState currentGameState;
    public Canvas pauseMenuCanvas;
    public Canvas levelCompletedCanvas;
    public Canvas optionsCanvas;
    const String keyHighScore = "HighScoreLevel1";
    public enum GameState
    {
        [InspectorName("Gameplay")] GAME,
        [InspectorName("Pause")] PAUSE_MENU,
        [InspectorName("Level completed (either successfully or failed)")] LEVEL_COMPLETED,
        [InspectorName("Options")] OPTIONS,
    }
    private int score = 0;
    private int enemiesFelled = 0;
    public Image[] keysArray = null;
    public Image[] livesArray = null;
    private int keysFound = 0;
    private int lives = 3;
    private float timer = 0;

    public int GetKeys()
    {
        return keysFound;
    }
    public int GetLives()
    {
        return lives;
    }

    public bool removeLife()
    {
        if (lives > 1)
        {
            lives--;                                                        // usun zycie
            livesArray[lives].color = new Color(0.245f, 0.245f, 0.245f);    // zmien kolor i-tej ikonki na szary
            return true;                                                    // >= 0 zyc
        }
        else if(lives == 1)
        {
            lives--;                                                        // usun zycie
            livesArray[lives].color = new Color(0.245f, 0.245f, 0.245f);    // zmien kolor i-tej ikonki na szary
            return false;                                                  // 0 zyc - koniec gry
        }
        else
        {
            return false;                                                  // 0 zyc - koniec gry
        }
    }
    public void AddLives(Collider2D colid)
    {
        if (lives < 3)
        {
            livesArray[lives].color = new Color(1f, 1f, 1f);
            lives++;
            colid.gameObject.SetActive(false);
        }
    }
    public void AddKeys(string name)
    {
        switch (name[name.Length - 1])
        {
            case 'R':
                keysArray[0].color = new Color(0.81f, 0.29f, 0.29f);
                break;
            case 'G':
                keysArray[1].color = new Color(0.1f, 1.0f, 0);
                break;
            case 'B':
                keysArray[2].color = new Color(0.24f, 0.44f, 0.89f);
                break;
        }
        keysFound++;
    }
    public void AddPoints(int points)
    {
        score += points;
        if (score < 10)
        {
            scoreText.text = ("00" + score.ToString());
            // dodaj dwa zera
        }
        else if (score < 100)
        {
            // dodaj jedno zero
            scoreText.text = ("0" + score.ToString());
        }
        else
        {
            // nie dodawaj zer
            scoreText.text = score.ToString();
        }
    }

    public void AddEnemyKill()
    {
        enemiesFelled++;
        if (enemiesFelled < 10)
        {
            enemiesText.text = ("00" + enemiesFelled.ToString());
            // dodaj dwa zera
        }
        else if (enemiesFelled < 100)
        {
            // dodaj jedno zero
            enemiesText.text = ("0" + enemiesFelled.ToString());
        }
        else
        {
            // nie dodawaj zer
            enemiesText.text = enemiesFelled.ToString();
        }
    }
    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        if (newGameState == GameState.GAME)
        {
            inGameCanvas.enabled = true;
            levelCompletedCanvas.enabled = false;
            pauseMenuCanvas.enabled = false;
            optionsCanvas.enabled = false;
            Time.timeScale = 1;
        }
        else if (newGameState==GameState.OPTIONS)
        {
            optionsCanvas.enabled = true;
            inGameCanvas.enabled = false;
            levelCompletedCanvas.enabled = false;
            pauseMenuCanvas.enabled = false;
            graphicQualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
            Time.timeScale = 0;
        }
        else if (newGameState == GameState.PAUSE_MENU)
        {
            pauseMenuCanvas.enabled = true;
            levelCompletedCanvas.enabled = false;
            inGameCanvas.enabled = false;
            optionsCanvas.enabled = false;
            Time.timeScale = 0;
        }
        else if (newGameState == GameState.LEVEL_COMPLETED)
        {
            levelCompletedCanvas.enabled = true;
            inGameCanvas.enabled = false;
            pauseMenuCanvas.enabled = false;
            optionsCanvas.enabled = false;
            Time.timeScale = 0;
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name == "Level 1 Grupa 93")
            { 
                int highScore = PlayerPrefs.GetInt(keyHighScore);
                if (score > highScore)
                {
                    highScore = score;
                    PlayerPrefs.SetInt(keyHighScore, score);
                }
                if (score < 10)
                {
                    endingScoreText.text = ("Your score = 00" + score.ToString());
                    // dodaj dwa zera
                }
                else if (score < 100)
                {
                    // dodaj jedno zero
                    endingScoreText.text = ("Your score = 0" + score.ToString());
                }
                else
                {
                    // nie dodawaj zer
                    endingScoreText.text = ("Your score = " + score.ToString());
                }
                if (highScore < 10)
                {
                    highScoreText.text = ("The best score = 00" + highScore.ToString());
                    // dodaj dwa zera
                }
                else if (highScore < 100)
                {
                    // dodaj jedno zero
                    highScoreText.text = ("The best score = 0" + highScore.ToString());
                }
                else
                {
                    // nie dodawaj zer
                    highScoreText.text = ("The best score = "+highScore.ToString());
                }

            }
        }
        else
        {
            levelCompletedCanvas.enabled = false;
            inGameCanvas.enabled = false;
            pauseMenuCanvas.enabled = false;
            optionsCanvas.enabled = false;
            Time.timeScale = 1;
        }
    }

    void Options()
    {
        SetGameState(GameState.OPTIONS);
    }
    void PauseMenu()
    {
        SetGameState(GameState.PAUSE_MENU);
    }

    void InGame()
    {
        SetGameState(GameState.GAME);
    }

    public void LevelCompleted()
    {
        SetGameState(GameState.LEVEL_COMPLETED);
    }
    public void GameOver()
    {
        SetGameState(GameState.LEVEL_COMPLETED);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        volumeSlider.onValueChanged.AddListener(SetVolume);
        keysArray[0].color = new Color(0.81f, 0.29f, 0.29f, 0.5f);
        keysArray[1].color = new Color(0.1f, 1.0f, 0, 0.5f);
        keysArray[2].color = new Color(0.24f, 0.44f, 0.89f, 0.5f);
        if (!PlayerPrefs.HasKey(keyHighScore))
        {
            PlayerPrefs.SetInt(keyHighScore, 0);
        }


        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Duplicated Game Manager ", gameObject);
        }
        levelCompletedCanvas.enabled = false;
        optionsCanvas.enabled = false;
        pauseMenuCanvas.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        //timerText.text = "timer: " + timer.ToString() + "   timer/60: " + (timer /60).ToString() + "   timer%60: " + (timer%60).ToString();
        timerText.text = string.Format("{0:00}:{1:00}", (int)(timer / 60), (int)timer % 60);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGameState == GameState.GAME)
            {
                PauseMenu();
                //Debug.Log("Pause Menu");
            }
            else if (currentGameState == GameState.PAUSE_MENU)
            {
                InGame();
                //Debug.Log("In Game");
            }
        }
    }

    public void OnResumeButtonClicked()
    {
        InGame();
    }

    public void OnExitToMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnOptionsButtonClicked()
    {
        Options();
    }

    public void OnPlusButtonClicked()
    {
        int a = QualitySettings.GetQualityLevel();
        if(a < 5)
        {
            QualitySettings.SetQualityLevel(a+1);
        }

        graphicQualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }
    public void OnMinusButtonClicked()
    {
        int a = QualitySettings.GetQualityLevel();
        if (a > 0)
        {
            QualitySettings.SetQualityLevel(a - 1);
        }
        graphicQualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public int GetPoints()
    {
        return score;
    }

    public void setCongratulate(String a)
    {
        congText.text = a;
    }


}
