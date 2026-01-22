using UnityEngine;

public class BeeMovement : MonoBehaviour
{
    public float moveSpeed = 15f;

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f);

        // Move the bee relative to its current positio
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
}
