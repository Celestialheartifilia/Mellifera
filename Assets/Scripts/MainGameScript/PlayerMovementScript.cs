using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float speed = 8f;
    public Camera cam;

    [Header("Direction animation objects")]
    public GameObject frontObj;
    public GameObject backObj;
    public GameObject leftObj;
    public GameObject rightObj;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (cam == null)
            cam = Camera.main;

        ShowOnly(frontObj); // start facing front
    }

    void FixedUpdate()
    {
        // Get mouse world position EVERY frame
        Vector3 mouse = Input.mousePosition;
        Vector3 world = cam.ScreenToWorldPoint(mouse);

        Vector2 targetPos = new Vector2(world.x, world.y);
        Vector2 current = rb.position;

        // Move toward mouse
        Vector2 newPos = Vector2.MoveTowards(current, targetPos, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        Vector2 dir = targetPos - current;

        // Direction animation
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0) ShowOnly(rightObj);
            else ShowOnly(leftObj);
        }
        else
        {
            if (dir.y > 0) ShowOnly(backObj);
            else ShowOnly(frontObj);
        }
    }

    void ShowOnly(GameObject obj)
    {
        if (frontObj) frontObj.SetActive(obj == frontObj);
        if (backObj) backObj.SetActive(obj == backObj);
        if (leftObj) leftObj.SetActive(obj == leftObj);
        if (rightObj) rightObj.SetActive(obj == rightObj);
    }



}