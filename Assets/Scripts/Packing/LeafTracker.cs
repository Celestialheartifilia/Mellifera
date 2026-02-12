using UnityEngine;

public class LeafTracker : MonoBehaviour
{
    int totalLeaves;
    int removedLeaves;

    PackingManager packingManager;

    void Start()
    {
        packingManager = FindObjectOfType<PackingManager>();

        totalLeaves = GetComponentsInChildren<LeafDispose>(true).Length;
        removedLeaves = 0;
    }

    public void NotifyLeafRemoved()
    {
        removedLeaves++;

        if (removedLeaves >= totalLeaves)
        {
            Debug.Log("All leaves plucked!");
            packingManager.OnLeavesPlucked();
        }
    }
}
