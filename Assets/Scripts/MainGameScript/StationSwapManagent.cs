using UnityEngine;
using UnityEngine.SceneManagement;

public class StationSwapManagent : MonoBehaviour
{
    public void LoadHybridScene()
    {
        SceneManager.LoadScene("HybridingFlowerScene");
    }

    public void LoadPackagingScene()
    {
        SceneManager.LoadScene("PackingScene");
    }

    public void LoadCustomerOrderScene()
    {
        SceneManager.LoadScene("OrderTakingScene");
    }
}
