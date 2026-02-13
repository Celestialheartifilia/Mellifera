using UnityEngine;

public class HybridGuideUI : MonoBehaviour
{
    public HybridRulesSOScript hybridRules;

    [System.Serializable]
    public class GuideSlot
    {
        public SpriteRenderer flowerASprite;
        public SpriteRenderer flowerBSprite;
        public SpriteRenderer resultSprite;
    }

    public GuideSlot[] guideSlots;

    public void DisplayGuide()
    {
        if (hybridRules == null)
        {
            Debug.LogError("HybridRules not assigned.");
            return;
        }

        for (int i = 0; i < guideSlots.Length; i++)
        {
            if (i >= hybridRules.rules.Length)
            {
                guideSlots[i].flowerASprite.gameObject.SetActive(false);
                guideSlots[i].flowerBSprite.gameObject.SetActive(false);
                guideSlots[i].resultSprite.gameObject.SetActive(false);
                continue;
            }

            var rule = hybridRules.rules[i];

            guideSlots[i].flowerASprite.sprite = rule.flowerA.itemSprite;
            guideSlots[i].flowerBSprite.sprite = rule.flowerB.itemSprite;
            guideSlots[i].resultSprite.sprite = rule.resultHybrid.itemSprite;

            guideSlots[i].flowerASprite.gameObject.SetActive(true);
            guideSlots[i].flowerBSprite.gameObject.SetActive(true);
            guideSlots[i].resultSprite.gameObject.SetActive(true);
        }
    }
}
