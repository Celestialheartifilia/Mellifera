using UnityEngine;

public class TopNavUI : MonoBehaviour
{
    public static TopNavUI instance;
    void Awake()
    {
        // If an instance already exists, destroy this one so we don't have two bars
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        // This keeps the whole Canvas and the Timer inside it alive!
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
