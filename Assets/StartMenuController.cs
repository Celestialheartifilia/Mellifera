using UnityEngine.SceneManagement;
using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    public void OnStartClick()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#endif
        Application.Quit();
    }
}
