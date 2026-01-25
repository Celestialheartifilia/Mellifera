using UnityEngine;

public class FlowerCutSwap : MonoBehaviour
{
    public Sprite uncutSprite;

    [Header("Prefabs")]
    public GameObject cutFlowerPrefab;   // draggable flower
    public GameObject stemPiecePrefab;   // falling stem

    [Header("Sprites")]
    public Sprite cutFlowerSprite;       // top flower sprite
    public Sprite stemPieceSprite;       // stem sprite

    SpriteRenderer sr;
    bool cutDone;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (uncutSprite != null) sr.sprite = uncutSprite;
    }

    public void Cut()
    {
        if (cutDone) return;
        cutDone = true;

        // Hide uncut flower
        sr.enabled = false;

        // Spawn draggable cut flower
        if (cutFlowerPrefab != null)
        {
            var cutFlower = Instantiate(cutFlowerPrefab, transform.position, Quaternion.identity);

            var fsr = cutFlower.GetComponent<SpriteRenderer>();
            if (fsr != null && cutFlowerSprite != null)
                fsr.sprite = cutFlowerSprite;
        }

        // Spawn falling stem
        if (stemPiecePrefab != null)
        {
            var stem = Instantiate(stemPiecePrefab, transform.position, Quaternion.identity);

            var ssr = stem.GetComponent<SpriteRenderer>();
            if (ssr != null && stemPieceSprite != null)
                ssr.sprite = stemPieceSprite;
        }
    }
}
