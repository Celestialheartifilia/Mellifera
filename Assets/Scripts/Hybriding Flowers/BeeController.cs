using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public PollinationManager pollinationManager;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private NormalFlower currentFlower;
    private Pot currentPot;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    void Update()
    {
        moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("pollinating");
            TryPollinate();
        }
            

        if (Input.GetKeyDown(KeyCode.Space))
            TryPlant();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    void TryPollinate()
    {
        if (currentFlower == null)
        {
            Debug.Log("null current flower");
            return;
        }
        if (currentFlower.isPollinated)
        {
            Debug.Log("pollinated current flower");
            return;
        }

        bool accepted = pollinationManager.TryAddPollinatedFlower(currentFlower.flowerData);
        if (accepted)
        {
            currentFlower.SetPollinated(true);
            Debug.Log(currentFlower);
        }
    }

    void TryPlant()
    {
        if (currentPot == null) return;

        bool planted = pollinationManager.TryPlantInto(currentPot);
        if (planted)
        {
            Debug.Log("Planted successfully.");
            Debug.Log(currentPot);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        NormalFlower flower = other.GetComponentInParent<NormalFlower>();
        if (flower != null)
        {
            currentFlower = flower;
            Debug.Log("Detected flower: " + flower.name);
        }

        Pot pot = other.GetComponentInParent<Pot>();
        if (pot != null)
        {
            currentPot = pot;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        NormalFlower flower = other.GetComponentInParent<NormalFlower>();
        if (flower != null && flower == currentFlower)
        {
            currentFlower = null;
        }

        Pot pot = other.GetComponentInParent<Pot>();
        if (pot != null && pot == currentPot)
        {
            currentPot = null;
        }
    }

}
