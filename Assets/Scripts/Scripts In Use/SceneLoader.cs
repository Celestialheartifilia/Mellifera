using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void LoadHybridingScene()
    {
        SceneManager.LoadScene("HybridingFlowerScene");
    }

    public void LoadPackingScene()
    {
        SceneManager.LoadScene("PackingScene");
    }

    public void LoadMainGameScene()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void LoadOrderTakingScene()
    {
        SceneManager.LoadScene("OrderTakingScene");
    }


}
