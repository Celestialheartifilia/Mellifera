using UnityEngine;

public class LeafTracker : MonoBehaviour
{
    int totalLeaves;
    int removedLeaves;
    public GameObject PluckLeaves;

    PackingManager packingManager;

    void Start()
    {
        PluckLeaves.SetActive(true);
        packingManager = FindObjectOfType<PackingManager>();

        totalLeaves = GetComponentsInChildren<LeafDispose>(true).Length;
        removedLeaves = 0;
    }

    public void NotifyLeafRemoved()
    {
        removedLeaves++;

        if (removedLeaves >= totalLeaves)
        {
            PluckLeaves.SetActive(false);
            Debug.Log("All leaves plucked!");
            packingManager.OnLeavesPlucked();
        }
    }
}
