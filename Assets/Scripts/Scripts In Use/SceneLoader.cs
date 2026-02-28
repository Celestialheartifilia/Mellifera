using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void LoadMainGameScene()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void LoadOrderTakingScene()
    {
        SceneManager.LoadScene("OrderTakingScene");
    }

    public void LoadHybridingFlowerScene()
    {
        SceneManager.LoadScene("HybridingFlowerScene");
    }

    public void LoadPackingScene()
    {
        SceneManager.LoadScene("PackingScene");
    }

    public void LoadOptionMenuScene()
    {
        SceneManager.LoadScene("OptionMenuScene");
    }
}
