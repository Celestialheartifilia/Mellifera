using UnityEngine;
using UnityEngine.UI;

public class HybridUIManager : MonoBehaviour
{
    public BeeController beeController;
    public Button fertiliserButton;

    void Update()
    {
        fertiliserButton.interactable =
            beeController.currentPot != null &&
            beeController.currentPot.IsReadyToFertilise();
    }

    public void OnFertiliserClicked()
    {
        if (beeController.currentPot == null)
        {
            return;
        }
        beeController.currentPot.Fertilise();
    }
}
