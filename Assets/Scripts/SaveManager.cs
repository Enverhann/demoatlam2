using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public readonly int defaultLastLevel = 1;
    private int _levelIndex;
    private static bool _loaded = false;
    private static int _currentLevel = 1;
    [SerializeField] private TextMeshProUGUI levelText;
    public GameObject player;
    Vector3 startpos = new Vector3(0, -1.36f, -10);
    private void Awake()
    {
        _levelIndex = SceneManager.GetActiveScene().buildIndex;
    }
    void Start()
    {
        levelText.text = "Level:" + _currentLevel.ToString();
        LoadGame();
        SaveGame();
        PlayerPrefs.SetInt("CurrentLevel", _currentLevel);
        player.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX"), PlayerPrefs.GetFloat("PlayerY"), PlayerPrefs.GetFloat("PlayerZ"));
    }

    private void Update()
    {
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
    }

    public void NextLevel(string newGameLevel)
    {
        //Save game and score for next level.
        _currentLevel++;

        SceneManager.LoadScene(newGameLevel);

        _levelIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Game Saved");
        player.transform.position = startpos;
    }

    public void RestartGame()
    {
        //Restart level.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadGame()
    {
        //Load game on startup.
        if (!_loaded)
        {
            _loaded = true;
            _levelIndex = PlayerPrefs.GetInt("SavedScene", defaultLastLevel);
            SceneManager.LoadScene(_levelIndex);
            _currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        }
    }
    public void SaveGame()
    {
        //Save game.
        PlayerPrefs.SetInt("SavedScene", _levelIndex);
        Debug.Log("Score Loaded");
    }

    public void ResetWholeGame()
    {
        SceneManager.LoadScene("Level1");
        _currentLevel = 1;
        player.transform.position = startpos;
    }
}
