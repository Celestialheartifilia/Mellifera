using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    // Stores collected hybrid flower DATA
    public List<ItemsSOScript> collectedHybrids = new();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddHybrid(ItemsSOScript hybrid)
    {
        if (hybrid == null) return;

        if (!collectedHybrids.Contains(hybrid))
            collectedHybrids.Add(hybrid);
    }

    public void Clear()
    {
        collectedHybrids.Clear();
    }
}
