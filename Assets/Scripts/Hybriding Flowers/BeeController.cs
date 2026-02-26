using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.LightTransport;

public class BeeController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public PollinationManager pollinationManager;
    public Pot currentPot;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("Disappear Animation")]
    public float disappearDuration = 0.25f;
    Vector3 originalScale;

    [Header("Direction animation objects")]
    public GameObject frontObj;
    public GameObject backObj;
    public GameObject leftObj;
    public GameObject rightObj;


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

        ShowOnly(frontObj); // start facing front

        //Set false to not Appear first 
        NeedToPollinate.SetActive(false);
        FlowerPollinatedAlready.SetActive(false);
        PressSpacebarToPlant.SetActive(false);
        PlantedSuccessfully.SetActive(false);

        originalScale = transform.localScale;

    }

    void Update()
    {

        // Mouse follow
        // Always auto-follow mouse
        // Always auto-follow mouse (correct ScreenToWorldPoint z)
        
        Camera mainCam = Camera.main;
        if (mainCam == null)
        {
            Debug.LogError("No MainCamera found. Tag your camera as MainCamera.");
            return;
        }

        Vector3 mp = Input.mousePosition;
        mp.z = -mainCam.transform.position.z;   // distance from camera to your 2D plane (usually 10)

        Vector3 mouseWorld = mainCam.ScreenToWorldPoint(mp);
        mouseWorld.z = 0f;

        Vector2 dir = (Vector2)(mouseWorld - transform.position);

        // optional stop when very close
        if (dir.magnitude < 0.1f) moveInput = Vector2.zero;
        else moveInput = dir.normalized;

        //------------------------------------------------

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


        // moveInput = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical")).normalized;

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


    void ShowOnly(GameObject obj)
    {
        if (frontObj) frontObj.SetActive(obj == frontObj);
        if (backObj) backObj.SetActive(obj == backObj);
        if (leftObj) leftObj.SetActive(obj == leftObj);
        if (rightObj) rightObj.SetActive(obj == rightObj);
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

        bool accepted = pollinationManager.TryAddPollinatedFlower(currentFlower);
        if (accepted)
        {
            Debug.Log("flower can be planted now" + currentFlower);
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
        if (!planted)
            return;

        Debug.Log("Planted successfully.");

        StartCoroutine(HandlePlanting());
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

    IEnumerator HandlePlanting()
    {
        yield return ShowForSeconds(PlantedSuccessfully, 0.5f);

        //gameObject.SetActive(false);

        float t = 0f;

        while (t < disappearDuration)
        {
            t += Time.deltaTime;

            float progress = 1f - (t / disappearDuration);

            transform.localScale = originalScale * progress;

            yield return null;
        }

        gameObject.SetActive(false);

        transform.localScale = originalScale; // reset for next spawn
    }
}


