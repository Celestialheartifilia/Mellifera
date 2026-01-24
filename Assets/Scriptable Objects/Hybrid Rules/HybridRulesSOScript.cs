using UnityEngine;

[CreateAssetMenu(fileName = "HybridRulesSOScript", menuName = "Scriptable Objects/HybridRulesSOScript")]
public class HybridRulesSOScript : ScriptableObject
{
    [System.Serializable]
    public class Rule
    {
        public ItemsSOScript flowerA;
        public ItemsSOScript flowerB;
        public ItemsSOScript resultHybrid;
    }

    public Rule[] rules;

    public ItemsSOScript GetHybridResult(ItemsSOScript a, ItemsSOScript b)
    {
        foreach (var rule in rules)
        {
            if ((rule.flowerA == a && rule.flowerB == b) ||
                (rule.flowerA == b && rule.flowerB == a))
            {
                return rule.resultHybrid;
            }
        }
        return null;
    }
}
