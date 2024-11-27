using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Singleton dla �atwego dost�pu do LevelManagera
    public static LevelManager Instance;

    private void Awake()
    {
        // Zapewniamy, �e istnieje tylko jeden LevelManager w grze
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Nie usuwaj obiektu przy prze�adowaniu sceny
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Funkcja do przej�cia do kolejnego poziomu
    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Nie ma wi�cej poziom�w. Wracamy do menu g��wnego.");
            LoadMainMenu();
        }
    }

    // Funkcja do prze�adowania obecnego poziomu
    public void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    // Funkcja do za�adowania menu g��wnego
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Upewnij si�, �e scena o nazwie "MainMenu" istnieje
    }

    // Funkcja do za�adowania poziomu o okre�lonym indeksie
    public void LoadLevelByIndex(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(levelIndex);
        }
        else
        {
            Debug.LogError("Nieprawid�owy indeks poziomu!");
        }
    }

    // Funkcja do zako�czenia gry
    public void QuitGame()
    {
        Debug.Log("Wyj�cie z gry.");
        Application.Quit();
    }
}
