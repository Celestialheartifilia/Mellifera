using UnityEngine;

/*Controls the bee movement prevents the player from moving outside the background */
public class PlayerMovementScript : MonoBehaviour
{
    // Horizontal movement speed of the player
    public float speed = 8f;

    // Background sprite used to define movement boundaries
    public SpriteRenderer background;

    // Reference to the Rigidbody2D component
    Rigidbody2D rb;

    // Awake runs once when the object is created
    // Good place to cache component references
    void Awake()
    {
        // Get the Rigidbody2D attached to this GameObject from monoscript :>
        rb = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate is used for physics-related movement
    void FixedUpdate()
    {
        // Get horizontal input:
        // -1 = left, 0 = no input, 1 = right
        float x = Input.GetAxisRaw("Horizontal");

        // Move the player horizontally using physics
        // Y velocity is preserved so gravity/jumping still works
        rb.linearVelocity = new Vector2(
            x * speed,
            rb.linearVelocity.y
        );

        // If no background is assigned, skip clamping
        if (background == null) return;

        // Get the world-space bounds of the background sprite
        Bounds b = background.bounds;

        // Clamp the player's X position so they stay inside the background
        float clampedX = Mathf.Clamp(
            rb.position.x,
            b.min.x,   // left edge of background
            b.max.x    // right edge of background
        );

        // Apply the clamped position back to the Rigidbody
        rb.position = new Vector2(
            clampedX,
            rb.position.y
        );
    }
}
