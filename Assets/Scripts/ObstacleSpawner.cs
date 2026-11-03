using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public Item itemToSpawn;
    public float speed = 0.01f;
    public List<Item> items = new List<Item>();
    public Transform parent;
    public float maxHorizontal;
    public float minHorizontal;
    public float maxVertical;
    public float minVertical;
    public float spawnZ = 900f;

    [SerializeField] private Item playerItem;
    [SerializeField] private RunnerHealth playerHealth;
    [SerializeField] private Vector3 hitRange = new Vector3(1f, 1f, 20f);

    private int nextLane;

    private void Start()
    {
        InvokeRepeating(nameof(BeginSpawn), 1, 0.3f);
    }

    private void BeginSpawn()
    {
        var spawnedItem = Instantiate(itemToSpawn, parent);
        spawnedItem.itemPosition = GetRandomLocation();
        items.Add(spawnedItem);
    }

    private Vector3 GetRandomLocation()
    {
        float laneSpacing = (maxHorizontal - minHorizontal) / 2f;
        float x = minHorizontal + nextLane * laneSpacing;

        nextLane++;

        if (nextLane > 2)
        {
            nextLane = 0;
        }

        return new Vector3(x, minVertical, spawnZ);
    }

    private void LateUpdate()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            Item item = items[i];
            float previousZ = item.itemPosition.z;

            ItemMover(item);

            Vector3 difference = item.itemPosition - playerItem.itemPosition;

            float nearestZ = Mathf.Min(previousZ, item.itemPosition.z);
            float farthestZ = Mathf.Max(previousZ, item.itemPosition.z);

            bool withinDepth = nearestZ <= playerItem.itemPosition.z + hitRange.z && farthestZ >= playerItem.itemPosition.z - hitRange.z;
            bool hit = withinDepth && Mathf.Abs(difference.x) < hitRange.x && Mathf.Abs(difference.y) < hitRange.y;

            if (hit)
            {
                playerHealth.TakeDamage();
            }

            if (hit || item.itemPosition.z <= -100f)
            {
                items.RemoveAt(i);
                Destroy(item.gameObject);
            }
        }
    }

    private void ItemMover(Item item)
    {
        item.itemPosition.z -= speed * Time.deltaTime;
    }
}