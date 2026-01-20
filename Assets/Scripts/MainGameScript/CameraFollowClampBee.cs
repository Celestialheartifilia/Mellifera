using UnityEngine;

// This script makes the camera follow a target (your bee)
// while clamping its movement within the background sprite
public class CameraFollowClampBee : MonoBehaviour
{
    /* Object follow Bee */
    public Transform target;

    /* The background sprite that defines the movement boundaries */
    public SpriteRenderer background;

    /* How smooth the camera movement is */
    // smooth timeee
    public float smoothTime = 0.12f;

    //Smooth to keep track of velocity
    float velocityX;

    // LateUpdate runs AFTER all movement =
    void LateUpdate()
    {
        // Safety check: stop if target or background is missing
        if (target == null || background == null) return;

        // Get the Camera component attached to this GameObject
        Camera cam = GetComponent<Camera>();

        // Half of the camera's visible height (orthographic camera)
        float halfHeight = cam.orthographicSize;

        // Half of the camera's visible width
        // Aspect = screen width / height
        float halfWidth = halfHeight * cam.aspect;

        // Get the world-space bounds of the background sprite
        Bounds b = background.bounds;

        // Calculate the left boundary where the camera can move
        // Add halfWidth so camera doesn't show empty space
        float minX = b.min.x + halfWidth;

        // Calculate the right boundary where the camera can move
        float maxX = b.max.x - halfWidth;

        // Clamp the target's X position so the camera stays in bounds
        float targetX = Mathf.Clamp(target.position.x, minX, maxX);

        // Smoothly move the camera toward the target X position
        float smoothX = Mathf.SmoothDamp(
            transform.position.x,   // current camera X
            targetX,                // target camera X
            ref velocityX,          // velocity reference (required)
            smoothTime              // smooth duration
        );

        // Apply the new position
        // Y and Z stay unchanged (2D side-scrolling)
        transform.position = new Vector3(
            smoothX,
            transform.position.y,
            transform.position.z
        );
    }
}
