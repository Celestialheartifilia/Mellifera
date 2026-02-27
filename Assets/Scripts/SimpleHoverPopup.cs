using UnityEngine;

public class SimpleHoverPopup : MonoBehaviour
{
    [Header("Popup Object")]
    public GameObject popup;

    void Start()
    {
        if (popup != null)
            popup.SetActive(false);
    }

    void OnMouseEnter()
    {
        if (popup != null)
            popup.SetActive(true);
    }

    void OnMouseExit()
    {
        if (popup != null)
            popup.SetActive(false);
    }
}
