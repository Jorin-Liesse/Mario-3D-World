using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private RectTransform _start;
    [SerializeField] private RectTransform _inGame;
    [SerializeField] private RectTransform _gameOver;
    [SerializeField] private RectTransform _end;
    [SerializeField] private RectTransform _info;
    [SerializeField] private AudioSource _themeSound = null;
    [SerializeField] private AudioSource _macarena = null;
    [SerializeField] private GameObject[] _inGameObjects;

    [HideInInspector] public enum Screen { Start, InGame, GameOver, End, Info }
    [HideInInspector] public Screen CurrentScreen = Screen.Start; // Start, InGame, GameOver, End
    private Screen _preScreen = Screen.Start;

    void Start()
    {
        foreach (GameObject obj in _inGameObjects)
        {
            obj.SetActive(false);
        }

        _themeSound.Play();
        _macarena.Stop();
    }
    void Update()
    {
        CheckCurrentScreen();

        if(CurrentScreen == Screen.Start)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ShowInfo();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                StartGame();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                QuitGame();
            }
        }
        else if(CurrentScreen == Screen.End) 
        {
            if (Input.GetKeyDown(KeyCode.M)) 
            {
                BackToMainMenu();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentScreen == Screen.Start)
            {
                QuitGame();
            }
            else
            {
                ResetLevel();
                BackToMainMenu();
            }
        }

        if (_preScreen != CurrentScreen)
        {
            if (CurrentScreen == Screen.InGame || CurrentScreen == Screen.End) {
                foreach (GameObject obj in _inGameObjects)
                {
                    obj.SetActive(true);
                }
            }

            else {
                foreach (GameObject obj in _inGameObjects)
                {
                    obj.SetActive(false);
                }
            }

            if (CurrentScreen == Screen.End)
            {
                _themeSound.Stop();
                _macarena.Play();
            }
        }

        _preScreen = CurrentScreen;
    }

    private void CheckCurrentScreen() {
        switch (CurrentScreen)
        {
            case Screen.Start:
                _start.gameObject.SetActive(true);
                _inGame.gameObject.SetActive(false);
                _gameOver.gameObject.SetActive(false);
                _end.gameObject.SetActive(false);
                _info.gameObject.SetActive(false);
                break;
            case Screen.InGame:
                _start.gameObject.SetActive(false);
                _inGame.gameObject.SetActive(true);
                _gameOver.gameObject.SetActive(false);
                _end.gameObject.SetActive(false);
                _info.gameObject.SetActive(false);
                break;
            case Screen.GameOver:
                _start.gameObject.SetActive(false);
                _inGame.gameObject.SetActive(false);
                _gameOver.gameObject.SetActive(true);
                _end.gameObject.SetActive(false);
                _info.gameObject.SetActive(false);
                break;
            case Screen.End:
                _start.gameObject.SetActive(false);
                _inGame.gameObject.SetActive(false);
                _gameOver.gameObject.SetActive(false);
                _end.gameObject.SetActive(true);
                _info.gameObject.SetActive(false);
                break;
            case Screen.Info:
                _start.gameObject.SetActive(false);
                _inGame.gameObject.SetActive(false);
                _gameOver.gameObject.SetActive(false);
                _end.gameObject.SetActive(false);
                _info.gameObject.SetActive(true);
                break;
        }
    }

    public void StartGame()
    {
        CurrentScreen = Screen.InGame;
    }

    public void GameOver()
    {
        CurrentScreen = Screen.GameOver;
    }

    public void EndGame()
    {
        CurrentScreen = Screen.End;
    }

    public void ShowInfo()
    {
        CurrentScreen = Screen.Info;
    }

    public void BackToMainMenu()
    {
        CurrentScreen = Screen.Start;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
