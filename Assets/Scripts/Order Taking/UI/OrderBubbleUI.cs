using UnityEngine;

public class OrderBubbleUI : MonoBehaviour
{
    public SpriteRenderer[] itemSlots;

    public void DisplayOrder(OrderList order)
    {
        // Clear all slots first
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].sprite = null;
            itemSlots[i].gameObject.SetActive(false);
        }

        // Fill slots based on order items
        for (int i = 0; i < order.orderedItems.Count && i < itemSlots.Length; i++)
        {
            itemSlots[i].sprite = order.orderedItems[i].itemSprite;
            itemSlots[i].gameObject.SetActive(true);
        }
    }
}
