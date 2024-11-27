using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Singleton dla ³atwego dostêpu do LevelManagera
    public static LevelManager Instance;

    private void Awake()
    {
        // Zapewniamy, ¿e istnieje tylko jeden LevelManager w grze
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Nie usuwaj obiektu przy prze³adowaniu sceny
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Funkcja do przejœcia do kolejnego poziomu
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
            Debug.Log("Nie ma wiêcej poziomów. Wracamy do menu g³ównego.");
            LoadMainMenu();
        }
    }

    // Funkcja do prze³adowania obecnego poziomu
    public void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    // Funkcja do za³adowania menu g³ównego
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Upewnij siê, ¿e scena o nazwie "MainMenu" istnieje
    }

    // Funkcja do za³adowania poziomu o okreœlonym indeksie
    public void LoadLevelByIndex(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(levelIndex);
        }
        else
        {
            Debug.LogError("Nieprawid³owy indeks poziomu!");
        }
    }

    // Funkcja do zakoñczenia gry
    public void QuitGame()
    {
        Debug.Log("Wyjœcie z gry.");
        Application.Quit();
    }
}
