using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Collections;

public class BeeController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public PollinationManager pollinationManager;
    public Pot currentPot;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private NormalFlower currentFlower;

    [Header("Visual Indicators")]
    public GameObject NeedToPollinate;
    public GameObject FlowerPollinatedAlready;
    public GameObject PressSpacebarToPlant;
    public GameObject PlantedSuccessfully;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        //Set false to not Appear first 
        NeedToPollinate.SetActive(false);
        FlowerPollinatedAlready.SetActive(false);
        PressSpacebarToPlant.SetActive(false);
        PlantedSuccessfully.SetActive(false);
    }

    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPollinate();
        }
            

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryPlant();
            StartCoroutine(ShowForSeconds(PlantedSuccessfully, 0.5f));
        }
            
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    void TryPollinate()
    {
        if (currentFlower == null)
        {
            Debug.Log("Not on flower");
            return;
        }
        if (currentFlower.isPollinated)
        {
            StartCoroutine(ShowForSeconds(FlowerPollinatedAlready, 0.5f));
            Debug.Log("Flower is already pollinated");
            return;
        }

        bool accepted = pollinationManager.TryAddPollinatedFlower(currentFlower.flowerData);
        if (accepted)
        {
            currentFlower.SetPollinated(true);
            Debug.Log(currentFlower);
        }
    }

    //Show for a few seconds
    IEnumerator ShowForSeconds(GameObject obj, float seconds)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);
    }

    void TryPlant()
    {
        if (currentPot == null) 
        {
            return;
        }

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
            StartCoroutine(ShowForSeconds(NeedToPollinate, 0.5f));
        }

        Pot pot = other.GetComponentInParent<Pot>();
        if (pot != null)
        {
            currentPot = pot;
            Debug.Log("Detected pot");
            StartCoroutine(ShowForSeconds(PressSpacebarToPlant, 1f));
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
