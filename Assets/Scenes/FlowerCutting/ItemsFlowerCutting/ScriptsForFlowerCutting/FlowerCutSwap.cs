using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowerCutSwap : MonoBehaviour
{
    // This is the picture of the flower BEFORE cutting
    public Sprite uncutSprite;

    [Header("Things we spawn")]
    public GameObject cutFlowerPrefab;   // The flower top that appears after cutting
    public GameObject stemPiecePrefab;   // The stem that falls down

    [Header("UI")]
    public GameObject CollectCanvas;     // The "Flower Collected!" popup

    [Header("Scene")]
    public string sceneName;             // The place we go when player presses Home

    [Header("Pictures")]
    public Sprite cutFlowerSprite;       // Picture for the cut flower
    public Sprite stemPieceSprite;       // Picture for the stem

    SpriteRenderer sr;   // Lets us change the flower picture
    bool cutDone;        // So we don't cut again and again

    void Awake()
    {
        // Get the flower picture component
        sr = GetComponent<SpriteRenderer>();

        // Show the flower BEFORE cutting
        if (uncutSprite != null)
            sr.sprite = uncutSprite;

        // Hide the popup at the start
        if (CollectCanvas != null)
            CollectCanvas.SetActive(false);
    }

    // ?? THIS is the button function (Home button will call this)
    public void LoadScene()
    {
        // If we forgot to type the scene name, show error
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("No scene name typed! Put the scene name in Inspector.");
            return;
        }

        // Go to the new scene
        SceneManager.LoadScene(sceneName);
    }

    // ?? This runs when we CUT the flower
    public void Cut()
    {
        // Stop if already cut
        if (cutDone) return;
        cutDone = true;

        // Hide the original flower
        if (sr != null)
            sr.enabled = false;

        // ?? Create the CUT flower (top part)
        if (cutFlowerPrefab != null)
        {
            var cutFlower = Instantiate(cutFlowerPrefab, transform.position, Quaternion.identity);

            // Change its picture
            var fsr = cutFlower.GetComponent<SpriteRenderer>();
            if (fsr != null && cutFlowerSprite != null)
                fsr.sprite = cutFlowerSprite;

            // Make sure we can CLICK the flower
            if (cutFlower.GetComponent<Collider2D>() == null)
                cutFlower.AddComponent<BoxCollider2D>();

            // When player clicks flower ? show popup + remove flower
            var clickHandler = cutFlower.AddComponent<CutFlowerClick_Internal>();
            clickHandler.collectCanvas = CollectCanvas;
        }

        // ?? Create the falling stem
        if (stemPiecePrefab != null)
        {
            var stem = Instantiate(stemPiecePrefab, transform.position, Quaternion.identity);

            var ssr = stem.GetComponent<SpriteRenderer>();
            if (ssr != null && stemPieceSprite != null)
                ssr.sprite = stemPieceSprite;
        }
    }

    // ?? This handles clicking the CUT flower
    private class CutFlowerClick_Internal : MonoBehaviour
    {
        public GameObject collectCanvas;

        void OnMouseDown()
        {
            // Show "Flower Collected!" popup
            if (collectCanvas != null)
                collectCanvas.SetActive(true);

            // Make the flower disappear
            Destroy(gameObject);
        }
    }
}
