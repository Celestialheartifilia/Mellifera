using UnityEngine;

public class CustomerScript : MonoBehaviour
{
    public float speed = 2f;
    public Transform counterPoint;
    public GameObject orderUI;

    private bool reachedCounter = false;

    void Start()
    {
        if (orderUI != null)
            orderUI.SetActive(false);
    }

    void Update()
    {
        if (reachedCounter) return;

        // Move towards counter
        transform.position = Vector2.MoveTowards(
            transform.position,
            counterPoint.position,
            speed * Time.deltaTime
        );

        // Check if arrived
        if (Vector2.Distance(transform.position, counterPoint.position) < 0.05f)
        {
            reachedCounter = true;
            ShowOrder();
        }
    }

    void ShowOrder()
    {
        if (orderUI != null)
            orderUI.SetActive(true);
    }
}
