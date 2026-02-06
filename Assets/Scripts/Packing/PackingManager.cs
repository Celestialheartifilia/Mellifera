using UnityEngine;
using UnityEngine.UI;

public class PackingManager : MonoBehaviour
{
    public Transform buttonParent;
    public Button buttonPrefab;
    public Transform flowerSpawnPoint;
    public GameObject packingFlowerPrefab;

    void Start()
    {
        foreach (ItemsSOScript flower in GameState.Instance.createdFlowers)
        {
            Button btn = Instantiate(buttonPrefab, buttonParent);
            btn.GetComponentInChildren<Text>().text = flower.itemName;

            btn.onClick.AddListener(() =>
            {
                SpawnFlower(flower);
            });
        }
    }

    void SpawnFlower(ItemsSOScript flower)
    {
        GameObject f = Instantiate(packingFlowerPrefab, flowerSpawnPoint.position, Quaternion.identity);
        f.GetComponent<SpriteRenderer>().sprite = flower.itemSprite;
    }

}
