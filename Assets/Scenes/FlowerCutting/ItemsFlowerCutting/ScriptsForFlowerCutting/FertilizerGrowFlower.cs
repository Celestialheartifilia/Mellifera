using System.Collections;
using UnityEngine;

public class FertilizerGrowFlower : MonoBehaviour
{
    [Header("References")]
    public GameObject emptyShovelPreview;
    public GameObject soilShovelTool;

    public Pot pot;

    Vector3 soilShovelStartPos;

    void Start()
    {
        emptyShovelPreview.SetActive(false);
        soilShovelTool.SetActive(false);

        soilShovelStartPos = soilShovelTool.transform.position;
    }

    bool fertiliserUsed = false;

    public void DisableFertiliser()
    {
        fertiliserUsed = true;
    }

    bool CanUse()
    {
        return pot != null &&
               pot.growthState == Pot.FlowerGrowthState.Planted &&
               !fertiliserUsed;
    }

    void OnMouseEnter()
    {
        if (!CanUse()) return;

        emptyShovelPreview.SetActive(true);
    }

    void OnMouseExit()
    {
        emptyShovelPreview.SetActive(false);
    }

    void OnMouseDown()
    {
        if (!CanUse()) return;

        emptyShovelPreview.SetActive(false);

        soilShovelTool.transform.position = soilShovelStartPos;
        soilShovelTool.SetActive(true);
    }

    public void ResetTool()
    {
        soilShovelTool.SetActive(false);
        emptyShovelPreview.SetActive(false);
    }

    public void ResetFertiliserState()
    {
        fertiliserUsed = false;
    }


}
