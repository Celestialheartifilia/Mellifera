using UnityEngine;

public class HybridGuideViewer : MonoBehaviour
{
    public GameObject guidePanel;
    public HybridGuideUI hybridGuideUI;

    bool isOpen = false;

    void Start()
    {
        guidePanel.SetActive(false);
    }

    public void ToggleGuide()
    {
        isOpen = !isOpen;
        guidePanel.SetActive(isOpen);

        if (isOpen)
        {
            hybridGuideUI.DisplayGuide();
        }
    }
}
