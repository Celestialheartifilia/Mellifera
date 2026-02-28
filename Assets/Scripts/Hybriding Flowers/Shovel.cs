using UnityEngine;

public class Shovel : MonoBehaviour
{
    public Pot pot;
    public Collider2D potCollider;
    public FertilizerGrowFlower fertilizerManager;

    bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (other != potCollider) return;

        triggered = true;

        bool success = pot.Fertilise();

        if (success)
        {
            fertilizerManager.ResetTool();
            fertilizerManager.DisableFertiliser();
        }

        triggered = false;
    }
}
