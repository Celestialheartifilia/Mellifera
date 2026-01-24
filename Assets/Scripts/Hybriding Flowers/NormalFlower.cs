using UnityEngine;

public class NormalFlower : MonoBehaviour
{
    public ItemsSOScript flowerData;
    public bool isPollinated;

    public void SetPollinated(bool value)
    {
        isPollinated = value;
        // Later: change sprite / play effect here
    }
}
