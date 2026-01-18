using UnityEngine;

[CreateAssetMenu(fileName = "FlowerData", menuName = "Scriptable Objects/FlowerData")]
public class FlowerData : ScriptableObject
{
    public string displayName;
    // for speech bubble icons, inventory icons
    public Sprite icon;

    public int typeId;
}
