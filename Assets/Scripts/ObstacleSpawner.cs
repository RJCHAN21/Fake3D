using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObstacleSpawner : MonoBehaviour
{
#region Public Fields
    public Item itemToSpawn;
    public float speed = 0.01f;
    public List<Item> items = new List<Item>();
    public Transform parent;
    public float maxHorizontal;
    public float minHorizontal;
    public float maxVertical;
    public float minVertical;
    public float spawnZ = 900f;
#endregion

#region Serialized Fields
    [SerializeField] private Item playerItem;
    [SerializeField] private RunnerHealth playerHealth;
    [SerializeField] private Vector3 hitRange = new Vector3(1f, 1f, 20f);
#endregion

#region Private Fields
    private int nextLane;
    private ObjectPool<Item> pool;
#endregion

#region Unity Life Cycle
    private void Awake()
    {
        pool = new ObjectPool<Item>(
            CreateObstacle,
            ActivateObstacle,
            DeactivateObstacle,
            DestroyObstacle,
            true,
            16,
            32
        );
    }

    private void Start()
    {
        InvokeRepeating(nameof(BeginSpawn), 1, 0.3f);
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
                pool.Release(item);
            }
        }
    }  

    private void OnDestroy()
    {
        pool?.Clear();
    }
#endregion

#region Spawn & Movement Logic
    private void BeginSpawn()
    {
        Item spawnedItem = pool.Get();
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

    private void ItemMover(Item item)
    {
        item.itemPosition.z -= speed * Time.deltaTime;
    }
#endregion

#region Object Pooling
    private Item CreateObstacle()
    {
        return Instantiate(itemToSpawn, parent);
    }

    private void ActivateObstacle(Item item)
    {
        item.itemPosition = GetRandomLocation();
        item.gameObject.SetActive(true);
    }

    private void DeactivateObstacle(Item item)
    {
        item.gameObject.SetActive(false);
    }

    private void DestroyObstacle(Item item)
    {
        if (item != null)
        {
            Destroy(item.gameObject);
        }
    }
#endregion
}