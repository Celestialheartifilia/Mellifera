using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float speed = 8f;
    public SpriteRenderer background;

    [Header("Direction objects (drag from Hierarchy)")]
    public GameObject frontObj;
    public GameObject backObj;
    public GameObject leftObj;
    public GameObject rightObj;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>(); // IMPORTANT
        ShowOnly(frontObj);
    }

    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (rb != null)
            rb.velocity = new Vector2(x * speed, y * speed);

        // Swap visible direction object
        if (x < 0) ShowOnly(leftObj);
        else if (x > 0) ShowOnly(rightObj);
        else if (y < 0) ShowOnly(frontObj);
        else if (y > 0) ShowOnly(backObj);

        // Optional clamp
        if (background == null || rb == null) return;

        Bounds b = background.bounds;
        float clampedX = Mathf.Clamp(rb.position.x, b.min.x, b.max.x);
        float clampedY = Mathf.Clamp(rb.position.y, b.min.y, b.max.y);
        rb.position = new Vector2(clampedX, clampedY);
    }

    void ShowOnly(GameObject obj)
    {
        if (frontObj) frontObj.SetActive(obj == frontObj);
        if (backObj) backObj.SetActive(obj == backObj);
        if (leftObj) leftObj.SetActive(obj == leftObj);
        if (rightObj) rightObj.SetActive(obj == rightObj);
    }
}
