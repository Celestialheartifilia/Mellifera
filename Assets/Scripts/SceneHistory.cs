using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHistory : MonoBehaviour
{
    public static SceneHistory Instance;

    private string previousScene;
    private string currentScene;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        currentScene = SceneManager.GetActiveScene().name;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        previousScene = currentScene;
        currentScene = scene.name;
    }

    public void OnExitPressed()
    {
        if (!string.IsNullOrEmpty(previousScene))
        {
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            SceneManager.LoadScene("MainMenu"); // fallback
        }
    }
}
