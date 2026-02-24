using UnityEngine;

public class HoverHighlight : MonoBehaviour
{
    SpriteRenderer sr;
    Color originalColor;

    [Header("Highlight Settings")]
    public Color highlightColor = Color.yellow;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    void OnMouseEnter()
    {
        sr.color = highlightColor; // highlight when mouse over
    }

    void OnMouseExit()
    {
        sr.color = originalColor; // return to normal
    }
}