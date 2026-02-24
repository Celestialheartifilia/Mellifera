using UnityEngine;
using UnityEngine.SceneManagement;




public class StationSwapManagent : MonoBehaviour
{

    [SerializeField] private string sceneName;

    void OnMouseDown()
    {
        SceneManager.LoadScene(sceneName);
    }

}
